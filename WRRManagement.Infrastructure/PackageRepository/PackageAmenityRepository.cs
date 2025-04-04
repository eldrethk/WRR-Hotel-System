using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WRRManagement.Domain.Amenities;
using WRRManagement.Domain.Base;
using WRRManagement.Domain.Packages;
using WRRManagement.Infrastructure.Data;

namespace WRRManagement.Infrastructure.PackageRepository
{
    public class PackageAmenityRepository : GenericRepository<PackageAmenity>, IPackageAmenity
    {
        protected new WRRContext context;
        public PackageAmenityRepository(WRRContext context) : base(context)
        {
            this.context = context;
        }

        public int Add(PackageAmenity packageAmenity)
        {
            int id = 0;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "PackageID", ParameterValue = packageAmenity.PackageID });
            parameters.Add(new ParameterInfo() { ParameterName="AmenityID", ParameterValue=packageAmenity.ExtraAmenityID });
            parameters.Add(new ParameterInfo() { ParameterName="ViewRate", ParameterValue=packageAmenity.ViewRate });
            parameters.Add(new ParameterInfo() { ParameterName = "Mandatory", ParameterValue = packageAmenity.Mandatory });
            parameters.Add(new ParameterInfo() { ParameterName="Qty", ParameterValue=packageAmenity.MandatoryQuantity });
            parameters.Add(new ParameterInfo() { ParameterName="AdditionalPurchases", ParameterValue = packageAmenity.AdditionalPurchases });
            try
            {
                id = DapperHelper.ExecuteQuery("dbo.genInsPackageAmenity", parameters, context);
            }
            catch(Exception ex) { }
            return id;
        }

        public PackageAmenity Get(int packageId, int extraAmenityId)
        {
            PackageAmenity amenity = null;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "PackageID", ParameterValue = packageId });
            parameters.Add(new ParameterInfo() { ParameterName="ExtraAmenityID", ParameterValue=extraAmenityId });
            try
            {
                amenity = DapperHelper.GetRecord<PackageAmenity>("dbo.genSelPackageAmenityByID", parameters, context);
            }
            catch (Exception ex) { }
            return amenity;
        }

        public List<PackageAmenity> GetAll(int packageId)
        {
            List<PackageAmenity> amenities =new List<PackageAmenity>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="PackageID", ParameterValue= packageId });
            try
            {
                amenities = DapperHelper.GetRecords<PackageAmenity>("dbo.genSelPackageAmenities", parameters, context);
            }
            catch(Exception e) { }
            return amenities;   
        }

        public List<ExtraAmenity> GetMandatoryAmenities(int packageId)
        {
            List<ExtraAmenity> list = new List<ExtraAmenity>();
            List<ParameterInfo> parameters=new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="PackageID", ParameterValue=packageId });
            try
            {
                list = DapperHelper.GetRecords<ExtraAmenity>("dbo.genSelMandatoryAmenitiesForPackage", parameters, context);
            }
            catch(Exception e) { }
            return list;
        }

        public List<Package> GetPackagesAssociatedWithAmenity(int extraAmenityId)
        {
            List<Package> list = new List<Package>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="amenityid", ParameterValue= extraAmenityId });
            try
            {
                list = DapperHelper.GetRecords<Package>("dbo.genSelPackageAssociatedWithAmenity", parameters, context);
            }
            catch(Exception ex) { }
            return list;
        }

        public void Remove(int extraAmenityID, int packageId)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "ExtraAmenityID", ParameterValue = extraAmenityID });
            parameters.Add(new ParameterInfo() { ParameterName = "PackageID", ParameterValue = packageId });
            try
            {
                DapperHelper.ExecuteQuery("dbo.genDelPackageAmenity", parameters, context);
            }
            catch(Exception ex) { } 
        }

        public void Update(PackageAmenity packageAmenity)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "PackageAmenityID", ParameterValue = packageAmenity.PackageAmenityID });
            parameters.Add(new ParameterInfo() { ParameterName = "ViewRate", ParameterValue = packageAmenity.ViewRate });
            parameters.Add(new ParameterInfo() { ParameterName = "Mandatory", ParameterValue = packageAmenity.Mandatory });
            parameters.Add(new ParameterInfo() { ParameterName = "Qty", ParameterValue = packageAmenity.MandatoryQuantity });
            parameters.Add(new ParameterInfo() { ParameterName = "AdditionalPurchases", ParameterValue = packageAmenity.AdditionalPurchases });
            try
            {
                DapperHelper.ExecuteQuery("dbo.genUpdPackageAmenity", parameters, context);
            }
            catch (Exception ex) { }
        }
    }
}
