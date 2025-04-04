using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using WRR8_0.Extension;
using WRR8_0.Models;
using WRRManagement.Domain.Amenities;
using WRRManagement.Domain.Packages;
using WRRManagement.Infrastructure.AmenityRepository;


namespace WRR8_0.Controllers
{
    public class ExtraAmenityController : Controller
    {
        private readonly IExtraAmenity extraAmenityRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IPackageAmenity packageAmenityRepository;

        public ExtraAmenityController(IExtraAmenity extraAmenityRepository, IWebHostEnvironment hostingEnvironment, IPackageAmenity packageAmenityRep)
        {
            this.extraAmenityRepository = extraAmenityRepository;
            _hostingEnvironment = hostingEnvironment;
            packageAmenityRepository = packageAmenityRep;
        }
        // GET: ExtraAmenityController
        public ActionResult Index()
        {
            int hotelid = HttpContext.Session.GetInt("HotelID");
            List<ExtraAmenity> amenities = extraAmenityRepository.GetAllForHotel(hotelid);
            return View(amenities);
        }

        // GET: ExtraAmenityController/Details/5
        public ActionResult Details(int id)
        {
            string type = string.Empty;
           
            AmenityViewModel model = new AmenityViewModel
            {
                Amenity = extraAmenityRepository.GetAmenity(id),
                Packages = packageAmenityRepository.GetPackagesAssociatedWithAmenity(id),
             
            };
            if (model.Amenity.PerDayPerPerson)
                type = "Per Day Per Person";
            else if (model.Amenity.PerDay)
                type = "Per Day";
            else if (model.Amenity.PerNightStay)
                type = "Per Night Stay";
            else if (model.Amenity.OneTimeFee)
                type = "One Time Fee";
            else if (model.Amenity.OneTimeFeePerson)
                type = "One Time Fee Person";
            else if (model.Amenity.Discount)
                type = "Discount";

            model.AmenityType = type;
            return View(model);
        }

        // GET: ExtraAmenityController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ExtraAmenityController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AmenityViewModel model)
        {
            try
            {


                if (ModelState.IsValid)
                {
                    int HotelId = HttpContext.Session.GetInt("HotelID");

                    model.Amenity.HotelID = HotelId;
                    model.Amenity.Description = HttpUtility.HtmlDecode(model.Amenity.Description);
                    model.Amenity.Mandatory = false;
                    model.Amenity.ViewOnRackRate = model.ViewOnRackRate;
                    model.Amenity.ViewRate = false;

                    if (model.AmenityType == "PerDayPerPerson")
                        model.Amenity.PerDayPerPerson = true;
                    else if (model.AmenityType == "PerDay")
                        model.Amenity.PerDay = true;
                    else if (model.AmenityType == "PerNightStay")
                        model.Amenity.PerNightStay = true;
                    else if (model.AmenityType == "OneTimeFee")
                        model.Amenity.OneTimeFee = true;
                    else if (model.AmenityType == "OneTimeFeePerson")
                        model.Amenity.OneTimeFeePerson = true;
                    else if (model.AmenityType == "Discount")
                        model.Amenity.Discount = true;


                    if (model.Amenity.Discount == false)
                        model.Amenity.DiscountRegularRate = 0;

                    //upload image and save to amenity
                    if (model.UploadImage != null)
                    {
                        var supportedTypes = new[] { "image/jpg", "image/jpeg", "image/png" };
                        if (!supportedTypes.Contains(model.UploadImage.Image.ContentType))
                        {
                            ModelState.AddModelError("Image", "Invalid type. Only JPG and PNG are allowed.");
                            return View(model);
                        }
                        var uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img/amenity-images");
                        var fileName = Guid.NewGuid().ToString() + "_" + HotelId.ToString() + "_" + model.UploadImage.Image.FileName;
                        var path = Path.Combine(uploadFolder, fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            model.UploadImage.Image.CopyTo(fileStream);
                        }
                        model.Amenity.PictureUrl = fileName;
                    }
                    else
                    {
                        model.Amenity.PictureUrl = string.Empty;
                    }

                    extraAmenityRepository.Add(model.Amenity);
                    return RedirectToAction(nameof(Index));
                }

                return View(model);
            }
            catch
            {
                return View(model);
            }
        }

