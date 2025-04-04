using Microsoft.AspNetCore.Mvc;
using System.Web;
using WRR8_0.Extension;
using WRR8_0.Models;
using WRRManagement.Domain.Amenities;
using WRRManagement.Domain.Hotels;
using WRRManagement.Domain.Packages;
using WRRManagement.Domain.RoomTypes;

namespace WRR8_0.Controllers
{
    public class PackagesController : Controller
    {
        private readonly IPackage packageRep;
        private readonly IRoomType roomRep;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IExtraAmenity amenityRep;

        public PackagesController(IPackage packageRep, IRoomType roomRep, IWebHostEnvironment hostingEnvironment, IExtraAmenity amenityRep)
        {
            this.packageRep = packageRep;
            this.roomRep = roomRep;
            _hostingEnvironment = hostingEnvironment;
            this.amenityRep = amenityRep;
        }
        public IActionResult Index()
        {
            int hotelid = HttpContext.Session.GetInt("HotelID");

            List<Package> packages = packageRep.GetPackages(hotelid);
            return View(packages);
        }

        public IActionResult Create()
        {
            int hotelid = HttpContext.Session.GetInt("HotelID");
            PackageViewModel model = new PackageViewModel();
            model.RoomTypes = roomRep.GetAllForHotel(hotelid);
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(PackageViewModel model)
        {

            try
            {
                int hotelid = HttpContext.Session.GetInt("HotelID");
                model.RoomTypes = roomRep.GetAllForHotel(hotelid);
                if (ModelState.IsValid && hotelid > 0)
                {

                    model.Package.HotelID = hotelid;
                    model.Package.Description = HttpUtility.HtmlDecode(model.Package.Description);
                    model.Package.SmImage = string.Empty;
                    model.Package.Deposit = 0;

                    if (model.PackageType == "Nights")
                    {
                        model.Package.NightsFree = true;
                        model.Package.PercentageOff = 0;
                    }
                    else if (model.PackageType == "Percentage")
                    {
                        model.Package.PercentOff = true;
                        model.Package.NumberOfNights = 0;
                    }
                    else if (model.PackageType == "Rate")
                    {
                        model.Package.PricePoint = true;
                        model.Package.NumberOfNights = 0;
                        model.Package.PercentageOff = 0;
                    }

                    int id = packageRep.Add(model.Package);

                    if (model.UploadImage != null)
                    {
                        var supportedTypes = new[] { "image/jpg", "image/jpeg", "image/png" };
                        if (!supportedTypes.Contains(model.UploadImage.Image.ContentType))
                        {
                            ModelState.AddModelError("Image", "Invalid type. Only JPG and PNG are allowed.");
                            return View(model);
                        }
                        var uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img/package-images");
                        var fileName = Guid.NewGuid().ToString() + "_" + hotelid.ToString() + "_" + model.UploadImage.Image.FileName;
                        var path = Path.Combine(uploadFolder, fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            model.UploadImage.Image.CopyTo(fileStream);
                        }
                        model.Package.SmImage = fileName;
                        packageRep.UpdateImageToPackage(id, fileName);
                    }
                    if (id > 0 && model.SelectedRoomTypeIds != null)
                    {
                        packageRep.AddRoomToPackage(id, model.SelectedRoomTypeIds.ToList());
                    }
                    else
                    {
                        ModelState.AddModelError("", "There was an error saving your package and selected room with the system");
                    }
                    return RedirectToAction("Index");

                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }
            return View(model);
        }

        public IActionResult Edit(int id)
        {

            Package package = packageRep.GetPackage(id);
            string packageType = string.Empty;
            if (package.NightsFree == true)
                packageType = "Nights";
            else if (package.PercentOff == true)
                packageType = "Percentage";
            else if (package.PricePoint == true)
                packageType = "Rate";

            PackageViewModel model = new PackageViewModel
            {
                Package = package,
                PackageType = packageType,
                RoomTypes = roomRep.GetAllForHotel(package.HotelID),
                SelectedRoomTypeIds = packageRep.GetRoomTypes(id).Select(x => x.RoomTypeID).ToArray()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(PackageViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.Package.Description = HttpUtility.HtmlDecode(model.Package.Description);
                    if (model.PackageType == "Nights")
                    {
                        model.Package.NightsFree = true;
                        model.Package.PercentageOff = 0;
                    }
                    else if (model.PackageType == "Percentage")
                    {
                        model.Package.PercentOff = true;
                        model.Package.NumberOfNights = 0;
                    }
                    else if (model.PackageType == "Rate")
                    {
                        model.Package.PricePoint = true;
                        model.Package.NumberOfNights = 0;
                        model.Package.PercentageOff = 0;
                    }


                    if (model.UploadImage != null)
                    {
                        var supportedTypes = new[] { "image/jpg", "image/jpeg", "image/png" };
                        if (!supportedTypes.Contains(model.UploadImage.Image.ContentType))
                        {
                            ModelState.AddModelError("Image", "Invalid type. Only JPG and PNG are allowed.");
                            return View(model);
                        }
                        var uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img/package-images");
                        var fileName = Guid.NewGuid().ToString() + "_" + model.Package.HotelID.ToString() + "_" + model.UploadImage.Image.FileName;
                        var path = Path.Combine(uploadFolder, fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            model.UploadImage.Image.CopyTo(fileStream);
                        }
                        model.Package.SmImage = fileName;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(model.Package.SmImage))
                        {
                            model.Package.SmImage = string.Empty;
                        }

                    }
                    packageRep.Update(model.Package);

                    if (model.SelectedRoomTypeIds != null)
                        packageRep.AddRoomToPackage(model.Package.PackageID, model.SelectedRoomTypeIds.ToList());
                    else
                    {
                        ModelState.AddModelError("", "There was an error saving your package and selected room with the system");
                    }

                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            }

            catch
            {
                return View(model);

            }
        }

        public IActionResult Delete(int id)
        {
            packageRep.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            PackageViewModel model = new PackageViewModel
            {
                Package = packageRep.GetPackage(id),
                RoomTypes = roomRep.GetAllForHotel(packageRep.GetPackage(id).HotelID),
                Amenities = amenityRep.GetPackageAmenities(id).FindAll(x => x.Mandatory == true)
            };
            return View(model);
        }
    }
}