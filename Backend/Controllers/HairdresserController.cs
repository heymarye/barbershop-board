using Microsoft.AspNetCore.Mvc;
using Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HairdresserController : ControllerBase
    {
        private readonly DataContext _context;
        public HairdresserController(DataContext context) => _context = context;

        [HttpGet]
        public IActionResult GetHairdressers()
        {
            var hairdresser = _context.Hairdressers!.AsQueryable();
            return Ok(hairdresser);
        }

        [HttpGet("{id}")]
        public IActionResult GetHairdresser(int? id)
        {
            var hairdresser = _context.Hairdressers?.FirstOrDefault(c => c.Id == id);
            if (hairdresser == null)
            {
                return NotFound();
            }
            return Ok(hairdresser);
        }

        [HttpPost]
        public IActionResult CreateHairdresser([FromBody] Hairdresser hairdresser)
        {
            var dbHairdresser = _context.Hairdressers?.Find(hairdresser.Id);
            if (dbHairdresser == null)
            {
                _context.Add(hairdresser);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetHairdresser), new { Id = hairdresser.Id }, hairdresser);
            }
            else
            {
                return Conflict();
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateHairdresser(int? id, [FromBody] Hairdresser hairdresser)
        {
            var dbHairdresser = _context.Hairdressers!.AsNoTracking().FirstOrDefault(x => x.Id == hairdresser.Id);
            if (id != hairdresser.Id || dbHairdresser == null)
            {
                return NotFound();
            }

            _context.Update(hairdresser);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteHaidresser(int? id)
        {
            var hairdresser = _context.Hairdressers?.Find(id);
            if (hairdresser == null)
            {
                return NotFound();
            }
            _context.Remove(hairdresser);
            _context.SaveChanges();

            return NoContent();
        }
    }
}