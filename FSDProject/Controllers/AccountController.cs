using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSDProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DataContext _context;
        public AccountController(DataContext context)
        {
            _context = context;
        }
        
        [HttpPost("addAccount")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<List<Account>>> AddAccount(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return Ok(await _context.Accounts.ToListAsync());
        }

        
        [HttpPut("updateAccount")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<List<Account>>> UpdateAccount(Account request)
        {
            var dbAccount = await _context.Accounts.FindAsync(request.ID);
            var dbUser = await _context.Users.FindAsync(request.ID);
            if (dbAccount == null || dbUser == null)
                return BadRequest("Account not found.");
            dbAccount.FirstName = request.FirstName;
            dbAccount.LastName = request.LastName;
            dbAccount.DateOfBirth = request.DateOfBirth;
            dbAccount.Phone = request.Phone;
            dbAccount.Address = request.Address;
            dbAccount.Funds += request.Funds;
            dbAccount.IsAdmin = request.IsAdmin;
            dbUser.isAdmin = request.IsAdmin;

            await _context.SaveChangesAsync();

            return Ok(await _context.Accounts.ToListAsync());
        }

        [HttpGet("getAllAccounts")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Account>>> Get()
        {
            return Ok(await _context.Accounts.ToListAsync());
        }


        [HttpGet("getAccountById")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Account>> Get(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
                return BadRequest("Account not found.");
            return Ok(account);
        }

        [HttpDelete("deleteAccountById")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Account>>> Delete(int id)
        {
            var dbAccount = await _context.Accounts.FindAsync(id);
            if (dbAccount == null)
                return BadRequest("Account not found.");

            _context.Accounts.Remove(dbAccount);
            await _context.SaveChangesAsync();

            return Ok(await _context.Accounts.ToListAsync());
        }
    }
}

