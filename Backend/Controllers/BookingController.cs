using Microsoft.AspNetCore.Mvc;
using Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly DataContext _context;
        public BookingController(DataContext context) => _context = context;

        [HttpGet]
        public IActionResult GetBookings([FromQuery] string? date, [FromQuery] string? time)
        {
            var bookings = _context.Bookings!.AsQueryable();
            if (date != null)
            {
                bookings = bookings.Where(x => x.Date == date);
            }
            if (time != null)
            {
                bookings = bookings.Where(x => x.Time == time);
            }
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public IActionResult GetBooking(int? id)
        {
            var booking = _context.Bookings?.FirstOrDefault(c => c.Id == id);
            if (booking == null)
            {
                return NotFound();
            }
            return Ok(booking);
        }

        [HttpPost]
        public IActionResult CreateBooking([FromBody] Booking booking)
        {
            var dbBooking = _context.Bookings!.Find(booking.Id);
            var hairdresser = _context.Hairdressers!.Find(booking.HairdresserId);
            var service = _context.Services!.Find(booking.ServiceId);
            var specialOffer = _context.SpecialOffers!.Find(booking.SpecialOfferId);

            if (dbBooking == null)
            {
                _context.Add(booking);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetBooking), new { Id = booking.Id }, booking);
            }
            else
            {
                return Conflict();
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBooking(int? id, [FromBody] Booking booking)
        {
            var hairdresserId = _context.Hairdressers!.Find(booking.HairdresserId);
            var serviceId = _context.Services!.Find(booking.ServiceId);
            var specialOfferId = _context.SpecialOffers!.Find(booking.SpecialOfferId);
            var dbBooking = _context.Bookings!.AsNoTracking().FirstOrDefault((x => x.Id == booking.Id));
            if (id != booking.Id || dbBooking == null)
            {
                return NotFound();
            }

            _context.Update(booking);
            _context.SaveChanges();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteBooking(int? id)
        {
            var booking = _context.Bookings?.Find(id);
            if (booking == null)
            {
                return NotFound();
            }

            _context.Remove(booking);
            _context.SaveChanges();
            return NoContent();
        }
    }
}