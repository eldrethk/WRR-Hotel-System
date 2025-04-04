using Microsoft.AspNetCore.Mvc;
using WRR8_0.Extension;
using WRR8_0.Models;
using WRRManagement.Domain.RoomTypes;

namespace WRR8_0.Controllers
{
    public class StayRestrictionsController : Controller
    {
        private readonly IMinStay _minStayRepository;
        private readonly IRoomType _roomTypeRepository;

        public StayRestrictionsController(IMinStay minStayRepository, IRoomType roomTypeReposity)
        {
            _minStayRepository = minStayRepository;
            _roomTypeRepository = roomTypeReposity;
        }
        public IActionResult Index(int? roomid)
        {
            this.RestoreModelState();
            int id = roomid ?? 0;
            int HotelID = HttpContext.Session.GetInt("HotelID");
            List<RoomType> hotelRooms = _roomTypeRepository.GetAllForHotel(HotelID);
            string roomName = string.Empty;

            if (id > 0)
                roomName = hotelRooms.FirstOrDefault(x => x.RoomTypeID == id)?.Name ?? "Room Not Found";

            RoomEventViewModel model = new RoomEventViewModel
            {
                Rooms = hotelRooms,
                StartDate = null,
                EndDate = null,
                Quantity = null,
                SelectedRoomTypeID = id,
                SelectRoomName = roomName
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(RoomEventViewModel model)
        {
            this.RestoreModelState();
            int hotelID = HttpContext.Session.GetInt("HotelID");
            model.Rooms = _roomTypeRepository.GetAllForHotel(hotelID);
            if (model.SelectedRoomTypeID > 0)
                model.SelectRoomName = model.Rooms.FirstOrDefault(x => x.RoomTypeID == model.SelectedRoomTypeID)?.Name;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RoomEventViewModel model) {
            if (ModelState.IsValid) {
                
                ModelState.Clear();
                if (model.StartDate > DateTime.MinValue && model.EndDate > DateTime.MinValue)
                {
                    try
                    {
                        _minStayRepository.AddDateRange(model.SelectedRoomTypeID, model.StartDate.Value, model.EndDate.Value, model.Quantity.Value);
                    }
                    catch
                    {
                        ModelState.AddModelError("", "There was an error saving your stay restriction");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please enter valid dates");
                }
            }
            this.SerializeModelState();
            return RedirectToAction("Index", new {roomid = model.SelectedRoomTypeID});
        }

        public IActionResult OpenModal(int id)
        {
            RoomEventViewModel model = new RoomEventViewModel()
            {
                SelectedRoomTypeID = id,
                SelectRoomName = _roomTypeRepository.GetById(id).Name ?? string.Empty
            };
            return PartialView("_daterange", model);

        }
    }
}
