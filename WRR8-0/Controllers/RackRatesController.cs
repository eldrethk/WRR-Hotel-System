using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using WRR8_0.Extension;
using WRR8_0.Models;
using WRRManagement.Domain.Hotels;
using WRRManagement.Domain.RoomTypes;
using WRRManagement.Infrastructure.RoomTypeRespository;

namespace WRR8_0.Controllers
{
    public class RackRatesController : Controller
    {
        private readonly IRoomType _roomTypeRep;
        private readonly IRackRate _rackRateRep;

        public RackRatesController(IRoomType roomTypeRep, IRackRate rackRateRep)
        {
            _roomTypeRep = roomTypeRep;
            _rackRateRep = rackRateRep;
        }
        public IActionResult Index(int? roomID)
        {
            this.RestoreModelState();
            int hotelID = HttpContext.Session.GetInt("HotelID");        
            RackRackViewModel model = new RackRackViewModel()
            {
                HotelRooms = GetRooms(hotelID),
                SelectedRoomID = roomID ?? -1,
                HotelID = hotelID
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(RackRackViewModel model)
        {
            int hotelID = HttpContext.Session.GetInt("HotelID");         
            model.HotelRooms = GetRooms(hotelID);
            model.HotelID = hotelID;
            return View(model);
        }

        public List<RoomType> GetRooms(int hotelId) 
        {
            List<RoomType> roomList = new List<RoomType>();
            roomList.AddRange(_roomTypeRep.GetAllForHotel(hotelId));
            roomList.Add(new RoomType() { RoomTypeID = 0, Name = "All Rooms" });
            return roomList;
        }
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DateRange(DateRangeViewModel model)
        {           
            if (ModelState.IsValid)
            {
                ModelState.Clear();
                if (model.SelectedRoomTypeID > 0)
                {
                    if (!_rackRateRep.CheckDates(model.SelectedRoomTypeID, model.StartDate, model.EndDate))
                    {
                        AddEmptyRackRate(model.SelectedRoomTypeID, model.StartDate, model.EndDate);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Date Range overlaps with another date range");
                    }
                }
                else if(model.SelectedRoomTypeID == 0)
                {
                    int HotelID = HttpContext.Session.GetInt("HotelID");
                    List<RoomType> roomList = _roomTypeRep.GetAllForHotel(HotelID);
                    foreach(RoomType room in roomList)
                    {
                        bool valid = _rackRateRep.CheckDates(room.RoomTypeID, model.StartDate, model.EndDate);
                        if (!valid)
                        {
                            AddEmptyRackRate(room.RoomTypeID, model.StartDate, model.EndDate);
                        }
                        else
                        {
                            ModelState.AddModelError("", room.Name + " date range overlaps with another date range");
                        }

                    }
                }
            }
            this.SerializeModelState();
            return RedirectToAction("Index", new { roomID = model.SelectedRoomTypeID});
        }

        private void AddEmptyRackRate(int roomID, DateTime start, DateTime end)
        {
            _rackRateRep.Add(new RackRate()
            {
                RoomTypeID = roomID,
                StartDate = start,
                EndDate = end,
                TierARate = 0,
                TierBRate = 0,
                TierCRate = 0
            });
        }
        public IActionResult OpenModal(int id)
        {
            string name = string.Empty;
            if (id > 0)
                name = _roomTypeRep.GetById(id).Name;
            else
                name = "All Rooms";

            DateRangeViewModel model = new DateRangeViewModel()
            {
                SelectedRoomTypeID = id,
                SelectRoomName = name
            };
            return PartialView("_daterange", model);
        }

    }
}
