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
    public class RoomFeatureRepository : GenericRepository<RoomFeatures>, IRoomFeatures
    {
        protected new WRRContext context;
        public RoomFeatureRepository(WRRContext context) : base(context)
        {
            this.context = context;
        }

        public int Add(RoomFeatures roomFeatures)
        {
            int id = 0;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="RoomTypeID", ParameterValue=roomFeatures.RoomTypeID});
            parameters.Add(new ParameterInfo() { ParameterName = "Icon", ParameterValue = roomFeatures.Icon } );
            parameters.Add(new ParameterInfo() { ParameterName = "Feature", ParameterValue = roomFeatures.Features });
            try
            {
                id = DapperHelper.ExecuteQuery("dbo.genInsRoomFeature", parameters, context);
            }
            catch (Exception ex) { }
            return id;
        }

        public void Delete(int id)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="ID", ParameterValue=id});
            try
            {
                DapperHelper.ExecuteQuery("dbo.genDelRoomFeature", parameters, context);
            }
            catch(Exception ex) { } 

        }

        public List<RoomFeatures> GetRoomFeatures(int roomId)
        {
            List<RoomFeatures> features = new List<RoomFeatures>(); 
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="RoomTypeID", ParameterValue = roomId});
            try
            
            {
                features = DapperHelper.GetRecords<RoomFeatures>("dbo.genSelRoomFeature", parameters, context);
            }
            catch( Exception ex) { }    
            return features;
        }

        public void Update(RoomFeatures roomFeatures)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="ID", ParameterValue=roomFeatures.ID});
            parameters.Add(new ParameterInfo() { ParameterName="Icon", ParameterValue=roomFeatures.Icon});
            parameters.Add(new ParameterInfo() { ParameterName="Feature", ParameterValue=roomFeatures.Features});

            try
            {
                DapperHelper.ExecuteQuery("dbo.genUpdRoomFeature", parameters, context);

            }
            catch (Exception ex) { }

        }
    }
}
