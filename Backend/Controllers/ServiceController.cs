using Microsoft.AspNetCore.Mvc;
using Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly DataContext _context;
        public ServiceController(DataContext context) => _context = context;

        [HttpGet]
        public IActionResult GetServices()
        {
            var services = _context.Services!.AsQueryable();
            return Ok(services);
        }

        [HttpGet("{id}")]
        public IActionResult GetService(int? id)
        {
            var service = _context.Services?.FirstOrDefault(c => c.Id == id);
            if (service == null)
            {
                return NotFound();
            }
            return Ok(service);
        }

        [HttpPost]
        public IActionResult CreateService([FromBody] Service service)
        {
            var dbService = _context.Services?.Find(service.Id);
            if (dbService == null)
            {
                _context.Add(service);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetService), new { Id = service.Id }, service);
            }
            else
            {
                return Conflict();
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateService(int? id, [FromBody] Service service)
        {
            var dbService = _context.Services!.AsNoTracking().FirstOrDefault(x => x.Id == service.Id);
            if (id != service.Id || dbService == null)
            {
                return NotFound();
            }
            if (service.Title != service.Title?.ToString() || service.Price <= 0
                || service.Duration != DateTime.Parse(service.Duration).ToString("HH:mm"))
            {
                return BadRequest();
            }

            _context.Update(service);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteService(int? id)
        {
            var service = _context.Services?.Find(id);
            if (service == null)
            {
                return NotFound();
            }
            _context.Remove(service);
            _context.SaveChanges();

            return NoContent();
        }
    }
}