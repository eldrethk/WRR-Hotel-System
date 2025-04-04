using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WRRManagement.Domain.Base;
using WRRManagement.Domain.System;
using WRRManagement.Infrastructure.Data;

namespace WRRManagement.Infrastructure.SystemRepository
{
    public class MarketingRepository : GenericRepository<OptInEmails>, IOptInEmails
    {
        protected new WRRContext context;
        public MarketingRepository(WRRContext context) : base(context)
        {
            this.context = context;
        }

        public int Add(string email, int hotelID, string firstname, string lastname, string state)
        {
            int id = 0;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="Email", ParameterValue=email});
            parameters.Add(new ParameterInfo() { ParameterName = "HotelID", ParameterValue = hotelID });
            parameters.Add(new ParameterInfo() { ParameterName = "FirstName", ParameterValue = firstname });
            parameters.Add(new ParameterInfo() { ParameterName = "LastName", ParameterValue = lastname });
            parameters.Add(new ParameterInfo() { ParameterName = "State", ParameterValue = state });
            try
            {
                id = DapperHelper.ExecuteQuery("dbo.genInsOptInEmail", parameters, context);
            }
            catch (Exception ex) { }
            return id;
        }

        public List<OptInEmails> GetAll(int hotelId, DateTime start, DateTime end)
        {
            List<OptInEmails> list = new List<OptInEmails>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="HotelID", ParameterValue=hotelId });
            parameters.Add(new ParameterInfo() { ParameterName = "StartDate", ParameterValue = start } );
            parameters.Add(new ParameterInfo() { ParameterName="EndDate", ParameterValue=end });
            try
            {
                list = DapperHelper.GetRecords<OptInEmails>("dbo.genSelEmailList", parameters, context);
            }
            catch(Exception ex) { }
            return list;
        }
    }
}
