using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using WRRManagement.Domain.APIModels;
using WRRManagement.Domain.Reservation;
using WRRManagement.Domain.RoomTypes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WRR8_0.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomType roomTypeRepository;
        private readonly IAvailableRackRoom availableRackRoomsRepository;
        private readonly IRoomImage roomImageRepository;
        private readonly IRoomFeatures roomFeaturesRepository;
        private readonly IAdultBase adultBaseRepository;
        private readonly IMaxBase maxBaseReposity;
        private readonly IRoomAllocation roomAllocationRepository;

        public RoomsController(IRoomType roomTypeRepository, IAvailableRackRoom availableRackRooms, IRoomImage roomImage, 
            IRoomFeatures roomFeatures, IAdultBase adultBase, IMaxBase maxBase, IRoomAllocation roomAllocationRepository)
        {
            this.roomTypeRepository = roomTypeRepository;
            this.availableRackRoomsRepository = availableRackRooms;
            this.roomImageRepository = roomImage;
            this.roomFeaturesRepository = roomFeatures;
            this.adultBaseRepository = adultBase;
            this.maxBaseReposity = maxBase;
            this.roomAllocationRepository = roomAllocationRepository;
        }

        // GET: api/Rooms/List/5
        [HttpGet("List/{hotelid}")]
        public IActionResult GetAll(int hotelid)
        {
            List<RoomType> allrooms = roomTypeRepository.GetAllForHotel(hotelid);
            List<ViewRoomModel> list = new List<ViewRoomModel>();
            foreach(var room in allrooms)
            {
                List<RoomImage> images = roomImageRepository.GetRoomImages(room.RoomTypeID);
                foreach(var img in images)
                {
                    img.FileName = Path.Combine("img", "room-images", img.FileName);
                }
                ViewRoomModel model = new ViewRoomModel()
                {
                    roomType = room,
                    mainImage = images.FirstOrDefault(x => x.MainImage).FileName,                    
                    roomImages = images,
                    roomFeatures = roomFeaturesRepository.GetRoomFeatures(room.RoomTypeID)                   
                };
                if (room.AdultBase)
                    model.MaxGuests = adultBaseRepository.GetByRoomID(room.RoomTypeID).MaxRoomTotal;
                else if (room.MaxBase)
                    model.MaxGuests = maxBaseReposity.GetByRoomID(room.RoomTypeID).MaxBaseCount;
                else
                    model.MaxGuests = 2;

                list.Add(model);
            }
            if (list.Count > 0)
                return Ok(list);
            else
                return NotFound();
        }


        [HttpGet("Availability/{hotelId}/{start}/{end}/{adults}/{child}")]
        public IActionResult GetAvailablity(int hotelId, DateTime start, DateTime end, int adults, int child)
        {
            List<AvailableRackRoom> availRacks = new List<AvailableRackRoom>();
            availRacks = availableRackRoomsRepository.GetAvailableRackRooms(hotelId, start, end, adults, child);

            if (availRacks.Count > 0)
                return Ok(availRacks);
            else
                return NotFound();
        }

        // GET api/<RoomsController>/5
        [HttpGet("check/{roomid}/{start}/{end}")]
        public bool CheckAvail(int room, DateTime start, DateTime end)
        {
            bool valid = false;
            try
            {
                valid = roomAllocationRepository.AllocationIsValid(room, start, end);
            }
            catch (Exception ex)
            {
                return false;
            }
            return valid;
        }

        // POST api/<RoomsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

    }

    
}
