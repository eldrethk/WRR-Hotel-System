using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WRRManagement.Domain.Base;
using WRRManagement.Domain.Reservation;
using WRRManagement.Infrastructure.Data;

namespace WRRManagement.Infrastructure.ReservationRepository
{
    public class ReservationAmenityRepository : IReservationAmenity
    {
        protected new WRRContext context;

        public ReservationAmenityRepository(WRRContext context) 
        {
            this.context = context;
        }
        public int AddBookedAmenity(ReservationAmenity reservationAmenity)
        {
            int id = 0;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "ReservationID", ParameterValue = reservationAmenity.ReservationID });

            try
            {
                id = DapperHelper.ExecuteQuery("dbo.genInsReservationAmenity", parameters, context);
            }
            catch (Exception ex) { }
            return id;
        }

        public int AddPackageBookedAmenity(ReservationAmenity reservationAmenity)
        {
            int id = 0;
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "ReservationID", ParameterValue = reservationAmenity.ReservationID });

            try
            {
                id = DapperHelper.ExecuteQuery("dbo.genInsReservationAmenity", parameters, context);
            }
            catch (Exception ex) { }
            return id;  
        }

        public List<ReservationAmenity> GetAmenitiesBookedForPackage(int reservationId)
        {
            List<ReservationAmenity> list = new List<ReservationAmenity>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();

            parameters.Add(new ParameterInfo() { ParameterName = "ReservationID", ParameterValue = reservationId });
            try
            {
                list = DapperHelper.GetRecords<ReservationAmenity>("dbo.genSelReservationAmenity", parameters, context);
            }
            catch (Exception ex) { }
            return list;
        }

        public List<ReservationAmenity> GetAmenitiesBookedForRackRate(int reservationId)
        {
            List<ReservationAmenity> list = new List<ReservationAmenity>();
            List<ParameterInfo> parameters = new List<ParameterInfo>();
            parameters.Add(new ParameterInfo() { ParameterName = "ReservationID", ParameterValue = reservationId });
            try
            {
                list = DapperHelper.GetRecords<ReservationAmenity>("dbo.rptAmenityBookedByReservID", parameters, context);
            }
            catch (Exception ex)
            {
            }
            return list;
    }
}
}
