using Graduation_project.DTO.Service;
using Graduation_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Graduation_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        //DB
        Graduation_projectContext _context;
        public ServiceController(Graduation_projectContext context)
        {
            _context = context;
        }
        //----------------------------------------------
        [Authorize(Roles = "Craftsman")]
        [HttpGet("View-Services")]
        public async Task<IActionResult> GetServices()
        {
            var services = await _context.Services.Include(c => c.Client).ToListAsync();

            var serviceReadDTOs = services.Select(service => new ServiceReadDTO
            {
                Id = service.Id,
                Title = service.Title,
                Description = service.Description,
                Budget = service.Budget,
                CreatedAt = service.CreatedAt,
                ClientName = service.Client?.Name 
            }).ToList();

            return Ok(serviceReadDTOs); 
        }
        //======================
        //------------------------------------------------
        //get service by id
        [Authorize(Roles = "Craftsman,Client")]
        [HttpGet("View-Service/{id}")]
        public async Task<IActionResult> GetServiceById(int id)
        {
            var service = await _context.Services
                .Include(c => c.Client)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (service == null)
            {
                return NotFound(); 
            }

            var serviceReadDTO = new ServiceReadDTO
            {
                Id = service.Id,
                Title = service.Title,
                Description = service.Description,
                Budget = service.Budget,
                CreatedAt = service.CreatedAt,
                ClientName = service.Client.Name
            };

            return Ok(serviceReadDTO);
        }

        //------------------------------------------------
        [Authorize(Roles = "Client")]
        [HttpPost("Create-Service")]
        public async Task<IActionResult> CreateService(ServiceCreateDTO serviceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = await _context.Clients.FindAsync(serviceDto.ClientId);
            if (client == null)
            {
                return NotFound($"Client with ID {serviceDto.ClientId} was not found.");
            }

            var service = new Service
            {
                Title = serviceDto.Title,
                Description = serviceDto.Description,
                Budget = serviceDto.Budget,
                CreatedAt = DateTime.UtcNow,
                ClientId = serviceDto.ClientId
            };

            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();

            var serviceReadDto = new ServiceReadDTO
            {
                Id = service.Id,
                Title = service.Title,
                Description = service.Description,
                Budget = service.Budget,
                CreatedAt = service.CreatedAt,
                ClientName = client.Name
            };

            return CreatedAtAction(nameof(GetServiceById), new { id = service.Id }, serviceReadDto);
        }
        //================================
        //--------------------

        //Update the service
        [Authorize(Roles = "Client")]
        [HttpPut("Update-Service/{id}")]
        public async Task<IActionResult> UpdateService(int id, ServiceUpdateDTO serviceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var serviceFromDB = await _context.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (serviceFromDB == null)
            {
                return NotFound($"Service with ID {id} was not found.");
            }

            serviceFromDB.Title = serviceDto.Title;
            serviceFromDB.Description = serviceDto.Description;
            serviceFromDB.Budget = serviceDto.Budget;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        //===========================

        //Delete the service
        [Authorize(Roles = "Client")]
        [HttpDelete("Delete-Service/{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services.Include(e=>e.Offers).FirstOrDefaultAsync(x => x.Id == id);

            if (service == null)
            {
                return NotFound($"Service with ID {id} was not found.");
            }

            if (service.Offers.Any()) 
            {
                return BadRequest("Cannot delete the service because it has associated offers.");
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        







    }
}


