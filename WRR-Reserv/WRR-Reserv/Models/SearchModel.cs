//using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel.DataAnnotations;
using WRRManagement.Domain.Hotels;
using WRRManagement.Domain;

namespace WRR_Reserv.Models
{
    public class SearchModel
    {
        [Required(ErrorMessage = "Arrival Date is required")]
        [Display(Name = "Arrival Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DateGreaterThanToday(ErrorMessage = "Arrival Date must be greater than today")]
        [DataType(DataType.Date)]
        public DateTime ArrivalDate { get; set; } = DateTime.Today;
        [Required(ErrorMessage = "Departure Date is required")]
        [Display(Name = "Departure Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [DateGreaterThanToday(ErrorMessage = "Departure Date must be greater than today")]
        [DateGreaterThan("ArrivalDate", ErrorMessage = "Departure Date must be greater than Arrival Date")]
        [DataType(DataType.Date)]
        public DateTime DepartureDate { get; set; } = DateTime.Today.AddDays(3);
        [Required(ErrorMessage = "Number of Adults is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Adult has to be greater than one")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter a valid Number")]
        public int Adults { get; set; } = 1;
        [Required(ErrorMessage = "Number of Children is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid number")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter a valid Number")]
        public int Children { get; set; } = 0;
        // public List<RoomTypeViewModel> RoomTypes { get; set; }
        //public Hotel Hotel { get; set; }
        public int HotelId { get; set; }
    }
}
