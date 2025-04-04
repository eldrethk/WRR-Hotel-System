using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using WRR8_0.Models;
using WRR8_0.Extension;
using WRRManagement.Domain.Hotels;
using WRRManagement.Infrastructure.SystemRepository;
using WRRManagement.Domain.System;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace WRR8_0.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHotelUser hotelUserRep;
        private readonly UserManager<IdentityApplicationUser> _userManager;
        private readonly IHotel hotelRep;

        public HomeController(ILogger<HomeController> logger, IHotelUser hotelUser, IHotel hotelRep)
        {
            _logger = logger;
            this.hotelUserRep = hotelUser;
            this.hotelRep = hotelRep;
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Dashboard() 
        {
            int hotelId = 0;
            hotelId = HttpContext.Session.GetInt("HotelID");
            if(hotelId > 0)
            {
                Hotel hotel = hotelRep.GetHotel(hotelId);
                HttpContext.Session.SetString("HotelName", hotel.Name);
            }
            return View(); 
        }

        public IActionResult MenuList()
        {
            try
            {
                var UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                List<Hotel> hotels = hotelUserRep.GetHotelsForUser(UserID);
                var model = new UserViewModel()
                {
                    Hotels = hotels,
                    UserID = UserID
                };
                return View(model);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult SetHotel(UserViewModel model)
        {
            int HotelID = 0;
            HttpContext.Session.Remove("HotelID");


            if (model.SelectedHotel != null)
            {
                HotelID = Convert.ToInt32(model.SelectedHotel);            
                HttpContext.Session.SetInt("HotelID", HotelID);
                //need to add Acoounts controller online users
                return RedirectToAction("Dashboard");
            }
            else
               return NotFound();
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
