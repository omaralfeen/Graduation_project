using Graduation_project.DTO.Client;
using Graduation_project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Graduation_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        Graduation_projectContext _context;
        public ClientController(Graduation_projectContext context)
        {
            _context = context;
        }

        [HttpGet("Get_Clients")]
        public async Task<IActionResult> GetClients()
        {
            var clients = await _context.Clients
                .Include(c => c.User) 
                .ToListAsync();

            var result = clients.Select(c => new DisplayClientDTO
            {
                Id = c.Id,
                Name = c.Name,
                Address = c.Address,
                Email = c.User.Email,
                PhoneNumber = c.User.PhoneNumber,
                ProfileImage=c.User.ProfileImage,
            });

            return Ok(result);
        }

        [HttpGet("Get_ClientById/{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            var client = await _context.Clients
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
                return NotFound("العميل غير موجود.");

            var dto = new DisplayClientDTO
            {
                Id = client.Id,
                Name = client.Name,
                Address = client.Address,
                Email = client.User.Email,
                PhoneNumber = client.User.PhoneNumber,
                ProfileImage = client.User.ProfileImage,
            };

            return Ok(dto);
        }

    }
}
