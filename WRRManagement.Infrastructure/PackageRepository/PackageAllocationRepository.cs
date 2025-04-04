using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WRRManagement.Domain.Base;
using WRRManagement.Domain.Packages;
using WRRManagement.Infrastructure.Data;

namespace WRRManagement.Infrastructure.PackageRepository
{
    public class PackageAllocationRepository : GenericRepository<PackageAllocation>, IPackageAllocation
    {
        protected new WRRContext context;
        public PackageAllocationRepository(WRRContext context) : base(context)
        {
            this.context = context;
        }

        public int Add(int roomid, int packageid, DateTime date, int qty)
        {
            int id = 0;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="RoomID", ParameterValue=roomid});
            parameters.Add(new ParameterInfo() { ParameterName="packageid", ParameterValue = packageid});
            parameters.Add(new ParameterInfo() { ParameterName = "Date", ParameterValue = date });
            parameters.Add(new ParameterInfo() { ParameterName="Quantity", ParameterValue=qty});
            try
            {
                id = DapperHelper.ExecuteQuery("dbo.genInsPackageAllocation", parameters, context);
            }
            catch (Exception ex) { }
            return id;  
        }

        public void AddDateRange(int roomid, int packageid, DateTime start, DateTime end, int qty)
        {
            DateTime temp = start;
            while(temp <= end)
            {
                int id = Add(roomid, packageid, temp, qty);
                temp = temp.AddDays(1);
            }
        }

        public bool AllocationIsValid(int roomid, int packageid, DateTime start, DateTime end)
        {
            bool valid = false;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "RoomID", ParameterValue = roomid });
            parameters.Add(new ParameterInfo() { ParameterName = "PackageID", ParameterValue = packageid });
            parameters.Add(new ParameterInfo() { ParameterName = "Start", ParameterValue = start });
            parameters.Add(new ParameterInfo() { ParameterName = "End", ParameterValue = end });
            try
            {
                valid = DapperHelper.ExecuteQueryBool("dbo.genVerifyRoomAvailabilityForPackageAllocation", parameters, context, "@Valid");
            }
            catch (Exception ex) { }
            return valid;
        }

        public PackageAllocation GetPackageAllocation(int AllocationID)
        {
            PackageAllocation packageAllocation = null;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "AllocationID", ParameterValue = AllocationID });
            try
            {
                packageAllocation = DapperHelper.GetRecord<PackageAllocation>("dbo.genSelPackageAllocationByID", parameters, context);
            }
            catch(Exception ex) { }
            return packageAllocation;
        }

        public List<PackageAllocation> GetPackageAllocations(int packageID, int roomTypeID)
        {
            List<PackageAllocation> packageAllocations = new List<PackageAllocation>();
            List<ParameterInfo> parameterInfos = new List<ParameterInfo>();
            parameterInfos.Add(new ParameterInfo() { ParameterName = "RoomID", ParameterValue = roomTypeID });
            parameterInfos.Add(new ParameterInfo() { ParameterName = "PackageID", ParameterValue = packageID });

            try
            {
                packageAllocations = DapperHelper.GetRecords<PackageAllocation>("dbo.genSelPackageAllocationByRoomID", parameterInfos, context);
            }
            catch(Exception e) { }
            return packageAllocations;
        }
     
        public List<Package> GetPackageWithAllocations(int hotelId)
        {
            List<Package> packageAllocation = new List<Package>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "hotelid", ParameterValue = hotelId });
            try
            {
                packageAllocation = DapperHelper.GetRecords <Package>("dbo.genSelPackagesWithAllocation", parameters, context);
            }
            catch (Exception ex) { }
            return packageAllocation;
        }

        public int LowestAllocation(int roomid, int packageid, DateTime start, DateTime end)
        {
            int Qty = 0;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="RoomID", ParameterValue=roomid });
            parameters.Add(new ParameterInfo() { ParameterName="PackageID", ParameterValue = packageid });
            parameters.Add(new ParameterInfo() { ParameterName="Start", ParameterValue=start});
            parameters.Add(new ParameterInfo() { ParameterName="End", ParameterValue=end});
            try
            {
                Qty = DapperHelper.ExecuteQueryInt("dbo.genSelLowQtyForPackageAllocation", parameters, context, "@Qty");
            }
            catch(Exception ex) { }
            return Qty;
        }

        public void RollBackAllocation(int roomid, int packageid, DateTime date)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="RoomTypeID", ParameterValue= roomid });
            parameters.Add(new ParameterInfo() { ParameterName="Date", ParameterValue=date});
            parameters.Add(new ParameterInfo() { ParameterName="PackageID", ParameterValue=packageid});
            try
            {
                DapperHelper.ExecuteQuery("dbo.genRollBackPackageAllocation", parameters, context);
            }
            catch(Exception ex) { }
        }

        public void Update(int Qty, int AllocationID)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "AllocationID", ParameterValue = AllocationID });
            parameters.Add(new ParameterInfo() { ParameterName = "Quantity", ParameterValue = Qty });
            try
            {
                DapperHelper.ExecuteQuery("dbo.genUpdPackageAllocation", parameters, context);
            }
            catch(Exception e) { }
        }

        
    }
}
