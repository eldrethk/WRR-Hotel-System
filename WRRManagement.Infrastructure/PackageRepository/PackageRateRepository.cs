using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WRRManagement.Domain.Base;
using WRRManagement.Domain.Packages;
using WRRManagement.Infrastructure.Data;

namespace WRRManagement.Infrastructure.PackageRepository
{
    public class PackageRateRepository : GenericRepository<PackageRate>, IPackageRate
    {
        protected new WRRContext context;
        public PackageRateRepository(WRRContext context) : base(context)
        {
            this.context = context;
        }

        public int Add(PackageRate packageRate)
        {
            int id = 0;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "Start", ParameterValue = packageRate.StartDate } );
            parameters.Add(new ParameterInfo() { ParameterName="End", ParameterValue=packageRate.EndDate } );
            parameters.Add(new ParameterInfo() { ParameterName="RoomID", ParameterValue=packageRate.RoomTypeID } );
            parameters.Add(new ParameterInfo() { ParameterName="Price", ParameterValue= packageRate.Price } );  
            parameters.Add(new ParameterInfo() { ParameterName="PackageID", ParameterValue=packageRate.PackageID } );
            try
            {
                id = DapperHelper.ExecuteQuery("dbo.genInsPackageRate", parameters, context);
            }
            catch(Exception ex) { }
            return id;

            
        }

        public bool CheckDates(int roomId, DateTime start, DateTime end, int packageId)
        {
            bool valid = false;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "StartDate", ParameterValue = start });
            parameters.Add(new ParameterInfo() { ParameterName = "EndDate", ParameterValue = end });
            parameters.Add(new ParameterInfo() { ParameterName = "RoomID", ParameterValue = roomId }); 
            parameters.Add(new ParameterInfo() { ParameterName="PackageID", ParameterValue = packageId });
            try
            {
                valid = DapperHelper.ExecuteQueryBool("dbo.genVerifyPackageRates", parameters, context, "@Valid");
            }
            catch (Exception ex) { }
            return valid;
        }

        public PackageRate Get(int rateId)
        {
            PackageRate packageRate = null;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "PKRateID", ParameterValue = rateId });
            try
            {
                packageRate = DapperHelper.GetRecord<PackageRate>("dbo.genSelPackageRateByID", parameters, context);
            }
            catch(Exception ex) { }
            return packageRate;
        }

        public List<PackageRate> GetRackRates(int roomId, int packageId)
        {
            List<PackageRate> list = new List<PackageRate>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="RoomID", ParameterValue=roomId });
            parameters.Add(new ParameterInfo() { ParameterName="PackageID", ParameterValue=packageId });
            try
            {
                list = DapperHelper.GetRecords<PackageRate>("dbo.genSelPackageRates", parameters, context);
            }
            catch(Exception ex) { }
            return list;
        }

        public PackageRate GetRateForReservation(int roomId, DateTime temp, int packageId)
        {
            PackageRate packageRate = null;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "RoomID", ParameterValue = roomId });
            parameters.Add(new ParameterInfo() { ParameterName = "PackageID", ParameterValue = packageId });
            parameters.Add(new ParameterInfo() { ParameterName="Temp", ParameterValue=temp});
            try
            {
                packageRate = DapperHelper.GetRecord<PackageRate>("dbo.genSelPackageRateByDate", parameters, context);
            }
            catch(Exception ex) { }
            return packageRate;
        }

        public void Invisible(int rateId)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="RateID", ParameterValue=rateId });
            try
            {
                DapperHelper.ExecuteQuery("dbo.genInvisiblePackageRate", parameters, context);
            }
            catch(Exception ex) { }
        }

        public void Update(PackageRate packageRate)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "Start", ParameterValue = packageRate.StartDate });
            parameters.Add(new ParameterInfo() { ParameterName = "End", ParameterValue = packageRate.EndDate });     
            parameters.Add(new ParameterInfo() { ParameterName = "Price", ParameterValue = packageRate.Price });
            parameters.Add(new ParameterInfo() { ParameterName = "RateID", ParameterValue = packageRate.RateID });
            try
            {
                DapperHelper.ExecuteQuery("dbo.genUpdPackageRate", parameters, context);
            }
            catch (Exception ex) { }

        }

        public void Visible(int rateId)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "RateID", ParameterValue = rateId });
            try
            {
                DapperHelper.ExecuteQuery("dbo.genVisiblePackageRate", parameters, context);
            }
            catch (Exception ex) { }
        }
    }
}
