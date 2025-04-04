using Microsoft.AspNetCore.Mvc;
using WRR8_0.Extension;
using WRRManagement.Domain.Packages;
using WRR8_0.Models;
using WRRManagement.Domain.RoomTypes;

namespace WRR8_0.Controllers
{
    public class PackageAllocationController : Controller
    {
        private readonly IPackageAllocation _packageAllocationRep;
        private readonly IPackage _packageRep;

        public PackageAllocationController(IPackageAllocation packageAllocationRep, IPackage packageRep)
        {
            _packageAllocationRep = packageAllocationRep;
            _packageRep = packageRep;
        }
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
