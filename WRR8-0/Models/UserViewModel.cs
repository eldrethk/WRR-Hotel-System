using Microsoft.AspNetCore.Mvc.Rendering;
using WRRManagement.Domain.Hotels;

namespace WRR8_0.Models
{
    public class UserViewModel
    {
        public List<Hotel> Hotels { get; set; }
        public string UserID { get; set; }

        public string SelectedHotel { get; set; } 
        public IEnumerable<SelectListItem> UserHotels
        {
            get { return Hotels != null ? new SelectList(Hotels, "HotelID", "Name") : new List<SelectListItem>(); }
        }

    }
}
