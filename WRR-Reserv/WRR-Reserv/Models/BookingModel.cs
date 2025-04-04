using WRRManagement.Domain.Base;
using WRRManagement.Domain.Reservation;

namespace WRR_Reserv.Models
{
    public class BookingModel
    {
        public SearchModel SearchModel { get; set; }
        public AvailableRackRoom availableRackRooms { get; set; }

        public bool OptIn { get; set; } = true;
 

    }

    
}
