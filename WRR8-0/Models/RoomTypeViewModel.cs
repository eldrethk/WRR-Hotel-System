using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WRRManagement.Domain.RoomTypes;

namespace WRR8_0.Models
{
    public class RoomTypeViewModel
    {        
        public RoomType RoomType { get; set; }
        public RoomImage? RoomImage { get; set; }
        public AdultBase AdultBaseFee { get; set; }
        public MaxBase MaxBaseFee { get; set; }
        public string BaseFeeType { get; set; } = "Adult";
        
    }
}
