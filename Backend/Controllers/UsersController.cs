using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Backend.Model;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly DataContext _context;
        private IConfiguration _config;

        public UsersController(IConfiguration config, DataContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _context.UserList!.AsQueryable();
            return Ok(users);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User login)
        {
            var dbUser = _context.UserList!.FirstOrDefault(user => user.Username == login.Username);

            if (dbUser == null) return NotFound();

            if (dbUser.Password != HashPassword(login.Password)) return Unauthorized();

            var token = GenerateJSONWebToken(dbUser);

            return Ok(new { token = token });

        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int? id)
        {
            var user = _context.UserList?.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            var dbUser = _context.UserList?.Find(user.Username);
            if (dbUser == null)
            {
                _context.Add(user);
                _context.SaveChanges();
                return CreatedAtAction(nameof(GetUser), new { Id = user.Id }, user);
            }
            else
            {
                return Conflict();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int? id)
        {
            var user = _context.UserList?.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            _context.Remove(user);
            _context.SaveChanges();

            return NoContent();
        }

        private string HashPassword(string password)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: new byte[0],
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        private string GenerateJSONWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(10),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}