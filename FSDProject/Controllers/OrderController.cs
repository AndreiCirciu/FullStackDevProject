using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSDProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly DataContext _context;
        public OrderController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("getOrderByUserId")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<Order>> GetOrderByUserId(int id)
        {
            var order = await _context.Orders.Where(o => o.userId == id).ToListAsync();

            if (order == null)
            {
                return BadRequest("Order was not found.");
            }

            if (order.Count() <= 0)
            {
                return BadRequest("Your cart is empty");
            }

            return Ok(order);
        }

        [HttpPut("checkOut")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<Order>> Checkout(int id)
        {
            var carts = await _context.Carts.Where(c => c.UserId == id).ToListAsync();
            if (carts == null)
            {
                return BadRequest("Cart is empty");
            }
            double price = 0;
            var account = await _context.Accounts.FindAsync(id);
            foreach (var cart in carts)
            {
                price += cart.Price;
            }
            if (account != null)
            {
                if (account.Funds < price)
                {
                    return BadRequest("You are out of Funds!");
                }
            }

            string productNames = "";
            var order = new Order();
            foreach (var cart in carts)
            {
                _context.Carts.Remove(cart);
                var product = await _context.Medicines.FindAsync(cart.MedicineId);
                if (product != null)
                {
                    productNames += product.Name + " ";
                }
                order.Price += cart.Price;
            }

            if (productNames == "")
            {
                return BadRequest("Cart is empty");
            }
            order.medicineNames = productNames;
            order.userId = id;
            order.Status = "Test";
            order.Time = DateTime.UtcNow;

            account = await _context.Accounts.FindAsync(id);
            if (account != null)
            {
                account.Funds -= order.Price;
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(order);
        }

        [HttpPut("generateReports")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GenerateReports>> GenerateReports(bool sales, bool stock, string range)
        {
            List<Medicine> medicines = new();
            List<Order> orders = new();
            GenerateReports report = new();

            DateTime today = DateTime.Today;
            if (range == "Weekly")
            {
                today = DateTime.Today.AddDays(-7);
            }

            else if (range == "Monthly")
            {
                today = DateTime.Today.AddMonths(-1);
            }

            else if (range == "Yearly")
            {
                today = DateTime.Today.AddYears(-1);
            }
            else return BadRequest(range);

            if (sales == true)
            {
                orders = await _context.Orders.ToListAsync();
                orders = orders.Where(e => e.Time.Date >= today.Date && e.Time.Date <= DateTime.Today.Date).ToList();

                report.Orders = orders;
                return Ok(report.Orders);
            }

            if (stock == true)
            {
                medicines = await _context.Medicines.ToListAsync();
                report.Medicines = medicines;
                return Ok(report.Medicines);
            }

           return BadRequest("No choices!");

        }
    }
}