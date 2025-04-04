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
    public class TierLevelRepository : GenericRepository<TierLevel>, ITierLevel
    {
        protected new WRRContext context;
        public TierLevelRepository(WRRContext context) : base(context)
        {
            this.context = context;
        }

        public int Add(int hotelID , DateTime date, char tier)
        {
            int id = 0;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "HotelID", ParameterValue = hotelID });
            parameters.Add(new ParameterInfo() { ParameterName="Date", ParameterValue=date });
            parameters.Add(new ParameterInfo() { ParameterName = "Level", ParameterValue = tier });
            try
            {
                id = DapperHelper.ExecuteQuery("dbo.genInsTierLevel", parameters, context);
            }
            catch (Exception ex) { }
            return id;
        }
        public void AddDateRange(int hotelID, DateTime start, DateTime end, char tier)
        {
            if (start > DateTime.MinValue && end > DateTime.MinValue && hotelID > 0)
            {
                DateTime temp = start;
                while (temp <= end)
                {
                    int id = Add(hotelID, temp, tier);
                    temp = temp.AddDays(1);
                }
            }
        }

        public char GetTierForDate(int hotelId, DateTime date)
        {
            string tier = string.Empty;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="HotelID", ParameterValue=hotelId });    
            parameters.Add(new ParameterInfo() { ParameterName = "Date", ParameterValue = date } );
            try
            {
                tier = DapperHelper.ExecuteQueryString("dbo.genSelTierLevelByDate", parameters, context, "@Tier");
            }
            catch(Exception ex) { }
            if(string.IsNullOrEmpty(tier))
            {
                return 'a';
            }            
            return Convert.ToChar(tier.ToLower());
        
        }

        public List<TierLevel> GetTierLevelsForHotel(int hotelId)
        {
            List<TierLevel> levels = new List<TierLevel>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="HotelID", ParameterValue = hotelId});
            try
            {
                levels = DapperHelper.GetRecords<TierLevel>("dbo.genSelTierLevelByHotelID", parameters, context);
               
            }
            catch (Exception ex) { }
            return levels;
        }

        public void Update(char tier, int tierLevelId)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="Tier", ParameterValue=tier});
            parameters.Add(new ParameterInfo() { ParameterName="TierLevelID", ParameterValue=tierLevelId});
            try
            {
                DapperHelper.ExecuteQuery("dbo.genUpdTierLevel", parameters, context);
            }
            catch (Exception ex) { }    
            
        }
    }
}
