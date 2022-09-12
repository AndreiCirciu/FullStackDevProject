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
                   Password = "abc",
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
                   Password = "abcdef",
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
                   Password = "abcdefgh",
                   DateOfBirth = "19.02.1968",
                   Phone = "0711299",
                   Address = "Bucuresti",
                   Funds = 1000000000,
                   IsAdmin = 0
                }
            };
        [HttpPost("addAccount")]
        public async Task<ActionResult<List<Account>>> AddAccount(Account account)
        {
            accounts.Add(account);
            return Ok(accounts);
        }

        [HttpPut("updateAccount")]
        public async Task<ActionResult<List<Account>>> UpdateAccount(Account request)
        {
            var account = accounts.Find(m => m.ID == request.ID);
            if (account == null)
                return BadRequest("Medicine not found");
            account.FirstName = request.FirstName;
            account.LastName = request.LastName;
            account.FirstName = request.FirstName;
            account.FirstName = request.FirstName;
            account.FirstName = request.FirstName;
            account.FirstName = request.FirstName;
            return Ok(accounts);
        }
    }
}
