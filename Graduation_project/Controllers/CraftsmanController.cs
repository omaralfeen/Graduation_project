using Graduation_project.DTO.Craftsman;
using Graduation_project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Graduation_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CraftsmanController : ControllerBase
    {
        private readonly Graduation_projectContext _context;

        public CraftsmanController(Graduation_projectContext context)
        {
            _context = context;
        }

        // GET: api/Craftsman/Get_Craftsman
        [HttpGet("Get_Craftsman")]
        public async Task<IActionResult> GetCraftsmans()
        {
            var craftsmans = await _context.Craftsmen.ToListAsync();
            var result = craftsmans.Select(c => new DisplayCraftsmanDTO
            {
                Name = c.Name,
                Craft_Type = c.Craft_Type,
                Email = c.User?.Email,
                PhoneNumber = c.User?.PhoneNumber,
                ProfileImage = c.User.ProfileImage,
            }).ToList();

            return Ok(result);
        }

        // GET: api/Craftsman/Get_CraftsmanByTd/5
        [HttpGet("Get_CraftsmanByTd/{id}")]
        public async Task<IActionResult> GetCraftsmanById(int id)
        {
            var c = await _context.Craftsmen.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);
            if (c == null)
                return NotFound($"No craftsman found with ID = {id}");

            var result = new DisplayCraftsmanDTO
            {
                Name = c.Name,
                Craft_Type = c.Craft_Type,
                Email = c.User?.Email,
                PhoneNumber = c.User?.PhoneNumber,
                ProfileImage = c.User.ProfileImage,
            };

            return Ok(result);
        }

        // GET: api/Craftsman/Find_Craftsman/{name}
        [HttpGet("Find_Craftsman/{name}")]
        public async Task<IActionResult> FindCraftsman(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Name is required.");

            var findCraftsman = await _context.Craftsmen
                .Include(c => c.User)
                .Where(c => c.Name.ToLower().Contains(name.ToLower()))
                .ToListAsync();

            if (findCraftsman.Count == 0)
            {
                return NotFound("No craftsman found with the given name.");
            }

            var result = findCraftsman.Select(c => new DisplayCraftsmanDTO
            {
                Name = c.Name,
                Craft_Type = c.Craft_Type,
                Email = c.User?.Email,
                PhoneNumber = c.User?.PhoneNumber,
                ProfileImage = c.User?.ProfileImage,
            }).ToList();

            return Ok(result);
        }
    }

}
