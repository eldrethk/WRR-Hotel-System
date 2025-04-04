using Microsoft.AspNetCore.Mvc;
using WRR8_0.Extension;
using WRR8_0.Models;

namespace WRR8_0.Controllers
{
    public class PackageRackRateController : Controller
    {
        public IActionResult Index()
        {
            int HotelID = HttpContext.Session.GetInt("HotelID");
            HotelViewModel model = new HotelViewModel
            {
                HotelId = HotelID
            };
            return View(model);
        }
    }
}
