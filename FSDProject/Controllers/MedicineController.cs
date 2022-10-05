using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSDProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {

        private static List<Medicine> medicines = new List<Medicine>
            {
                new Medicine {
                    ID = 0,
                    Name = "Nurofen",
                    CompanyName = "Pfizer",
                    Price = 12,
                    Quantity = 20,
                    ImageUrl = "http",
                    Uses = "Cold",
                    ExpirationDate = "23.10.2024"
                },

                new Medicine {
                    ID = 1,
                    Name = "Zinat",
                    CompanyName = "MediCan",
                    Price = 37,
                    Quantity = 12,
                    ImageUrl = "http",
                    Uses = "BadCold",
                    ExpirationDate = "23.04.2023"
                },

                new Medicine {
                    ID = 2,
                    Name = "Tusocalm",
                    CompanyName = "Pfizer",
                    Price = 7,
                    Quantity = 220,
                    ImageUrl = "http",
                    Uses = "Cough",
                    ExpirationDate = "12.06.2024"
                }
            };
        private readonly DataContext _context;
        public MedicineController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("addMedicine")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Medicine>>> AddMedicine(Medicine medicine)
        {
            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();

            return Ok(await _context.Medicines.ToListAsync());
        }

        [HttpPut("updateMedicine")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Medicine>>> UpdateMedicine(Medicine request)
        {
            var dbMedicine = await _context.Medicines.FindAsync(request.ID);
            if (dbMedicine == null)
                return BadRequest("Medicine not found.");
            dbMedicine.Name = request.Name;
            dbMedicine.CompanyName = request.CompanyName;
            dbMedicine.ExpirationDate = request.ExpirationDate;
            dbMedicine.ImageUrl = request.ImageUrl;
            dbMedicine.Price = request.Price;
            dbMedicine.Quantity = request.Quantity;
            dbMedicine.Uses = request.Uses;
            dbMedicine.ExpirationDate = request.ExpirationDate;

            await _context.SaveChangesAsync();

            return Ok(await _context.Medicines.ToListAsync());
        }

        [HttpGet("getAllMedicine")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<List<Medicine>>> Get()
        {
            return Ok(await _context.Medicines.ToListAsync());
        }

        [HttpGet("getMedicineByUses")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<Medicine>> GetByUses(string uses)
        {
            var medicine = await _context.Medicines.FirstOrDefaultAsync(e => e.Uses == uses);
            if (medicine == null)
            {
                return BadRequest("There was no medicine found for that use");
            }
            return Ok(medicine);
        }

        [HttpGet("getMedicineById")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<Medicine>> Get(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
                return BadRequest("Medicine not found.");
            return Ok(medicine);
        }

        [HttpDelete("deleteMedicineById")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<Medicine>>> Delete(int id)
        {
            var dbMedicine = await _context.Medicines.FindAsync(id);
            if (dbMedicine == null)
                return BadRequest("Medicine not found.");

            _context.Medicines.Remove(dbMedicine);
            await _context.SaveChangesAsync();

            return Ok(await _context.Medicines.ToListAsync());
        }
    }
}