        // GET: ExtraAmenityController/Edit/5
        public ActionResult Edit(int id)
        {
            ExtraAmenity extraAmenity = extraAmenityRepository.GetAmenity(id);     
        
            string type = string.Empty;
            if (extraAmenity.PerDayPerPerson)
                type = "PerDayPerPerson";
            else if (extraAmenity.PerDay)
                type = "PerDay";
            else if (extraAmenity.PerNightStay)
                type = "PerNightStay";
            else if (extraAmenity.OneTimeFee)
                type = "OneTimeFee";
            else if (extraAmenity.OneTimeFeePerson)
                type = "OneTimeFeePerson";
            else if (extraAmenity.Discount)
                type = "Discount";

            AmenityViewModel model = new AmenityViewModel
            {
                Amenity = extraAmenity,
                //Description = HttpUtility.HtmlDecode(extraAmenity.Description),
                ViewRate = false,
                ViewOnRackRate = extraAmenity.ViewOnRackRate,
                AmenityType = type,
                Packages = packageAmenityRepository.GetPackagesAssociatedWithAmenity(id)
            };
            return View(model);
        }

        // POST: ExtraAmenityController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AmenityViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Amenity.Description = HttpUtility.HtmlDecode(model.Amenity.Description);
                    model.Amenity.ViewOnRackRate = model.ViewOnRackRate;
                    if (model.AmenityType == "PerDayPerPerson")
                        model.Amenity.PerDayPerPerson = true;
                    else if (model.AmenityType == "PerDay")
                        model.Amenity.PerDay = true;
                    else if (model.AmenityType == "PerNightStay")
                        model.Amenity.PerNightStay = true;
                    else if (model.AmenityType == "OneTimeFee")
                        model.Amenity.OneTimeFee = true;
                    else if (model.AmenityType == "OneTimeFeePerson")
                        model.Amenity.OneTimeFeePerson = true;
                    else if (model.AmenityType == "Discount")
                        model.Amenity.Discount = true;

                    if (model.Amenity.Discount == false)
                        model.Amenity.DiscountRegularRate = 0;

                    //upload image and save to amenity
                    if (model.UploadImage != null)
                    {
                        var supportedTypes = new[] { "image/jpg", "image/jpeg", "image/png" };
                        if (!supportedTypes.Contains(model.UploadImage.Image.ContentType))
                        {
                            ModelState.AddModelError("Image", "Invalid type. Only JPG and PNG are allowed.");
                            return View(model);
                        }
                        var uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img/amenity-images");
                        var fileName = Guid.NewGuid().ToString() + "_" + model.Amenity.HotelID.ToString() + "_" + model.UploadImage.Image.FileName;
                        var path = Path.Combine(uploadFolder, fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            model.UploadImage.Image.CopyTo(fileStream);
                        }
                        model.Amenity.PictureUrl = fileName;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(model.Amenity.PictureUrl))
                            model.Amenity.PictureUrl = string.Empty;
                    }

                    extraAmenityRepository.Update(model.Amenity);
                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            }
            catch
            {
                return View(model);
            }
        }

        // GET: ExtraAmenityController/Delete/5
        public ActionResult Delete(int id)
        {
            extraAmenityRepository.remove(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Report()
        {
            SearchReportViewModel model = new SearchReportViewModel()
            {
                HotelID = HttpContext.Session.GetInt("HotelID")
            };

            return View(model);
        }

        public IActionResult DetailReport(SearchReportViewModel model)
        {
            if (ModelState.IsValid) 
            { 
                int hotelid = HttpContext.Session.GetInt("HotelID");
                DateTime start = model.StartDate ?? DateTime.MinValue;
                DateTime end = model.EndDate ?? DateTime.MaxValue;
                if(hotelid > 0 && start > DateTime.MinValue && end > DateTime.MaxValue)
                {
                    //List<BookAmenity> 
                }
            }

        return View(model);
        }
    }
}
