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
    public class HotelLayoutRepository : GenericRepository<HotelLayout>, IHotelLayout
    {
        protected new WRRContext context;
        public HotelLayoutRepository(WRRContext context) : base(context)
        {
            this.context = context;
        }

        public HotelLayout Get(int hotelId)
        {
            HotelLayout hotelLayout = new HotelLayout();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="HotelID", ParameterValue=hotelId});
            try
            {
                hotelLayout = DapperHelper.GetRecord<HotelLayout>("dbo.genSelHotelLayout", parameters, context);
            }
            catch(Exception ex) { }
            return hotelLayout;
        }

        public void UpdateCSS(HotelLayout hotelLayout)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "HotelID", ParameterValue = hotelLayout.HotelID });
            parameters.Add(new ParameterInfo() { ParameterName = "CSS", ParameterValue = hotelLayout.HotelCSS });
            try
            {
                DapperHelper.ExecuteQuery("dbo.genInsHotelCSS", parameters, context);
            }
            catch (Exception ex) { }
        }

        public void UpdateEmailLayout(HotelLayout hotelLayout)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "HotelID", ParameterValue = hotelLayout.HotelID });
            parameters.Add(new ParameterInfo() { ParameterName = "Logo", ParameterValue = hotelLayout.EmailHotelLogo });
            parameters.Add(new ParameterInfo() { ParameterName="EmailHeader", ParameterValue=hotelLayout.EmailHeaderImage});
            try
            {
                DapperHelper.ExecuteQuery("dbo.genInsHotelEmail", parameters, context);
            }
            catch (Exception ex) { }
        }

        public void UpdateFooter(HotelLayout hotelLayout)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "HotelID", ParameterValue = hotelLayout.HotelID });
            parameters.Add(new ParameterInfo() { ParameterName = "Footer", ParameterValue = hotelLayout.FooterFileName });
            try
            {
                DapperHelper.ExecuteQuery("dbo.genInsHotelFooter", parameters, context);
            }
            catch (Exception ex) { }
        }

        public void UpdateHeader(HotelLayout hotelLayout)
        {
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "HotelID", ParameterValue = hotelLayout.HotelID });
            parameters.Add(new ParameterInfo() { ParameterName = "Header", ParameterValue = hotelLayout.HeaderFileName });
            try
            {
                DapperHelper.ExecuteQuery("dbo.", parameters, context);
            }
            catch(Exception ex) { }
        }
    }
}
