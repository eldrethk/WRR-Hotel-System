using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WRR8_0.Extension;
using WRR8_0.Models;
using WRRManagement.Domain.RoomTypes;

namespace WRR8_0.Controllers
{
    public class RoomTypeController : Controller
    {
        private readonly IRoomType roomTypeRepository;
        private readonly IAdultBase adultBaseRepository;
        private readonly IMaxBase maxBaseRepository;
        private readonly IRoomFeatures roomFeaturesRepository;

        public RoomTypeController(IRoomType roomTypeRepository, IAdultBase adultBaseRepository, IMaxBase maxBaseRepository, IRoomFeatures roomFeaturesRep)
        {
            this.roomTypeRepository = roomTypeRepository;
            this.adultBaseRepository = adultBaseRepository;
            this.maxBaseRepository = maxBaseRepository;
            this.roomFeaturesRepository = roomFeaturesRep;
        }
        // GET: RoomTypeController
        public IActionResult Index()
        {
            int hotelId = HttpContext.Session.GetInt("HotelID");
            List<RoomType> list = roomTypeRepository.GetAllForHotel(hotelId);

            return View(list);
        }

        // GET: RoomTypeController1/Details/5
        public IActionResult Details(int id)
        {
            RoomType room = roomTypeRepository.GetById(id);
            return View(room);
        }

        // GET: RoomTypeController1/Create
        public IActionResult Create()
        {
            var model = new RoomTypeViewModel();
            return View(model);
        }

        // POST: RoomTypeController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RoomTypeViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.RoomType.HotelID = HttpContext.Session.GetInt("HotelID");
                    if (model.BaseFeeType == "Adult")
                        model.RoomType.AdultBase = true;
                    else if (model.BaseFeeType == "Max")
                        model.RoomType.MaxBase = true;

                    int id = roomTypeRepository.Add(model.RoomType);

                    if (model.RoomType.AdultBase == true)
                    {                       
                        model.AdultBaseFee.RoomTypeID = id;
                        adultBaseRepository.Add(model.AdultBaseFee);
                    }
                    else if (model.RoomType.MaxBase == true)
                    {                   
                        model.MaxBaseFee.RoomTypeID = id;
                        maxBaseRepository.Add(model.MaxBaseFee);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model.RoomType);
            }
        }

        // GET: RoomTypeController1/Edit/5
        public IActionResult Edit(int id)
        {
            RoomTypeViewModel model = new RoomTypeViewModel();
            try
            {
                model.RoomType = roomTypeRepository.GetById(id);

                if (model.RoomType.AdultBase)
                {
                    model.AdultBaseFee = adultBaseRepository.GetByRoomID(model.RoomType.RoomTypeID);
                    model.BaseFeeType = "Adult";
                }
                else
                    model.AdultBaseFee = new AdultBase();

                if (model.RoomType.MaxBase)
                {
                    model.MaxBaseFee = maxBaseRepository.GetByRoomID(model.RoomType.RoomTypeID);
                    model.BaseFeeType = "Max";
                }
                else
                    model.MaxBaseFee = new MaxBase();

                return View(model);
            }
            catch (Exception ex) { }
            return View();
        }

        // POST: RoomTypeController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(RoomTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.BaseFeeType == "Adult")
                    {
                        model.RoomType.AdultBase = true;
                        model.AdultBaseFee.RoomTypeID = model.RoomType.RoomTypeID;
                        adultBaseRepository.Add(model.AdultBaseFee);
                    }
                    else if (model.BaseFeeType == "Max")
                    {
                        model.RoomType.MaxBase = true;
                        model.MaxBaseFee.RoomTypeID = model.RoomType.RoomTypeID;
                        maxBaseRepository.Add(model.MaxBaseFee);
                    }
                    roomTypeRepository.Update(model.RoomType);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View(model);
                }
            }
            return View(model);
        }

        // GET: RoomTypeController1/Delete/5
        public IActionResult Delete(int id)
        {
            roomTypeRepository.Invisible(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteFeatures(int id, int roomid)
        {
            roomFeaturesRepository.Delete(id);
            return RedirectToAction("RoomFeatures", new { id = roomid });
        }

        public IActionResult OpenModal(int id)
        {
            RoomFeatures roomFeatures = new RoomFeatures
            {
                RoomTypeID = id
            };
            return PartialView("_RoomFeatures", roomFeatures);
        }

        [HttpGet]
        public IActionResult RoomFeatures(int id)
        {
            List<RoomFeatures> list = roomFeaturesRepository.GetRoomFeatures(id);
            RoomType room = roomTypeRepository.GetById(id);
            ViewData["RoomTypeID"] = room.RoomTypeID;
            ViewData["RoomName"] = room.Name;
            return View(list);
        }

        [HttpPost]
        public IActionResult AddFeatures(RoomFeatures model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int id = roomFeaturesRepository.Add(model);
                }
                catch
                {
                    ModelState.AddModelError("", "There was an error saving your room feature");
                }
            }
            else
                ModelState.AddModelError("", "There was an error");

            return RedirectToAction("RoomFeatures", new { id = model.RoomTypeID });
        }

     
    }
}
