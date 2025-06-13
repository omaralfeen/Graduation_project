using Graduation_project.DTO.Offer;
using Graduation_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Graduation_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        //DB
        Graduation_projectContext _context;
        public OfferController(Graduation_projectContext context)
        {
            _context = context;
        }

        //get offers
        [Authorize(Roles = "Client")]
        [HttpGet("View-Offers/{serviceId}")]
        public async Task<IActionResult> GetOffersForService(int serviceId)
        {
            var service = await _context.Services.FindAsync(serviceId);
            if (service == null)
                return NotFound("Service not found");
            var offers = await _context.Offers.Include(c => c.Craftsman)
                .Where(o => o.ServiceId == serviceId).ToListAsync();
 
            var offerReadDTO = offers.Select(offer => new OfferReadDTO
            {
                Id = offer.Id,
                Price = offer.Price,
                Message = offer.Message,
                CreatedAt = DateTime.Now,
                IsAccepted=offer.IsAccepted,
                CraftsmanName = offer.Craftsman?.Name,

            }).ToList();
            return Ok(offerReadDTO);
        }

        //get offer by id
        [Authorize(Roles = "Craftsman,Client")]

        [HttpGet("GetOfferById/{id}")]
        public async Task<IActionResult> GetOfferById(int id)
        {
            var offer = await _context.Offers
                .Include(o => o.Craftsman).AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);

            if (offer == null)
                return NotFound();

            var offerDto = new OfferReadDTO
            {
                Id = offer.Id,
                Price = offer.Price,
                Message = offer.Message,
                CreatedAt = offer.CreatedAt,
                IsAccepted = offer.IsAccepted,
                CraftsmanName = offer.Craftsman?.Name
            };

            return Ok(offerDto);
        }
        //create offer
        [Authorize(Roles = "Craftsman")]
        [HttpPost("Create-Offer")]
        public async Task<IActionResult> CreateOffer(OfferCreateDTO offerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = await _context.Services.FindAsync(offerDto.ServiceId);
            if (service == null)
                return NotFound($"Service with ID {offerDto.ServiceId} was not found.");

            var craftsman = await _context.Craftsmen.FindAsync(offerDto.CraftsmanId);
            if (craftsman == null)
                return NotFound($"Craftsman with ID {offerDto.CraftsmanId} was not found.");

            var offer = new Offer
            {
                Price = offerDto.Price,
                Message = offerDto.Message,
                CreatedAt = DateTime.UtcNow,
                ServiceId = offerDto.ServiceId,
                CraftsmanId = offerDto.CraftsmanId
            };

            _context.Offers.Add(offer);
            await _context.SaveChangesAsync();

            var offerReadDto = new OfferReadDTO
            {
                Id = offer.Id,
                Price = offer.Price,
                Message = offer.Message,
                CreatedAt = offer.CreatedAt,
                CraftsmanName = craftsman.Name,
                IsAccepted = offer.IsAccepted
            };

            return CreatedAtAction(nameof(GetOfferById), new { id = offer.Id }, offerReadDto);
        }
        // update the service
        [Authorize(Roles = "Craftsman")]
        [HttpPut("Update-Offer/{id}")]
        public async Task<IActionResult> UpdateOffer(int id, [FromBody] OfferUpdateDTO offerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var offerFromDB = await _context.Offers.FindAsync(id);
            if (offerFromDB == null)
                return NotFound($"Offer with ID {id} not found.");

            offerFromDB.Price = offerDto.Price;
            offerFromDB.Message = offerDto.Message;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        // delete offer
        [Authorize(Roles = "Craftsman")]
        [HttpDelete("Delete-Offer/{id}")]
        public async Task<IActionResult> DeleteOffer(int id)
        {
            var offer = await _context.Offers.FindAsync(id);
            if (offer == null)
            {
                return NotFound($"Offer with ID {id} was not found.");
            }

            _context.Offers.Remove(offer);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Offer deleted successfully." });
        }
        /// <summary>
        /// //
        /// </summary>
        [Authorize(Roles = "Client")]
        [HttpPost("Accept-Offer/{offerId}")]
        public async Task<IActionResult> AcceptOffer(int offerId)
        {
            var offer = await _context.Offers.Include(o => o.Service).FirstOrDefaultAsync(o => o.Id == offerId);
            if (offer == null) return NotFound($"Offer with ID {offerId} not found.");

            var relatedOffers = await _context.Offers.Where(o => o.ServiceId == offer.ServiceId).ToListAsync();

            foreach (var o in relatedOffers)
                o.IsAccepted = false;

            offer.IsAccepted = true;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Offer has been accepted successfully." });
        }

    }
}
