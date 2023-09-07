using Microsoft.AspNetCore.Mvc;
using Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly DataContext _context;
        public ClientController(DataContext context) => _context = context;

        [HttpGet]
        public IActionResult GetClients([FromQuery] string? lastName)
        {
            var clients = _context.Clients!.AsQueryable();
            if (lastName != null)
            {
                clients = clients.Where(x => x.LastName == lastName);
            }
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public IActionResult GetClient(int? id)
        {
            var client = _context.Clients?.FirstOrDefault(c => c.Id == id);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpPost]
        public IActionResult CreateClient([FromBody] Client client)
        {
            var dbClient = _context.Clients?.Find(client.Id);
            if (dbClient == null)
            {
                _context.Add(client);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetClient), new { Id = client.Id }, client);
            }
            else
            {
                return Conflict();
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateClient(int? id, [FromBody] Client client)
        {
            var dbClient = _context.Clients!.AsNoTracking().FirstOrDefault(x => x.Id == client.Id);
            if (id != client.Id || dbClient == null)
            {
                return NotFound();
            }

            _context.Update(client);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteClient(int? id)
        {
            var client = _context.Clients?.Find(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Remove(client);
            _context.SaveChanges();
            return NoContent();
        }
    }
}