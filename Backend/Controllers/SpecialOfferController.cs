using Microsoft.AspNetCore.Mvc;
using Backend.Model;
using Backend.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialOfferController : ControllerBase
    {
        private readonly DataContext _context;
        public SpecialOfferController(DataContext context) => _context = context;

        [HttpGet]
        public IActionResult GetSpecialOffers([FromQuery] string? fromDate, [FromQuery] string? toDate)
        {
            var specialOffers = _context.SpecialOffers!.AsQueryable();
            if (fromDate != null)
            {
                specialOffers = specialOffers.Where(x => x.FromDate == fromDate);
            }
            if (toDate != null)
            {
                specialOffers = specialOffers.Where(x => x.ToDate == toDate);
            }
            return Ok(specialOffers);
        }

        [HttpGet("{id}")]
        public IActionResult GetSpecialOffer(int? id)
        {
            var specialOffer = _context.SpecialOffers?.FirstOrDefault(c => c.Id == id);
            if (specialOffer == null)
            {
                return NotFound();
            }
            return Ok(specialOffer);
        }

        [HttpPost]
        public IActionResult CreateSpecialOffer([FromBody] SpecialOffer specialOffer)
        {
            var dbSpecialOffer = _context.SpecialOffers!.Find(specialOffer.Id);
            var service = _context.Services!.Find(specialOffer.ServiceId);

            if (dbSpecialOffer == null)
            {
                var generateCode = new SpecialOffer
                {
                    Id = specialOffer.Id,
                    ServiceId = specialOffer.ServiceId,
                    Code = RandomHelper.RandomString(10),
                    Percentage = specialOffer.Percentage,
                    FromDate = specialOffer.FromDate,
                    ToDate = specialOffer.ToDate
                };
                _context.Add(generateCode);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetSpecialOffer), new { Id = specialOffer.Id }, specialOffer);
            }
            else
            {
                return Conflict();
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSpecialoffer(int? id, [FromBody] SpecialOffer specialOffer)
        {
            var serviceId = _context.Services!.Find(specialOffer.ServiceId);
            var dbSpecialOffer = _context.SpecialOffers!.AsNoTracking().FirstOrDefault(x => x.Id == specialOffer.Id);
            if (id != specialOffer.Id || dbSpecialOffer == null)
            {
                return NotFound();
            }
            _context.Update(specialOffer);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSpecialOffer(int? id)
        {
            var specialOffer = _context.SpecialOffers?.Find(id);
            if (specialOffer == null)
            {
                return NotFound();
            }
            _context.Remove(specialOffer);
            _context.SaveChanges();
            return NoContent();
        }
    }
}