using Microsoft.AspNetCore.Mvc;
using WRRManagement.Domain.Hotels;
using WRR8_0.Extension;
using WRR8_0.Models;
using WRR8_0.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using WRRManagement.Domain.Packages;
using WRRManagement.Infrastructure.RoomTypeRespository;

namespace WRR8_0.Controllers
{
    public class TierLevelController : Controller
    {
        private readonly ITierLevel _tierLevelRepositoty;
        private readonly IPackageTierLevel _packageTierLevelRepository;
        private readonly IPackage _packageRepository;

        public TierLevelController(ITierLevel tierLevelRepositoty, IPackageTierLevel packageTierLevelRepository, IPackage packageRepository)
        {
            _tierLevelRepositoty = tierLevelRepositoty;
            _packageTierLevelRepository = packageTierLevelRepository;
            _packageRepository = packageRepository;
        }
        public IActionResult Index()
        {
            this.RestoreModelState();
            int HotelID = HttpContext.Session.GetInt("HotelID");
            TierLevelViewModel model = new TierLevelViewModel()
            {
                SelectedID = HotelID
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(TierLevelViewModel model)
        {
            int HotelID = HttpContext.Session.GetInt("HotelID");
            if (ModelState.IsValid) { 
                if(model.StartDate > DateTime.MinValue && model.EndDate > DateTime.MinValue)
                {
                    
                    try
                    {
                        _tierLevelRepositoty.AddDateRange(HotelID, model.StartDate.Value, model.EndDate.Value, model.TierLevel);
                    }
                    catch
                    {
                        ModelState.AddModelError("", "There was an error saving your tier level");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please enter valid dates");
                }
            }
            this.SerializeModelState();
            return RedirectToAction("Index");
        }

        public IActionResult PackageTier(int? packageid)
        {
            int id = packageid ?? 0;
            int HotelID = HttpContext.Session.GetInt("HotelID");
            List<Package> packages = _packageTierLevelRepository.GetPackagesWithTier(HotelID);

            TierLevelViewModel model = new TierLevelViewModel()
            {
                Packages = packages,
                SelectedID = id,
                StartDate = null,
                EndDate = null,
                TierLevel = 'A'
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PackageTier(TierLevelViewModel model) 
        {
            int hotelID = HttpContext.Session.GetInt("HotelID");
            model.Packages = _packageTierLevelRepository.GetPackagesWithTier(hotelID);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PackageCreate(TierLevelViewModel model)
        {

            if (ModelState.IsValid)
            {
                ModelState.Clear();
                if (model.StartDate > DateTime.MinValue && model.EndDate > DateTime.MinValue)
                {
                    try
                    {
                        _packageTierLevelRepository.AddDateRange(model.SelectedID, model.StartDate.Value, model.EndDate.Value, model.TierLevel);
                    }
                    catch
                    {
                        ModelState.AddModelError("", "There was an error saving your Tier level");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please enter a valid date");
                }

            }
            this.SerializeModelState();
            return RedirectToAction("PackageTier", new { packageid = model.SelectedID });
        }

    }
}
