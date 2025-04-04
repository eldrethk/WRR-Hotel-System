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
    public class DisclaimerRepository : GenericRepository<Disclaimer>, IDisclaimer
    {
        protected new WRRContext context;
        public DisclaimerRepository(WRRContext context) : base(context)
        {
            this.context = context;
        }

        public Disclaimer GetDisclaimer(int hotelId)
        {
            Disclaimer disclaimer = new Disclaimer();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="HotelID", ParameterValue=hotelId});
            try
            {
                disclaimer = DapperHelper.GetRecord<Disclaimer>("dbo.genSelDisclaimer", parameters, context);
            }
            catch (Exception ex) { }
            return disclaimer;
        }

        public void Update(Disclaimer disclaimer)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="Disclaimer", ParameterValue =disclaimer.DisclaimerText});
            parameters.Add(new ParameterInfo() { ParameterName="Email", ParameterValue=disclaimer.EmailDisclaimerText });
            parameters.Add(new ParameterInfo() { ParameterName="HotelID", ParameterValue=disclaimer.HotelID});

            try
            {
                DapperHelper.ExecuteQuery("dbo.genUpdDisclaimer", parameters, context);
            }
            catch(Exception ex) { }
        }
    }
}
