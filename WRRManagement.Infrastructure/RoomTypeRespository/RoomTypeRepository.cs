using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WRRManagement.Infrastructure.Data;
using WRRManagement.Infrastructure;
using WRRManagement.Domain.Base;
using WRRManagement.Infrastructure.RoomTypeRespository;
using WRRManagement.Domain.RoomTypes;

namespace WRRManagement.Infrastructure.RoomTypeRespository
{
    public class RoomTypeRepository : GenericRepository<RoomType>, IRoomType
    {
       
        protected new WRRContext context;

        public RoomTypeRepository(WRRContext context) : base(context) => this.context = context;

        public RoomType GetById(int id)
        {
            RoomType roomType = null;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "RoomID", ParameterValue = id });
            try
            {
                roomType = DapperHelper.GetRecord<RoomType>("dbo.genSelRoomTypeByID", parameters, context);
            }
            catch(Exception ex) { }
            return roomType;
        }
        
        public List<RoomType> GetAllForHotel(int hotelId)
        {
            List<RoomType> list = new List<RoomType>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "HotelID", ParameterValue = hotelId });
            try
            {
                list = DapperHelper.GetRecords<RoomType>("dbo.genSelRoomsByHotelID", parameters, context);
            }
            catch(Exception ex) { }
            return list;
        }


        public void Invisible(int roomId)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="RoomID", ParameterValue=roomId });
            try
            {
                DapperHelper.ExecuteQuery("dbo.genInvisibleRoomType", parameters, context);
            }
            catch(Exception ex) { }
        }

        public int Add(RoomType roomType)
        {
            int id = 0;        
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName ="HotelID", ParameterValue=roomType.HotelID});                          
            parameters.Add(new ParameterInfo() { ParameterName = "Name", ParameterValue = roomType.Name });
            parameters.Add(new ParameterInfo() { ParameterName="Desc", ParameterValue=roomType.Description});
            parameters.Add(new ParameterInfo() { ParameterName = "Adult", ParameterValue = roomType.AdultBase });
            parameters.Add(new ParameterInfo() { ParameterName="Max", ParameterValue=roomType.MaxBase});
            parameters.Add(new ParameterInfo() { ParameterName="BedType", ParameterValue=roomType.BedType});
                                  
            try
            {
                id = DapperHelper.ExecuteQuery("dbo.genInsRoomType", parameters, context);
              
            }
            catch(Exception ex) { }
            return id;
        }

        public void Update(RoomType roomType)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="RoomID", ParameterValue = roomType.RoomTypeID});
            parameters.Add(new ParameterInfo() { ParameterName="Name", ParameterValue=roomType.Name});
            parameters.Add(new ParameterInfo() { ParameterName="Desc", ParameterValue=roomType.Description });  
            parameters.Add(new ParameterInfo() { ParameterName="Adult", ParameterValue=roomType.AdultBase});
            parameters.Add(new ParameterInfo() { ParameterName="Max", ParameterValue=roomType.MaxBase });
            parameters.Add(new ParameterInfo() { ParameterName="BedType", ParameterValue=roomType.BedType });

            try
            {
                DapperHelper.ExecuteQuery("dbo.genUpdRoomType", parameters, context);
            }
            catch(Exception ex) { } 

        }

        public List<RoomType> GetAllValidRoomsForGuestCount(int hotelId, int adults, int child)
        {
            List<RoomType> allRooms = GetAllForHotel(hotelId);
            List<RoomType> validRooms = new List<RoomType>();
            int maxTotal = adults + child;
            foreach (RoomType room in allRooms)
            {
                bool valid = false;
                if (room.AdultBase == true)
                {

                    //AdultBase abase = AdultBaseRepository.Get(room.RoomTypeID);
                    AdultBase abase = new AdultBase();
                    if (adults > abase.MaxAdult)
                        valid = false;
                    if (child > abase.MaxChild)
                        valid = false;
                    if (maxTotal > abase.MaxRoomTotal) valid = false;
                }
                else
                {
                    //MaxBase mbase = MaxBaseRepository.Get(room.RoomTypeID);
                    MaxBase mbase = new MaxBase();
                    if (maxTotal > mbase.MaxBaseCount) valid = false;
                }

                if (valid == true)
                    validRooms.Add(room);
            }

            return validRooms;
        }
    }
}
