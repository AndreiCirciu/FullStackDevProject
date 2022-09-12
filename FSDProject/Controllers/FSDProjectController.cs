using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FSDProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FSDProjectController : ControllerBase
    {

        private static List<Medicine> medicines = new List<Medicine>
            {
                new Medicine {
                    Id = 0,
                    Name = "Nurofen"
                },

                new Medicine {
                    Id = 1,
                    Name = "Tusocalm"
                },

                new Medicine {
                    Id = 2,
                    Name = "Zinat"
                }
            };

        [HttpPost("addMedicine")]
        public async Task<ActionResult<List<Medicine>>> AddMedicine(Medicine medicine)
        {
            medicines.Add(medicine);
            return Ok(medicines);
        }

        [HttpPut("updateMedicine")]
        public async Task<ActionResult<List<Medicine>>> UpdateMedicine(Medicine  request)
        {
            var medicine = medicines.Find(m => m.Id == request.Id);
            if (medicine == null)
                return BadRequest("Medicine not found");
            medicine.Name = request.Name;
            return Ok(medicines);
        }

        [HttpGet("getAllMedicine")]
        public async Task<ActionResult<List<Medicine>>> Get()
        {       
            return Ok(medicines);
        }       

        [HttpGet("getMedicineById")]
        public async Task<ActionResult<Medicine>> Get(int id)
        {
            var medicine = medicines.Find(m => m.Id == id);
            if (medicine == null)
                return BadRequest("Medicine not found");
            return Ok(medicine);
        }

        [HttpDelete("deleteMedicineById")]
        public async Task<ActionResult<List<Medicine>>> Delete(int id)
        {
            var medicine = medicines.Find(m => m.Id == id);
            if (medicine == null)
                return BadRequest("Medicine not found");
            medicines.Remove(medicine);
            return Ok(medicines);
        }
    }
}
