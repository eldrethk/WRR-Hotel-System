using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WRRManagement.Domain.Amenities;
using WRRManagement.Domain.Base;
using WRRManagement.Infrastructure.Data;

namespace WRRManagement.Infrastructure.AmenityRepository
{
    public class AmenityRepository : GenericRepository<ExtraAmenity>, IExtraAmenity
    {
        protected new WRRContext context;
        public AmenityRepository(WRRContext context) : base(context)
        {
            this.context = context;
        }

        public int Add(ExtraAmenity amenity)
        {
            int id = 0;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="Name", ParameterValue=amenity.Name});
            parameters.Add(new ParameterInfo() { ParameterName="Description", ParameterValue=amenity.Description});
            parameters.Add(new ParameterInfo() { ParameterName="Tax", ParameterValue = amenity.Tax });
            parameters.Add(new ParameterInfo() { ParameterName="AmenityRate", ParameterValue=amenity.AmenityRate});
            parameters.Add(new ParameterInfo() { ParameterName="PerDay", ParameterValue=amenity.PerDay});
            parameters.Add(new ParameterInfo() { ParameterName="PerDayPerPerson", ParameterValue = amenity.PerDayPerPerson });
            parameters.Add(new ParameterInfo() { ParameterName="OneTimeFee", ParameterValue=amenity.OneTimeFee});
            parameters.Add(new ParameterInfo() { ParameterName="OneTimeFeePerPerson", ParameterValue=amenity.OneTimeFeePerson});
            parameters.Add(new ParameterInfo() { ParameterName="PerNightStay", ParameterValue=amenity.PerNightStay});
            parameters.Add(new ParameterInfo() { ParameterName="ViewRate", ParameterValue=amenity.ViewRate});
            parameters.Add(new ParameterInfo() { ParameterName="Mandatory", ParameterValue = amenity.Mandatory });
            parameters.Add(new ParameterInfo() { ParameterName="Visible", ParameterValue = 1});
            parameters.Add(new ParameterInfo() { ParameterName="HotelID", ParameterValue=amenity.HotelID}); 
            parameters.Add(new ParameterInfo() { ParameterName="Discount", ParameterValue=amenity.Discount});   
            parameters.Add(new ParameterInfo() { ParameterName="PictureUrl", ParameterValue=amenity.PictureUrl});
            parameters.Add(new ParameterInfo() { ParameterName="ViewOnRR", ParameterValue=amenity.ViewOnRackRate});
            parameters.Add(new ParameterInfo() { ParameterName="DiscountRegRate", ParameterValue=amenity.DiscountRegularRate});
            parameters.Add(new ParameterInfo() { ParameterName="ShortDesc", ParameterValue=amenity.ShortDescription});
            try
            {
                id = DapperHelper.ExecuteQuery("dbo.genInsExtraAmenity", parameters, context);
            }
            catch(Exception ex) { }
            return id;
        }

        public List<ExtraAmenity> GetAllForHotel(int hotelId)
        {
            List<ExtraAmenity> amenities = new List<ExtraAmenity>();
            List<ParameterInfo> parameters = new List<ParameterInfo>(); 
            parameters.Add(new ParameterInfo() { ParameterName="HotelID", ParameterValue= hotelId});
            try
            {
                amenities = DapperHelper.GetRecords<ExtraAmenity>("dbo.genSelExtraAmenity", parameters, context);
            }
            catch(Exception ex) { }
            return amenities;
        }

        public ExtraAmenity GetAmenity(int id)
        {
            ExtraAmenity amenity = null;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="AmenityID", ParameterValue = id});
            try
            {
                amenity = DapperHelper.GetRecord<ExtraAmenity>("dbo.genSelExtraAmenityByID", parameters, context);
            }
            catch(Exception e) { }
            return amenity;

        }

        public List<ExtraAmenity> GetPackageAmenities(int packageId)
        {
            List<ExtraAmenity> amenities = new List<ExtraAmenity>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "PackageID", ParameterValue = packageId });
            try
            {
                amenities = DapperHelper.GetRecords<ExtraAmenity>("dbo.genSelExtraAmenitiesByPackageID", parameters, context);
            }
            catch (Exception ex) { }
            return amenities;
        }

        public List<ExtraAmenity> GetRackRateAmenities(int hotelId)
        {
            List<ExtraAmenity> amenities = new List<ExtraAmenity>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "HotelID", ParameterValue = hotelId });
            try
            {
                amenities = DapperHelper.GetRecords<ExtraAmenity>("dbo.genSelRackRateAmenities", parameters, context);
            }
            catch (Exception ex) { }
            return amenities;
        }

        public void remove(int id)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="AmenityID", ParameterValue=id});
            try
            {
                DapperHelper.ExecuteQuery("dbo.genInvisbleExtraAmenity", parameters, context);
                DapperHelper.ExecuteQuery("dbo.genDropAmenitiesFromPackage", parameters, context);
            }
            catch(Exception ex) { }
        }

        public void Update(ExtraAmenity amenity)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "Name", ParameterValue = amenity.Name });
            parameters.Add(new ParameterInfo() { ParameterName = "Description", ParameterValue = amenity.Description });
            parameters.Add(new ParameterInfo() { ParameterName = "Tax", ParameterValue = amenity.Tax });
            parameters.Add(new ParameterInfo() { ParameterName = "AmenityRate", ParameterValue = amenity.AmenityRate });
            parameters.Add(new ParameterInfo() { ParameterName = "PerDay", ParameterValue = amenity.PerDay });
            parameters.Add(new ParameterInfo() { ParameterName = "PerDayPerPerson", ParameterValue = amenity.PerDayPerPerson });
            parameters.Add(new ParameterInfo() { ParameterName = "OneTimeFee", ParameterValue = amenity.OneTimeFee });
            parameters.Add(new ParameterInfo() { ParameterName = "OneTimeFeePerPerson", ParameterValue = amenity.OneTimeFeePerson });
            parameters.Add(new ParameterInfo() { ParameterName = "PerNightStay", ParameterValue = amenity.PerNightStay });
            parameters.Add(new ParameterInfo() { ParameterName = "ViewRate", ParameterValue = amenity.ViewRate });
            parameters.Add(new ParameterInfo() { ParameterName = "Mandatory", ParameterValue = amenity.Mandatory });
            parameters.Add(new ParameterInfo() { ParameterName = "Amenity", ParameterValue = amenity.AmenityID });
            parameters.Add(new ParameterInfo() { ParameterName = "Discount", ParameterValue = amenity.Discount });
            parameters.Add(new ParameterInfo() { ParameterName = "PictureUrl", ParameterValue = amenity.PictureUrl });
            parameters.Add(new ParameterInfo() { ParameterName = "ViewOnRR", ParameterValue = amenity.ViewOnRackRate });
            parameters.Add(new ParameterInfo() { ParameterName = "DiscountRegRate", ParameterValue = amenity.DiscountRegularRate });
            parameters.Add(new ParameterInfo() { ParameterName = "ShortDesc", ParameterValue = amenity.ShortDescription });
            try
            {
                DapperHelper.ExecuteQuery("dbo.genUpdExtraAmenity", parameters, context);
            }
            catch (Exception ex) { }
        }
    }
}
