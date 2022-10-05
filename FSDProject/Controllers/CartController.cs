using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSDProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly DataContext _context;
        public CartController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("getCartByUserId")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<List<Cart>>> GetCartByUserId(int id)
        {
            var cart = await _context.Carts.Include(c => c.Medicine).Where(c => c.UserId == id).ToArrayAsync();
            return Ok(cart.Length == 0 ? 404 : cart);
        }

        [HttpPut("addToCart")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<Cart>> AddToCart(int userId, int medicineId)
        {
            var medicine = await _context.Medicines.FirstOrDefaultAsync(p => p.ID == medicineId);
            var user = await _context.Accounts.FirstOrDefaultAsync(u => u.ID == userId);


            if (medicine.Quantity <= 0)
            {
                return BadRequest("Out of stock.");
            }

            if (medicine == null)
            {
                return BadRequest("Medicine was not found.");
            }

            if (user == null)
            {
                return BadRequest("User was not found.");
            }

            var cart = await _context.Carts.Where(u => u.UserId == userId).FirstOrDefaultAsync(e => e.MedicineId == medicineId);

            if (cart == null)
            {
                var newCart = new Cart
                {
                    UserId = userId,
                    MedicineId = medicineId,
                    Quantity = 1,
                    Price = medicine.Price
                };

                medicine.Quantity--;

                _context.Carts.Add(newCart);

                await _context.SaveChangesAsync();

                return Ok(newCart);
            }
            cart.Price += medicine.Price;
            cart.Quantity += 1;


            medicine.Quantity -= 1;


            await _context.SaveChangesAsync();

            return Ok(cart);
        }

        [HttpPut("removeFromCartByUserId")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<Cart>> RemoveFromCart(int userId, int medicineId)
        {
            var user = await _context.Accounts.FirstOrDefaultAsync(c => c.ID == userId);
            var medicine = await _context.Medicines.FirstOrDefaultAsync(e => e.ID == medicineId);

            if (medicine == null || user == null)
            {
                return BadRequest("Medicine was not found.");
            }

            var cart = await _context.Carts.Where(u => u.UserId == userId).FirstOrDefaultAsync(p => p.MedicineId == medicineId);

            if (cart == null)
            {
                return BadRequest("Nothing to remove.");
            }

            cart.Quantity--;
            cart.Price -= medicine.Price;

            medicine.Quantity++;

            if (cart.Quantity <= 0)
            {
                _context.Carts.Remove(cart);
            }

            await _context.SaveChangesAsync();

            return Ok(cart);
        }
    }
}