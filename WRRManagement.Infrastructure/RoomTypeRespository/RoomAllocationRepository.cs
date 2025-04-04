using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WRRManagement.Domain.Base;
using WRRManagement.Domain.RoomTypes;
using WRRManagement.Infrastructure.Data;

namespace WRRManagement.Infrastructure.RoomTypeRespository
{
    public class RoomAllocationRepository : GenericRepository<RoomAllocation>, IRoomAllocation
    {
        protected new WRRContext context;
        protected new IRoomType roomRep;
        public RoomAllocationRepository(WRRContext context, IRoomType room) : base(context)
        {
            this.context = context;
            this.roomRep = room;
        }

        public int Add(int RoomTypeID, DateTime AllocationDate, int Qty)
        {
            int id = 0;
            if (RoomTypeID> 0)
            {             
                List<ParameterInfo> parameters = new List<ParameterInfo>();
                parameters.Add(new ParameterInfo() { ParameterName = "RoomID", ParameterValue = RoomTypeID });
                parameters.Add(new ParameterInfo() { ParameterName = "Date", ParameterValue = AllocationDate });
                parameters.Add(new ParameterInfo() { ParameterName = "Quantity", ParameterValue = Qty });
                try
                {
                    id = DapperHelper.ExecuteQuery("dbo.genInsAllocation", parameters, context);
                }
                catch (Exception ex) { }
            }
            return id; ;
        }

        public void AddAllocationBack(int roomtypeID, DateTime date)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "RoomTypeID", ParameterValue = roomtypeID });
            parameters.Add(new ParameterInfo() { ParameterName = "Date", ParameterValue = date });
            try
            {
                DapperHelper.ExecuteQuery("dbo.genRollBackAllocation", parameters, context);
            }
            catch (Exception ex) { }
        }

        public void AddDateRange(int RoomTypeID, DateTime start, DateTime end, int Qty)
        {
            if (start > DateTime.MinValue && end > DateTime.MinValue)
            {
                DateTime temp = start;
                while (temp <= end)
                {
                    Add(RoomTypeID, temp, Qty); 
                    temp = temp.AddDays(1);
                }
            }
          
        }

        public bool AllocationIsValid(int roomId, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public List<RoomAllocation> GetAllForRoom(int roomId)
        {
            List<RoomAllocation> allocations = new List<RoomAllocation>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="RoomId", ParameterValue=roomId});
            try
            {
                allocations = DapperHelper.GetRecords<RoomAllocation>("dbo.genSelAllocationByRoomID", parameters, context);
            }
            catch (Exception ex) { }
            return allocations;
        }

        public RoomAllocation GetByID(int allocationId)
        {
            RoomAllocation allocation = null;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="AllocationID", ParameterValue = allocationId});
            try
            {
                allocation = DapperHelper.GetRecord<RoomAllocation>("dbo.genSelAllocationByID", parameters, context);
                string roomname = roomRep.GetById(allocation.RoomTypeID).Name ?? string.Empty;
                allocation.RoomName = roomname;
            }
            catch(Exception ex) { }
            return allocation;
        }

        public int GetQuantityForDay(int roomId, DateTime date)
        {
            int quantity = 0;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="RoomID", ParameterValue= roomId});
            parameters.Add(new ParameterInfo() { ParameterName="PickedDated", ParameterValue=date });
            try
            {
                quantity = DapperHelper.ExecuteQuery("dbo.genSelAllocationByDay", parameters, context);
               
            }
            catch(Exception ex) { } 
            return quantity;
        }

        public int LowestAllocation(int roomId, DateTime start, DateTime end)
        {
            int quantity = 0;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="RoomID", ParameterValue=roomId});
            parameters.Add(new ParameterInfo() { ParameterName="Start", ParameterValue=start});
            parameters.Add(new ParameterInfo() { ParameterName="End", ParameterValue = end});

            try
            {
                quantity = DapperHelper.ExecuteQuery("dbo.genSelLowQtyForRoomAllocation", parameters, context);
            }
            catch(Exception ex) { }
            return quantity;
        }

        public void UpdateQuantity(int qty, int allocationId)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="Quantity", ParameterValue= qty });
            parameters.Add(new ParameterInfo() { ParameterName="AllocationID", ParameterValue=allocationId});
            try
            {
                DapperHelper.ExecuteQuery("dbo.genUpdAllocation", parameters, context);
            }
            catch (Exception ex) { }

        }
    }
}
