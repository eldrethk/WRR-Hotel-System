using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WRRManagement.Domain.Hotels;
using WRRManagement.Domain.APIModels;

namespace WRR8_0.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelInfoController : ControllerBase
    {
        private readonly IDisclaimer disclaimerRepository;
        private readonly IHotel hotelRepository;

        public HotelInfoController(IDisclaimer disclaimer, IHotel hotel)
        {
            this.disclaimerRepository = disclaimer;
            this.hotelRepository = hotel;
        }

        // GET: api/HotelInfo/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var disclaimer = disclaimerRepository.GetDisclaimer(id);
            Hotel hotelInfo = hotelRepository.GetHotel(id);

            HotelInfo info = new HotelInfo
            {
                Disclaimer = disclaimer.DisclaimerText,
                EmailDisclaimer = disclaimer.EmailDisclaimerText,
                HotelPhone = hotelInfo.TollFreePhone
            };
            
            if (info != null)
                return Ok(info);
            else return NotFound();
        }

    }

  

}
