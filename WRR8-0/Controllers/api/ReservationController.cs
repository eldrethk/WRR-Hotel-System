using Microsoft.AspNetCore.Mvc;
using WRRManagement.Domain.Reservation;
using WRRManagement.Domain.System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WRR8_0.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {

        private IOptInEmails optInEmailsRepository;
        private IReservation reservationRepository;

        public ReservationController(IOptInEmails optInEmails, IReservation reservation)
        {
            this.optInEmailsRepository = optInEmails;
            this.reservationRepository = reservation;
        }
        // GET: api/Reservation
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/Reservation/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/Reservation
        [HttpPost]
        public IActionResult Post([FromBody] Reservation reservation)
        {
            if (reservation == null)
            {
                return BadRequest("Reservation is null");
            }

            try
            {
                int reservationID = reservationRepository.AddReservation(reservation);
                return CreatedAtAction(nameof(Get), new { id = reservationID }, reservation);
            }
            catch
            {
                return BadRequest("Reservation is null");
            }
        }

        // POST api/OptIn/5
        [HttpPost("OptIn/")]
        public IActionResult PostOptIn([FromBody] string email, int hotelId, string firstname, string lastname, string state)
        {
            if (email == null)
            {
                return BadRequest("Email is null");
            }
            // Add email to database
            int optID = optInEmailsRepository.Add(email, hotelId, firstname, lastname, state);
            return CreatedAtAction(nameof(Get), new { id = optID }, email);
        }

        // DELETE api/Reservation/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
