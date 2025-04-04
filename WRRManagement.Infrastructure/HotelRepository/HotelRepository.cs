using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WRRManagement.Domain.Base;
using WRRManagement.Domain.Hotels;
using WRRManagement.Infrastructure.Data;

namespace WRRManagement.Infrastructure.HotelRepository
{
    public class HotelRepository : GenericRepository<Hotel>, IHotel
    {
        protected new WRRContext context;
        public HotelRepository(WRRContext context) : base(context)
        {
            this.context = context;
        }

        public Hotel GetHotel(int id)
        {
            Hotel hotel = null;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="HotelID", ParameterValue=id});
            try
            {
                hotel = DapperHelper.GetRecord<Hotel>("dbo.genSelHotel", parameters, context);
            }
            catch (Exception ex) { }
            return hotel;
        }

        public void Update(Hotel hotel)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo { ParameterName = "HotelID", ParameterValue = hotel.HotelID } );
            parameters.Add(new ParameterInfo { ParameterName="Name", ParameterValue=hotel.Name });
            parameters.Add(new ParameterInfo { ParameterName="Address1", ParameterValue=hotel.Address1 } );
            parameters.Add(new ParameterInfo { ParameterName="Address2", ParameterValue=hotel.Address2 } );
            parameters.Add(new ParameterInfo { ParameterName = "City", ParameterValue = hotel.City });
            parameters.Add(new ParameterInfo { ParameterName="State", ParameterValue=hotel.State });
            parameters.Add(new ParameterInfo { ParameterName="Zipcode", ParameterValue=hotel.ZipCode });    
            parameters.Add(new ParameterInfo { ParameterName="FreePhone", ParameterValue=hotel.TollFreePhone });
            parameters.Add(new ParameterInfo { ParameterName="LocalPhone", ParameterValue=hotel.LocalPhone });
            parameters.Add(new ParameterInfo { ParameterName = "CheckIn", ParameterValue = hotel.CheckIn });
            parameters.Add(new ParameterInfo { ParameterName="CheckOut", ParameterValue=hotel.CheckOut });
            parameters.Add(new ParameterInfo { ParameterName="Email", ParameterValue=hotel.Email });    
            parameters.Add(new ParameterInfo { ParameterName="AdminEmail", ParameterValue=hotel.AdminEmail });
            parameters.Add(new ParameterInfo { ParameterName="Website", ParameterValue=hotel.Website });
            parameters.Add(new ParameterInfo { ParameterName="Desc", ParameterValue=hotel.Description });
            try
            {
                DapperHelper.ExecuteQuery("dbo.genUpdHotel", parameters, context);
            }
            catch(Exception ex) { }
        }
    }
}
