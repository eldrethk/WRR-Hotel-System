using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WRRManagement.Domain.Reservation;
using WRRManagement.Infrastructure.Data;
using WRRManagement.Domain.Base;

namespace WRRManagement.Infrastructure.ReservationRepository
{
    public class ReportManagerRepository : IReservationQue
    {
        protected new WRRContext context;

        public ReportManagerRepository(WRRContext context)
        {
            this.context = context;
        }
        public int AddReservationQue(ReservationQue reservationQue)
        {
            int ID = 0;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "ReservationID", ParameterValue = reservationQue.ReservationID });
            parameters.Add(new ParameterInfo() { ParameterName = "BookedDate", ParameterValue = reservationQue.BookedDate });
            parameters.Add(new ParameterInfo() { ParameterName = "CusName", ParameterValue = reservationQue.CustomerName });
            parameters.Add(new ParameterInfo() { ParameterName = "ReservationType", ParameterValue = reservationQue.ReservationType });
            parameters.Add(new ParameterInfo() { ParameterName = "HotelID", ParameterValue = reservationQue.HotelID });

            try
            {
                ID = DapperHelper.ExecuteQuery("dbo.rptSelReservationQue", parameters, context);
            }
            catch (Exception ex)
            {
            }
            return ID;
        }

        public Amenitity_Booked GetAmenitity30BookReport(int hotelID)
        {
            throw new NotImplementedException();
        }

        public List<BookedAmenity> GetBookedAmenities(int hotelID, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public List<BookedAmenity> GetPackageBookedAmenities(int hotelID, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public Reservation_Booked GetReservation30BookReport(int hotelID)
        {
            throw new NotImplementedException();
        }

        public List<Reservation> GetReservationForReport(int hotelID, DateTime start, DateTime end, string searchBy)
        {
            throw new NotImplementedException();
        }

        public List<ReservationQue> GetReservationQue(int hotelID)
        {
            List<ReservationQue> list = new List<ReservationQue>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "HotelID", ParameterValue = hotelID });
            try
            {
                list = DapperHelper.GetRecords<ReservationQue>("dbo.genSelReservationQue", parameters, context);
            }
            catch (Exception ex) { }
            return list;
        }

        public Specials_Booked GetSpecials30BookReport(int hotelID)
        {
            throw new NotImplementedException();
        }

        public void UpdateReservationQue(int ReservationID, string UserName, int hotelID)
        {
            throw new NotImplementedException();
        }
    }
}
