using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;

namespace FSDProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;

        public AuthController(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;

        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            if (!await VerifyUsername(request.UserName))
            {
                return BadRequest("An user with that username already exists");
            }

            user.Username = request.UserName;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.isAdmin = request.isAdmin;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(await _context.Users.ToListAsync());


        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            var creds = await _context.Users.FirstOrDefaultAsync(e => e.Username == request.UserName);

            if (creds == null)
            {
                return BadRequest("No user");
            }

            if (creds.Username != request.UserName)
            {
                return BadRequest("User not found.");
            }

            if (!VerifyPasswordHash(request.Password, creds.PasswordHash, creds.PasswordSalt))
            {
                return BadRequest("Incorrect Password");
            }

            string token = CreateToken(creds);

            var json = JsonConvert.SerializeObject(new { jwtToken = token });
            return Ok(json);
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.isAdmin == 1 ? "Admin" : "User")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            SigningCredentials cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes((string)password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private async Task<bool> VerifyUsername(string username)
        {
            var creds = await _context.Users.FirstOrDefaultAsync(e => e.Username == username);

            return creds == null;

        }

        [HttpGet("getIfAdminOrUser")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Account>> GetByUses(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Username == username);
            if (user == null)
            {
                return BadRequest("Username not found.");
            }
            var json = JsonConvert.SerializeObject(new { isadmin = user.isAdmin });
            return Ok(json);

        }

        [HttpGet("getUserId")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Account>> GetId(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Username == username);
            if (user == null)
            {
                return BadRequest("Username not found.");
            }
            var json = JsonConvert.SerializeObject(new { userid = user.ID });
            return Ok(json);

        }


    }
}
