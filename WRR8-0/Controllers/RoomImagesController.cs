using Microsoft.AspNetCore.Mvc;
using WRR8_0.Models;
using WRRManagement.Domain.RoomTypes;

namespace WRR8_0.Controllers
{

    public class RoomImagesController : Controller
    {
        private readonly IRoomImage roomImageRep;
        private readonly IRoomType roomTypeRep;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public RoomImagesController(IRoomImage roomImageRep, IRoomType roomTypeRep, IWebHostEnvironment webHostEnvironment)
        {
            this.roomImageRep = roomImageRep;
            this.roomTypeRep = roomTypeRep;
            _hostingEnvironment = webHostEnvironment;
        }
        public IActionResult Index(int id)
        {
            List<RoomImage> images = roomImageRep.GetRoomImages(id);
            if (images == null)
                return NotFound();
            RoomType roomType = roomTypeRep.GetById(id);
            ViewData["RoomTypeName"] = roomType.Name;
            ViewData["RoomTypeID"] = roomType.RoomTypeID;

            if (roomType == null)
                return NotFound();

            return View(images);

        }

        public IActionResult SetMainImage(int id, int roomid)
        {
            roomImageRep.SetMainImage(id, roomid);
            return RedirectToAction("Index", new { id = roomid });
        }

        [HttpGet]
        public IActionResult Create(int roomid)
        {
            UploadImageViewModel model = new UploadImageViewModel { Id = roomid };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UploadImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image != null)
                {
                    var supportedTypes = new[] { "image/jpg", "image/jpeg", "image/png" };
                    if (!supportedTypes.Contains(model.Image.ContentType))
                    {
                        ModelState.AddModelError("Image", "Invalid type. Only JPG and PNG are allowed.");
                        return View(model);
                    }

                    int roomId = model.Id;
                    var uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "img/room-images");
                    var fileName = Guid.NewGuid().ToString() + "_" + roomId.ToString() + "_" + model.Image.FileName;
                    var path = Path.Combine(uploadFolder, fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(fileStream);
                    }
                    var roomImage = new RoomImage()
                    {
                        FileName = fileName,
                        RoomTypeID = roomId,
                        ContentLength = model.Image.Length,
                        ContentType = model.Image.ContentType,
                        Visible = true

                    };
                    roomImageRep.Add(roomImage);

                    return RedirectToAction("Index", new { id = roomId });
                }
            }
            return View(model);
        }

        public IActionResult Delete(int id, int roomid)
        {
            roomImageRep.Invisible(id);
            return RedirectToAction("Index", new { id = roomid });
        }
    }
}
    
