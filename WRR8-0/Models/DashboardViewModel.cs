using WRRManagement.Domain.Hotels;

namespace WRR8_0.Models
{
    public class DashboardViewModel
    {
        public Hotel Hotel { get; set; }

        /*public IPagedList<ReservationQue> ReservationQue { get; set; }

        public IPagedList<RoomComplaints> ComplaintQue { get; set; }

        public Reservation_Booked Reservation_Booked { get; set; }

        public Specials_Booked Specials_Booked { get; set; }

        public MiniVac_Booked MiniVac_Booked { get; set; }

        public Amenitity_Booked Amenitity_Booked { get; set; }*/

        public int TodaysArrival { get; set; }
        public int TodaysDeparture { get; set; }
        public int RoomOccuied { get; set; }

    }

}