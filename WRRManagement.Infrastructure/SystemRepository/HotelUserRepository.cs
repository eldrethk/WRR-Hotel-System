using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WRRManagement.Domain.Base;
using WRRManagement.Domain.Hotels;
using WRRManagement.Domain.System;
using WRRManagement.Infrastructure.Data;

namespace WRRManagement.Infrastructure.SystemRepository
{
    public class HotelUserRepository : GenericRepository<HotelUser>, IHotelUser
    {
        protected new WRRContext context;
        public HotelUserRepository(WRRContext context) : base(context)
        {
            this.context = context;
        }

        public int AddUserToHotel(string userId, int hotelId)
        {
            int id = 0;
            List<ParameterInfo> parameter = new List<ParameterInfo>();
            parameter.Add(new ParameterInfo() { ParameterName = "UserID", ParameterValue = userId });
            parameter.Add(new ParameterInfo() { ParameterName="hotelId", ParameterValue=hotelId });
            try
            {
                id = DapperHelper.ExecuteQuery("dbo.genInsHotelUser", parameter, context);
            }
            catch (Exception ex) { }
            return id;
         }

        public List<Hotel> GetHotelsForUser(string userId)
        {
            List<Hotel> hotels = new List<Hotel>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="UserID", ParameterValue=userId });
            try
            {
                hotels = DapperHelper.GetRecords<Hotel>("dbo.genSelHotelsAssociatedUser", parameters, context);
            }
            catch(Exception ex) { }
            return hotels;
        }

        public List<HotelUser> GetUserForHotel(int hotelId)
        {
            List<HotelUser> list = new List<HotelUser>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="HotelID", ParameterValue=hotelId});
            try
            {
                list = DapperHelper.GetRecords<HotelUser>("dbo.genSelUserForHotel", parameters, context);
            }
            catch(Exception ex) { }
            return list;
        }
    }
}
