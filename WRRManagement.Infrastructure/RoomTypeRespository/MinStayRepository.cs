using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WRRManagement.Domain.Base;
using WRRManagement.Domain.RoomTypes;
using WRRManagement.Infrastructure.Data;

namespace WRRManagement.Infrastructure.RoomTypeRespository
{
    public class MinStayRepository : GenericRepository<MinStay>, IMinStay
    {
        protected new WRRContext context;
        public MinStayRepository(WRRContext context) : base(context)
        {
            this.context = context;
        }

        public int Add(int RoomTypeID, DateTime StayDate, int Qty)
        {
            int id = 0;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="RoomID", ParameterValue = RoomTypeID });
            parameters.Add(new ParameterInfo() { ParameterName="Date", ParameterValue= StayDate });
            parameters.Add(new ParameterInfo() { ParameterName = "Quantity", ParameterValue = Qty } );

            try
            {
                id = DapperHelper.ExecuteQuery("dbo.genInsMinStay", parameters, context);
            }
            catch (Exception ex) { }
            return id;
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

        public MinStay GetById(int id)
        {
            MinStay minStay = null;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="MinStayID", ParameterValue = id});

            try
            {
                minStay = DapperHelper.GetRecord<MinStay>("dbo.genSelMinStayByID", parameters, context);
            }
            catch(Exception ex) { }
            return minStay;
        }

        public List<MinStay> GetAllForRoom(int roomId)
        {
            List<MinStay> stays = new List<MinStay>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "RoomID", ParameterValue = roomId });
           
            try
            {                
                stays = DapperHelper.GetRecords<MinStay>("dbo.genSelMinStayByRoomID", parameters, context);
            }
            catch(Exception e) { }
            return stays;
        }

        public int GetQuantityForDate(int roomId, DateTime date)
        {
            int quantity = 0;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "RoomID", ParameterValue = roomId });
            parameters.Add(new ParameterInfo() { ParameterName="PickedDate", ParameterValue=date });

            try
            {
                quantity = DapperHelper.ExecuteQuery("dbo.genSelMinStayByDate", parameters, context);
            }
            catch (Exception e) { }
            return quantity;
        }
    

        public void Update(int Quantity, int minStayId)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "Quantity", ParameterValue = Quantity });
            parameters.Add(new ParameterInfo() { ParameterName="MinStayID", ParameterValue= minStayId });

            try
            {
                DapperHelper.ExecuteQuery("dbo.genUpdMinStay", parameters, context);
            }
            catch (Exception e) { }             
        }

    }
}
