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
    public class MaxBaseRepository : GenericRepository<MaxBase>, IMaxBase
    {
        protected new WRRContext context;
        public MaxBaseRepository(WRRContext context) : base(context)
        {
            this.context = context;
        }

        public int Add(MaxBase entity)
        {
            int id = 0;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo(){ ParameterName="RoomID", ParameterValue = entity.RoomTypeID});
            parameters.Add(new ParameterInfo() { ParameterName = "BaseCount", ParameterValue = entity.BaseCount });
            parameters.Add(new ParameterInfo() { ParameterName="MaxCount", ParameterValue=entity.MaxBaseCount});

            try
            {
                id = DapperHelper.ExecuteQuery("dbo.genInsMaxBase", parameters, context);
            }
            catch (Exception ex) { }
            return id;
        }

        public MaxBase GetByRoomID(int roomid)
        {
            MaxBase entity = null;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "RoomID", ParameterValue = roomid });
            try
            {
                entity = DapperHelper.GetRecord<MaxBase>("dbo.genSelMaxBaseByID", parameters, context);

            }
            catch(Exception ex) { }
            return entity;
        }
    }
}
