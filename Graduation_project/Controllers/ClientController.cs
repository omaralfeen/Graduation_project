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
            var clients = await _context.Clients.ToArrayAsync();
            return Ok(clients);
        }
        [HttpGet("Get_ClientByTd/{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
            return Ok(client);
        }
    }
}
