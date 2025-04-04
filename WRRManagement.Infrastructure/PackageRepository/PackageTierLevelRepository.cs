using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WRRManagement.Domain.Base;
using WRRManagement.Domain.Packages;
using WRRManagement.Infrastructure.Data;

namespace WRRManagement.Infrastructure.PackageRepository
{
    public class PackageTierLevelRepository : GenericRepository<PackageTierLevel>, IPackageTierLevel
    {
        protected new WRRContext context;
        public PackageTierLevelRepository(WRRContext context) : base(context)
        {
            this.context = context;
        }

        public int Add(int packageId, DateTime date, char tier)
        {
            int id = 0;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "PackageID", ParameterValue = packageId });
            parameters.Add(new ParameterInfo() { ParameterName="Date", ParameterValue= date });
            parameters.Add(new ParameterInfo() { ParameterName = "Tier", ParameterValue = tier });
            try
            {
                id = DapperHelper.ExecuteQuery("dbo.genInsPackageTierLevel", parameters, context);
            }
            catch (Exception ex) { }
            return id;
        }

        public void AddDateRange(int packageId, DateTime start, DateTime end, char tier) {
            if (start > DateTime.MinValue && end>DateTime.MinValue && packageId > 0) 
            {
                DateTime temp = start;
                while(temp <= end)
                {
                    int id = Add(packageId, temp, tier);
                    temp = temp.AddDays(1);
                }
            }
        }

        public List<Package> GetPackagesWithTier(int hotelId)
        {
            List<Package> list = new List<Package>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "HotelID", ParameterValue = hotelId });
            try
            {
                list = DapperHelper.GetRecords<Package>("dbo.genSelPackagesWithTierLevel", parameters, context);
            }
            catch(Exception ex) { }
            return list;
        }

        public List<PackageTierLevel> GetPackageTierLevels(int packageId)
        {
            List<PackageTierLevel> list = new List<PackageTierLevel>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="PackageID", ParameterValue=packageId });
            try
            {
                list = DapperHelper.GetRecords<PackageTierLevel>("dbo.genSelPackageTierLevelByPackageID", parameters, context);
            }
            catch(Exception e) { }
            return list;
        }

        public char GetTierForDate(int packageId, DateTime date)
        {
            string tier = null;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="PackageID", ParameterValue=packageId}); 
            parameters.Add(new ParameterInfo() { ParameterName="Date", ParameterValue=date });
            try
            {
               tier = DapperHelper.ExecuteQueryString("dbo.genSelPackageTierLevelByDate", parameters, context, "@Tier");
            }
            catch(Exception ex) { }
            return Convert.ToChar(tier.ToLower());
        }

        public PackageTierLevel GetTierLevel(int tierLevelId)
        {
            PackageTierLevel level = null;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="TierLevelID", ParameterValue=tierLevelId });
            try
            {
                level = DapperHelper.GetRecord<PackageTierLevel>("dbo.genSelPackageTierLevelByID", parameters, context);
            }
            catch (Exception e) { }
            return level;
        }

        public void Update(char tier, int tierLevelId)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "PackageTierLevelID", ParameterValue = tierLevelId });
            parameters.Add(new ParameterInfo() { ParameterName = "Tier", ParameterValue = tier });
            try
            {
                DapperHelper.ExecuteQuery("dbo.genUpdPackageTierLevel", parameters, context);
            }
            catch(Exception ex) { }

        }
    }
}
