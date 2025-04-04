using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WRRManagement.Domain.Reservation;
using WRRManagement.Domain.Base;
using WRRManagement.Infrastructure.Data;
using WRRManagement.Domain.RoomTypes;
using Dapper;
using System.Data;

namespace WRRManagement.Infrastructure.ReservationRepository
{
    public class ReservationRepository : IReservation
    {
        protected new WRRContext context;
        private readonly IRoomAllocation roomAllocationRepository;
        public ReservationRepository(WRRContext context, IRoomAllocation roomAllocation)
        {
            this.context = context;
            this.roomAllocationRepository = roomAllocation;
        }

        public int AddReservation(Reservation reservation)
        {
            if (reservation.ArrivalDate < reservation.DepartureDate)
            {
                int id = 0;
                bool available = false;
                return DapperHelper.ExecuteWithTransaction((conn, transaction) =>
                {
                    try
                    {
                        foreach (DateTime dteTemp in EachDay(reservation.ArrivalDate, reservation.DepartureDate))
                        {
                            List<ParameterInfo> parameters = new List<ParameterInfo>();
                            parameters.Add(new ParameterInfo() { ParameterName = "RoomID", ParameterValue = reservation.RoomTypeID });
                            parameters.Add(new ParameterInfo() { ParameterName = "Date", ParameterValue = dteTemp });

                            //tran.commit() and rollback if not available
                            int result = conn.ExecuteScalar<int>("dbo.genReserveRoom", parameters, transaction, commandType: CommandType.StoredProcedure);

                            if (result <= 0)
                            {
                                throw new Exception($"Failed to reserve room ~ {reservation.RoomTypeID} / {dteTemp}");
                            }

                            available = true;
                            //if (!available) { return -1; }                         
                        }
                        if (available)
                        {
                            List<ParameterInfo> parameters2 = new List<ParameterInfo>()
                            {
                                new ParameterInfo { ParameterName = "HotelID", ParameterValue = reservation.HotelID },
                                new ParameterInfo { ParameterName = "RoomTypeID", ParameterValue = reservation.RoomTypeID },
                                new ParameterInfo { ParameterName = "ArrivalDate", ParameterValue = reservation.ArrivalDate },
                                new ParameterInfo { ParameterName = "DepartureDate", ParameterValue = reservation.DepartureDate },
                                new ParameterInfo { ParameterName = "TotalNights", ParameterValue = reservation.TotalNights },
                                new ParameterInfo { ParameterName = "Adults", ParameterValue = reservation.Adults },
                                new ParameterInfo { ParameterName = "Children", ParameterValue = reservation.Children },
                                new ParameterInfo { ParameterName = "AvgDailyRate", ParameterValue = reservation.AvgDailyRate },
                                new ParameterInfo { ParameterName = "SubTotal", ParameterValue = reservation.SubTotal },
                                new ParameterInfo { ParameterName = "ExtraAdultCharge", ParameterValue = reservation.ExtraAdultCharge },
                                new ParameterInfo { ParameterName = "ExtraChildCharge", ParameterValue = reservation.ExtarChildCharge },
                                new ParameterInfo { ParameterName = "WeekendFees", ParameterValue = reservation.WeekendFees },
                                new ParameterInfo { ParameterName = "ResortFees", ParameterValue = reservation.ResortFees },
                                new ParameterInfo { ParameterName = "TotalFees", ParameterValue = reservation.TotalFees },
                                new ParameterInfo { ParameterName = "Taxes", ParameterValue = reservation.Taxes },
                                new ParameterInfo { ParameterName = "TotalCharge", ParameterValue = reservation.TotalCharge },
                                new ParameterInfo { ParameterName = "Deposit", ParameterValue = reservation.Deposit },
                                new ParameterInfo { ParameterName = "Comments", ParameterValue = reservation.Comments ?? string.Empty },
                                new ParameterInfo { ParameterName = "CardHolderName", ParameterValue = reservation.CardHolderName },
                                new ParameterInfo { ParameterName = "CardExpireDate", ParameterValue = reservation.CardExpirationDate },
                                new ParameterInfo { ParameterName = "CardNumber", ParameterValue = reservation.CardNumber },
                                new ParameterInfo { ParameterName = "SecureCode", ParameterValue = reservation.CardSecureCode },
                                new ParameterInfo { ParameterName = "FirstName", ParameterValue = reservation.CusFirstName },
                                new ParameterInfo { ParameterName = "LastName", ParameterValue = reservation.CusLastName },
                                new ParameterInfo { ParameterName = "Address1", ParameterValue = reservation.CusAddress1 },
                                new ParameterInfo { ParameterName = "Address2", ParameterValue = reservation.CusAddress2 ?? string.Empty },
                                new ParameterInfo { ParameterName = "City", ParameterValue = reservation.CusCity },
                                new ParameterInfo { ParameterName = "State", ParameterValue = reservation.CusState },
                                new ParameterInfo { ParameterName = "ZipCode", ParameterValue = reservation.CusZip },
                                new ParameterInfo { ParameterName = "PhoneNumber", ParameterValue = reservation.CusDayPhone },
                                new ParameterInfo { ParameterName = "Email", ParameterValue = reservation.CusEmail },
                                new ParameterInfo { ParameterName = "UserInitals", ParameterValue = reservation.UserInitals }
                            };

                            id = DapperHelper.ExecuteQuery("dbo.genInsReservation", parameters2, context);
                        }
                    }
                    catch (Exception ex)
                    {
                        //add room allocation back

                    }
                    return available ? id : -1;

                }, context);
            }
            return -1;
        }

        private static IEnumerable<DateTime> EachDay(DateTime start, DateTime end)
        {
            for (var day = start; day < end; day = day.AddDays(1))
            {
                yield return day;
            }
        }

        public void AddReservationDailyRate(AvailableRackRoom rackRoom, int ReservationID)
        {
            try
            {
                for (int i = 0; i < rackRoom.ratesDate.Count; i++)
                {
                    List<ParameterInfo> parameters = new List<ParameterInfo>();
                    parameters.Add(new ParameterInfo() { ParameterName = "ReservationID", ParameterValue = ReservationID });

                    parameters.Add(new ParameterInfo() { ParameterName = "ReservationDate", ParameterValue = rackRoom.ratesDate[i] });
                    parameters.Add(new ParameterInfo() { ParameterName = "DailyRate", ParameterValue = rackRoom.rates[i] });

                    DapperHelper.ExecuteQuery("dbo.genInsDailyRate", parameters, context);
                }
            }
            catch (Exception ex) { }

        }
    }
}
