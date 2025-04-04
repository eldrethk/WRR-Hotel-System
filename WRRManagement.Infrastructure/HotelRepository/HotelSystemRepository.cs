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
    public class HotelSystemRepository : GenericRepository<HotelSystem>, IHotelSystem
    {
        protected new WRRContext context;
        public HotelSystemRepository(WRRContext context) : base(context)
        {
            this.context = context;
        }

        public int GetPriorDayBooking(int hotelId)
        {
            int days = 0;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="HotelID", ParameterValue=hotelId});
            try
            {
                days = DapperHelper.ExecuteQueryInt("dbo.genSelPriorDays", parameters, context, "@Days");
            }
            catch (Exception ex) { }
            return days;
        }

        public HotelSystem GetSystem(int hotelId)
        {
            HotelSystem hSystem = null;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "HotelID", ParameterValue = hotelId });
            try
            {
                hSystem = DapperHelper.GetRecord<HotelSystem>("dbo.genSelHotelSystem", parameters, context);
            }
            catch (Exception ex) { }
            return hSystem;
        }

        public void Update(HotelSystem sys)
        {
           List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName="HotelID", ParameterValue=sys.HotelID});
            parameters.Add(new ParameterInfo() { ParameterName="AdultFee", ParameterValue=sys.ExtraAdultFee});
            parameters.Add(new ParameterInfo() { ParameterName="ChildFee", ParameterValue=sys.ExtraChildFee}); 
            parameters.Add(new ParameterInfo() { ParameterName="BaseFee", ParameterValue=sys.ExtraBaseFee});
            parameters.Add(new ParameterInfo() { ParameterName="WeekendFee", ParameterValue=sys.WeekendFee});
            parameters.Add(new ParameterInfo() { ParameterName="ResortFee", ParameterValue=sys.ResortFee});
            parameters.Add(new ParameterInfo() { ParameterName="TaxRate", ParameterValue=sys.TaxRate});
            parameters.Add(new ParameterInfo() { ParameterName="AddTaxDeposit", ParameterValue=sys.AddTaxToDeposit});
            parameters.Add(new ParameterInfo() { ParameterName = "DisplayRoomRate", ParameterValue = sys.DisplayRoomRatesAs });
            parameters.Add(new ParameterInfo() { ParameterName="DisplayPackageRate", ParameterValue=sys.DisplayPackageRatesAs });
            parameters.Add(new ParameterInfo() { ParameterName="LowAllocationLimit", ParameterValue=sys.LowAllocationLimit});
            parameters.Add(new ParameterInfo() { ParameterName="PriorBook", ParameterValue=sys.PriorBook});
            parameters.Add(new ParameterInfo() { ParameterName="DepositRoomCal", ParameterValue=sys.DepositRoomCalAs});
            parameters.Add(new ParameterInfo() { ParameterName="DepositPackageCal", ParameterValue=sys.DepositPackageCalAs});
            parameters.Add(new ParameterInfo() { ParameterName="DepositRoomPercentage", ParameterValue=sys.DepositRoomPercentage});
            parameters.Add(new ParameterInfo() { ParameterName="DepositPackagePercentage", ParameterValue=sys.DepositPackagePercentage});
            parameters.Add(new ParameterInfo() { ParameterName="AddTaxWeekend", ParameterValue=sys.AddTaxToWeekendFee});
            parameters.Add(new ParameterInfo() { ParameterName = "AddTaxExtraPerson", ParameterValue = sys.AddTaxToExtraPerson });
            parameters.Add(new ParameterInfo() { ParameterName="ResortFeeCal", ParameterValue=sys.ResortFeeCalAs});
            parameters.Add(new ParameterInfo() { ParameterName="AddTaxToResortFee", ParameterValue=sys.AddTaxToResortFee});
            parameters.Add(new ParameterInfo() { ParameterName="RoomBreakDown", ParameterValue=sys.DisplayRoomBreakDownAs});
            parameters.Add(new ParameterInfo() { ParameterName="PackageBreakDown", ParameterValue=sys.DisplayPackageBreakDownAs});
            try
            {
                DapperHelper.ExecuteQuery("dbo.genUpdHotelSystem", parameters, context);
            }
            catch(Exception ex) { }

        }
    }
}
