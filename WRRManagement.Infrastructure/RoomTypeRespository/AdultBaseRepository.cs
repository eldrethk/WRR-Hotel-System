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
    public class AdultBaseRepository : GenericRepository<AdultBase>, IAdultBase
    {
        protected new WRRContext context;
        public AdultBaseRepository(WRRContext context) : base(context)
        {
            this.context = context;
        }

        public int Add(AdultBase adultBaseFee)
        {
            int id = 0;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="RoomID", ParameterValue=adultBaseFee.RoomTypeID });
            parameters.Add(new ParameterInfo() { ParameterName="AdultBase", ParameterValue=adultBaseFee.AdultBaseCount });
            parameters.Add(new ParameterInfo() { ParameterName="ChildBase", ParameterValue=adultBaseFee.ChildBaseCount });
            parameters.Add(new ParameterInfo() { ParameterName="AdultMax", ParameterValue=adultBaseFee.MaxAdult});
            parameters.Add(new ParameterInfo() { ParameterName = "ChildMax", ParameterValue = adultBaseFee.MaxChild } );
            parameters.Add(new ParameterInfo() { ParameterName="Total", ParameterValue=adultBaseFee.MaxRoomTotal });       

            try
            {
                id = DapperHelper.ExecuteQuery("dbo.genInsAdultBase", parameters, context);
            }
            catch (Exception ex) { }
            return id;
        }

        public AdultBase GetByRoomID(int roomID)
        {
            AdultBase? adultBaseFee = null;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "RoomID", ParameterValue = roomID });
            try
            {
                adultBaseFee = DapperHelper.GetRecord<AdultBase>("dbo.genSelAdultBaseByID", parameters, context);
            }
            catch (Exception ex) { }
            return adultBaseFee;
        }
    }
}
