using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WRR8_0.Extension;
using WRRManagement.Domain.Hotels;

namespace WRR8_0.Controllers
{
    public class HotelController : Controller
    {
        private readonly IHotel hotelRepository;
        private readonly IHotelSystem hotelSystemRepository;
        private readonly IDisclaimer disclaimerRepository;

        public HotelController(IHotel hotelRep, IHotelSystem hotelSystemRepository, IDisclaimer disclaimerRepository)
        {
            this.hotelRepository = hotelRep;
            this.hotelSystemRepository = hotelSystemRepository;
            this.disclaimerRepository = disclaimerRepository;
        }
        public IActionResult Index()
        {
            int hotelID = HttpContext.Session.GetInt("HotelID");
            Hotel hotel = hotelRepository.GetHotel(hotelID);
            return View(hotel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Hotel hotel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    hotelRepository.Update(hotel);
                    return RedirectToAction("Dashboard", "Home");
                }

            }
            catch
            {

            }
            return View(hotel);
        }

        public IActionResult SystemOptions()
        {
            var roomlist = new SelectList(new[] {
                new {Value = "Avg Per Day", Text = "Avg Per Day" },
                new {Value = "Subtotal", Text="Subtotal"},
                new {Value = "Total", Text = "Total"},
            }, "Value", "Text");

            var packagelist = new SelectList(new[] {
                new {Value = "Avg Per Day", Text = "Avg Per Day" },
                new {Value = "Subtotal", Text="Subtotal"},
                new {Value = "Total", Text = "Total"},
            }, "Value", "Text");

            var roombreakdownlist = new SelectList(new[]{
                new {Value = "Daily Rates", Text= "Daily Rates"},
                new {Value = "Basic", Text="Basic"}
            }, "Value", "Text");

            var packagebreakdownlist = new SelectList(new[]{
                new {Value = "Daily Rates", Text= "Daily Rates"},
                new {Value = "Basic", Text="Basic"}
            }, "Value", "Text");

            var depositlist = new SelectList(new[]
            {
                new {Value = "First Night Room Stay", Text="First Night Room Stay"},
                new {Value="First 2 Nights Room Stay", Text="First 2 Nights Room Stay" },
                new {Value="Percentage of Total", Text="Percentage of Total" },
                new {Value="Total Reservation", Text="Total Reservation"}
            }, "Value", "Text");

            var calbylist = new SelectList(new[]
            {
                new {Value="Flat Fee", Text="Flat Fee"},
                new {Value="Flat Fee Per Day", Text="Flat Fee Per Day"},
                new {Value="Flat Fee Per Person", Text="Flat Fee Per Person"}
            }, "Value", "Text");

            ViewBag.roomlist = roomlist;
            ViewBag.packagelist = packagelist;
            ViewBag.depositlist = depositlist;
            ViewBag.calbylist = calbylist;
            ViewBag.roombreakdownlist = roombreakdownlist;
            ViewBag.packagebreakdownlist = packagebreakdownlist;

            int hotelid = HttpContext.Session.GetInt("HotelID");
            HotelSystem hotel = hotelSystemRepository.GetSystem(hotelid);
            return View(hotel);
        }

        // POST: HotelController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SystemOptions(HotelSystem system)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    hotelSystemRepository.Update(system);
                    return RedirectToAction("Dashboard", "Home");
                }
                else { return View(system); }
            }
            catch
            {
                return View(system);
            }
        }

        public IActionResult Disclaimer()
        {

            int hotelid = HttpContext.Session.GetInt("HotelID");
            Disclaimer disclaimer = disclaimerRepository.GetDisclaimer(hotelid);
            return View(disclaimer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Disclaimer(Disclaimer disclaimer)
        {
            int hotelid = HttpContext.Session.GetInt("HotelID");
            try
            {
                if (ModelState.IsValid)
                {
                    disclaimerRepository.Update(disclaimer);
                    return RedirectToAction("Dashboard", "Home");
                }
                else { return View(disclaimer); }
            }
            catch { return View(disclaimer); }
        }

    }
}
