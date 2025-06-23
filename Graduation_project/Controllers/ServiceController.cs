using Graduation_project.DTO.Service;
using Graduation_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        [Authorize(Roles = "Craftsman,Client")]
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
                ClientName = service.Client?.Name ,
                ImageUrl = service.ImageUrl,
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
                Title = service.Title,
                Description = service.Description,
                Budget = service.Budget,
                CreatedAt = service.CreatedAt,
                ClientName = service.Client.Name,
                ImageUrl = service.ImageUrl
                
            };

            return Ok(serviceReadDTO);
        }

        //------------------------------------------------
        [Authorize(Roles = "Client")]
        [HttpPost("Create-Service")]
        public async Task<IActionResult> CreateService([FromForm] ServiceCreateDTO serviceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == userId);

            if (client == null)
            {
                return NotFound("المستخدم غير موجود كعميل.");
            }

            string? imageUrl = null;

            // رفع الصورة (لو فيه صورة)
            if (serviceDto.ImageFile != null && serviceDto.ImageFile.Length > 0)
            {
                var folderPath = Path.Combine("wwwroot", "Images");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fileName = $"{Guid.NewGuid()}_{serviceDto.ImageFile.FileName}";
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await serviceDto.ImageFile.CopyToAsync(stream);
                }

                imageUrl = $"/Images/{fileName}";
            }

            var service = new Service
            {
                Title = serviceDto.Title,
                Description = serviceDto.Description,
                Budget = serviceDto.Budget,
                CreatedAt = DateTime.UtcNow,
                ClientId = client.Id,
                ImageUrl = imageUrl
            };

            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();

            var serviceReadDto = new ServiceReadDTO
            {
                Title = service.Title,
                Description = service.Description,
                Budget = service.Budget,
                CreatedAt = service.CreatedAt,
                ImageUrl=service.ImageUrl,
                ClientName = client.Name
            };

            return CreatedAtAction(nameof(GetServiceById), new { id = service.Id }, serviceReadDto);
        }



        //================================
        //--------------------
        [Authorize(Roles = "Client")]
        [HttpPut("Update-Service/{id}")]
        public async Task<IActionResult> UpdateService(int id, [FromForm] ServiceUpdateDTO serviceDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == userId);
            if (client == null)
                return Unauthorized("المستخدم غير موجود كعميل.");

            var service = await _context.Services.FirstOrDefaultAsync(s => s.Id == id && s.ClientId == client.Id);
            if (service == null)
                return NotFound("الخدمة غير موجودة أو لا تملك صلاحية تعديلها.");

            // تعديل البيانات
            service.Title = serviceDto.Title;
            service.Description = serviceDto.Description;
            service.Budget = serviceDto.Budget;

            if (serviceDto.ImageFile != null && serviceDto.ImageFile.Length > 0)
            {
                var folderPath = Path.Combine("wwwroot", "Images");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fileName = $"{Guid.NewGuid()}_{serviceDto.ImageFile.FileName}";
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await serviceDto.ImageFile.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(service.ImageUrl))
                {
                    var oldPath = Path.Combine("wwwroot", service.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                service.ImageUrl = $"/Images/{fileName}";
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "تم تحديث الخدمة بنجاح",
                serviceId = service.Id
            });
        }


        //Delete the service
        [Authorize(Roles = "Client")]
        [HttpDelete("Delete-Service/{id}")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var client = await _context.Clients.FirstOrDefaultAsync(c => c.UserId == userId);
            if (client == null)
            {
                return NotFound("المستخدم غير موجود كعميل.");
            }

            var service = await _context.Services
                .Include(s => s.Offers)
                .FirstOrDefaultAsync(s => s.Id == id && s.ClientId == client.Id);

            if (service == null)
            {
                return NotFound($"الخدمة غير موجودة أو لا تملك صلاحية حذفها.");
            }

            if (service.Offers.Any())
            {
                return BadRequest("لا يمكن حذف الخدمة لأنها تحتوي على عروض.");
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();

            return NoContent();
        }









    }
}


