using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WRRManagement.Domain.Base;
using WRRManagement.Domain.RoomTypes;
using WRRManagement.Infrastructure.Data;

namespace WRRManagement.Infrastructure.RoomTypeRespository
{
    public class RackRateRespository : GenericRepository<RackRate>, IRackRate
    {
        protected new WRRContext context;
        public RackRateRespository(WRRContext context) : base(context)
        {
            this.context = context;
        }

        public int Add(RackRate rackRate)
        {
            int id = 0;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="Start", ParameterValue=rackRate.StartDate});
            parameters.Add(new ParameterInfo() { ParameterName="End", ParameterValue = rackRate.EndDate});
            parameters.Add(new ParameterInfo() { ParameterName="RoomID", ParameterValue=rackRate.RoomTypeID});
            parameters.Add(new ParameterInfo() { ParameterName="TierA", ParameterValue=rackRate.TierARate});
            parameters.Add(new ParameterInfo() { ParameterName="TierB", ParameterValue=rackRate.TierBRate});
            parameters.Add(new ParameterInfo() { ParameterName="TierC", ParameterValue=rackRate.TierCRate});
            parameters.Add(new ParameterInfo() { ParameterName="TierD", ParameterValue=rackRate.TierDRate});
            try
            {
                id = DapperHelper.ExecuteQuery("dbo.genInsRackRate", parameters, context);
            }
            catch (Exception ex) { }
            return id;

        }

        public bool CheckDates(int roomId, DateTime startDate, DateTime endDate)
        {
            bool valid = false;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "roomid", ParameterValue = roomId });
            parameters.Add(new ParameterInfo() { ParameterName = "startdate", ParameterValue = startDate });
            parameters.Add(new ParameterInfo() { ParameterName = "enddate", ParameterValue = endDate });
            try
            {
                valid = DapperHelper.ExecuteQueryBool("dbo.genVerifyDateRange", parameters, context, "valid");
                //valid = DapperHelper.GetRecord<bool>("dbo.genVerifyDateRanage", parameters, context);
            }
            catch (Exception ex)
            {
            }
            return valid;
        }

        public RackRate GetById(int id)
        {
            RackRate rate = null;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "RackRackID", ParameterValue=id});

            try
            {
                rate = DapperHelper.GetRecord<RackRate>("dbo.genSelRackRateByID", parameters, context);
            }
            catch (Exception ex) { }

            return rate;
        }

        public List<RackRate> GetByRoomId(int roomId)
        {
            List<RackRate> rates = new List<RackRate>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "RoomID", ParameterValue = roomId });
            try
            {
                rates = DapperHelper.GetRecords<RackRate>("dbo.genSelRackRateByRoomID", parameters, context);
            }
            catch(Exception ex) { } 
            return rates;
        }

        public RackRate GetRatesForReservation(int roomid, DateTime temp)
        {
            RackRate rate = null;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="RoomID", ParameterValue=roomid});
            parameters.Add(new ParameterInfo() { ParameterName="Temp", ParameterValue=temp});

            try
            {
                rate = DapperHelper.GetRecord<RackRate>("dbo.genSelRackRateByDate", parameters, context);
                if (rate == null)
                {
                    rate = new RackRate();
                    // Handle the case where no rate is found
                    // For example, you can log the information or throw a custom exception
                    //Log.Information($"No rate found for RoomID: {roomid} and Date: {temp}");
                    // throw new RateNotFoundException($"No rate found for RoomID: {roomid} and Date: {temp}");
                }
            }
            catch(Exception e) { }
            return rate;
        }

        public void Invisible(int rackRateId)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="RackRateID", ParameterValue= rackRateId });

            try
            {
                DapperHelper.ExecuteQuery("dbo.genInvisibleRackRate", parameters, context);
            }
            catch(Exception ex) { }
            
        }

        public void Update(RackRate rackRate)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "Start", ParameterValue = rackRate.StartDate });
            parameters.Add(new ParameterInfo() { ParameterName = "End", ParameterValue = rackRate.EndDate });
            parameters.Add(new ParameterInfo() { ParameterName = "RateID", ParameterValue = rackRate.RackRateID });
            parameters.Add(new ParameterInfo() { ParameterName = "TierA", ParameterValue = rackRate.TierARate });
            parameters.Add(new ParameterInfo() { ParameterName = "TierB", ParameterValue = rackRate.TierBRate });
            parameters.Add(new ParameterInfo() { ParameterName = "TierC", ParameterValue = rackRate.TierCRate });
            parameters.Add(new ParameterInfo() { ParameterName = "TierD", ParameterValue = rackRate.TierDRate });

            try
            {
                DapperHelper.ExecuteQuery("dbo.genUpdRackRate", parameters, context);
            }
            catch (Exception ex) { }

        }

        public void Visible(int rackRateId)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "RackRateID", ParameterValue = rackRateId });

            try
            {
                DapperHelper.ExecuteQuery("dbo.genVisibleRackRate", parameters, context);
            }
            catch (Exception ex) { }
        }
    }
}
