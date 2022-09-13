using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FSDProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private static List<Account> accounts = new List<Account>
            {
                new Account {
                   ID = 1,
                   FirstName = "Andrei",
                   LastName = "Circiu",
                   DateOfBirth = "25.05.1998",
                   Phone = "07123123",
                   Address = "Bucuresti",
                   Funds = 25000,
                   IsAdmin = 1
                },

                new Account {
                   ID = 1,
                   FirstName = "Ionut",
                   LastName = "Cercel",
                   DateOfBirth = "29.08.1996",
                   Phone = "0714893",
                   Address = "Bucuresti",
                   Funds = 1000000,
                   IsAdmin = 0
                },

                new Account {
                   ID = 1,
                   FirstName = "Petrica",
                   LastName = "Cercel",
                   DateOfBirth = "19.02.1968",
                   Phone = "0711299",
                   Address = "Bucuresti",
                   Funds = 1000000000,
                   IsAdmin = 0
                }
            };
        private readonly DataContext _context;
        public AccountController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("addAccount")]
        public async Task<ActionResult<List<Account>>> AddAccount(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return Ok(await _context.Accounts.ToListAsync());     
        }

        [HttpPut("updateAccount")]
        public async Task<ActionResult<List<Account>>> UpdateAccount(Account request)
        {
            var dbAccount = await _context.Accounts.FindAsync(request.ID);
            if (dbAccount == null)
                return BadRequest("Account not found.");
            dbAccount.FirstName = request.FirstName;
            dbAccount.LastName = request.LastName;
            dbAccount.DateOfBirth = request.DateOfBirth;
            dbAccount.Phone = request.Phone;
            dbAccount.Address = request.Address;
            dbAccount.Funds = request.Funds;
            dbAccount.IsAdmin = request.IsAdmin;

            await _context.SaveChangesAsync();

            return Ok(await _context.Accounts.ToListAsync());
        }

        [HttpGet("getAllAccounts")]
        public async Task<ActionResult<List<Account>>> Get()
        {
            return Ok(await _context.Accounts.ToListAsync());
        }

        [HttpGet("getAccountById")]
        public async Task<ActionResult<Account>> Get(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
                return BadRequest("Account not found.");
            return Ok(account);
        }

        [HttpDelete("deleteAccountById")]
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

