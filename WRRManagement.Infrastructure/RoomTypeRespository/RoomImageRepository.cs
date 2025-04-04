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
    public class RoomImageRepository : GenericRepository<RoomImage>, IRoomImage
    {
        protected new WRRContext context;
        public RoomImageRepository(WRRContext context) : base(context)
        {
            this.context = context;
        }

        public int Add(RoomImage img)
        {
           int id = 0;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="RoomTypeID", ParameterValue=img.RoomTypeID});
            parameters.Add(new ParameterInfo() { ParameterName = "Visible", ParameterValue = img.Visible });
            parameters.Add(new ParameterInfo() { ParameterName="ContentLength", ParameterValue=img.ContentLength});
            parameters.Add(new ParameterInfo() { ParameterName = "ContentType", ParameterValue = img.ContentType });
            parameters.Add(new ParameterInfo() { ParameterName = "FileName", ParameterValue = img.FileName });

            try
            {
                id = DapperHelper.ExecuteQuery("dbo.genInsRoomImage", parameters, context);
            }
            catch (Exception ex) { }
            return id;

        }

        public RoomImage Get(int id)
        {
            RoomImage img = null;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="ImageID", ParameterValue=id});
            try
            {
                img = DapperHelper.GetRecord<RoomImage>("dbo.genSelRoomImageByID", parameters, context);
            }
            catch (Exception ex) { }
            return img;
        }


        public List<RoomImage> GetRoomImages(int roomid)
        {
            List<RoomImage> images = new List<RoomImage>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="RoomID", ParameterValue=roomid});
            try
            {
                images = DapperHelper.GetRecords<RoomImage>("dbo.genSelRoomImagesByRoomID", parameters, context);
            }
            catch(Exception ex) { }
            return images;
        }

        public void Invisible(int id)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "ImageID", ParameterValue = id });
            try
            {
                DapperHelper.ExecuteQuery("dbo.genInvisibleRoomImage", parameters, context);
            }
            catch(Exception e) { }
        }

        public void SetMainImage(int id, int roomid)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="ImageID", ParameterValue=id});
            parameters.Add(new ParameterInfo() { ParameterName="RoomTypeID", ParameterValue=roomid});
            try
            {
                DapperHelper.ExecuteQuery("dbo.genUpdRoomMainImage", parameters, context);
            }
            catch(Exception e) { }  

        }

        
    }
}
