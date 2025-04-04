using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Runtime.InteropServices;
using System.Text.Json;
using WRR8_0.Extension;
using WRR8_0.Models;
using WRRManagement.Domain.Hotels;
using WRRManagement.Domain.RoomTypes;
using WRRManagement.Infrastructure.RoomTypeRespository;

namespace WRR8_0.Controllers
{
    public class RoomAllocationController : Controller
    {
        private readonly IRoomType _roomTypeRepository;
        private readonly IRoomAllocation _roomAllocationRepository;
        private readonly ILogger<RoomAllocationController> _logger;

        public RoomAllocationController(IRoomType roomTypeRepository, IRoomAllocation roomAllocationRepository, ILogger<RoomAllocationController> logger)
        {
            _roomTypeRepository = roomTypeRepository;
            _roomAllocationRepository = roomAllocationRepository;
            _logger = logger;
        }
        // GET: RoomAllocationController
 
        public IActionResult Index(int? roomid)
        {
            this.RestoreModelState();
            int id = roomid ?? 0;
            int HotelID = HttpContext.Session.GetInt("HotelID");
            List<RoomType> hotelRooms = _roomTypeRepository.GetAllForHotel(HotelID);
            string roomName = string.Empty;
            
            if (id > 0)
                roomName = hotelRooms.FirstOrDefault(x => x.RoomTypeID == id)?.Name ?? string.Empty;

            RoomEventViewModel roomEventViewModel = new RoomEventViewModel
            {
                Rooms = hotelRooms,
                StartDate = null,
                EndDate = null,
                Quantity = null,
                SelectedRoomTypeID = id,
                SelectRoomName = roomName
            };

            return View(roomEventViewModel);
        }
        

        // POST: RoomAllocationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(RoomEventViewModel model)
        {
            this.RestoreModelState();
            int hotelID = HttpContext.Session.GetInt("HotelID");
            model.Rooms = _roomTypeRepository.GetAllForHotel(hotelID);
            if(model.SelectedRoomTypeID > 0)
                model.SelectRoomName = model.Rooms.FirstOrDefault(x => x.RoomTypeID == model.SelectedRoomTypeID)?.Name;
              
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(RoomEventViewModel model) {
            
            if (ModelState.IsValid)
            {
                ModelState.Clear();
                if (model.StartDate > DateTime.MinValue && model.EndDate > DateTime.MinValue)
                {
                    try
                    {
                        _roomAllocationRepository.AddDateRange(model.SelectedRoomTypeID, model.StartDate.Value, model.EndDate.Value, model.Quantity.Value);                       
                    }
                    catch
                    {
                        ModelState.AddModelError("", "There was an error saving your room allocation");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please enter a valid date");
                }
             
            }
            this.SerializeModelState();
            return RedirectToAction("Index", new { roomid = model.SelectedRoomTypeID });
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
