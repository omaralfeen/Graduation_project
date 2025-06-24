using Graduation_project.DTO.Authentication;
using Graduation_project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Graduation_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region inject
        private UserManager<ApplicationUser> userManager;
        private Graduation_projectContext _context;
        private RoleManager<IdentityRole> roleManager;
        private IConfiguration config;
        public AccountController(
            UserManager<ApplicationUser> UserManager,
            Graduation_projectContext context,
             RoleManager<IdentityRole> RoleManager,
             IConfiguration Config)
        {
            userManager = UserManager;
            _context = context;
            roleManager = RoleManager;
            config = Config;
        }
        #endregion
        //--------------------------------------------------------------
        #region Register => Craftsman

        [HttpPost("Register_Craftsman")]
        public async Task<IActionResult> Register([FromForm] RegisterCraftsmanDTO dtoFromRequst)
        {
            if (ModelState.IsValid)
            {
                var Na_no = await _context.Craftsmen
                    .FirstOrDefaultAsync(n => n.National_No == dtoFromRequst.National_No);
                if (Na_no != null)
                {
                    return BadRequest("الرقم القومي مستخدم من قبل");
                }
                var existingUser = await userManager.FindByEmailAsync(dtoFromRequst.Email);
                if (existingUser != null)
                {
                    return BadRequest("هذا البريد الإلكتروني مستخدم بالفعل.");
                }

                var user = new ApplicationUser()
                {
                    UserName = dtoFromRequst.UserName,
                    Email = dtoFromRequst.Email,
                    PhoneNumber = dtoFromRequst.PhoneNumber,
                };
                IdentityResult result = await userManager.CreateAsync(user, dtoFromRequst.Password);

                if (result.Succeeded)
                {
                    var roleResult = await userManager.AddToRoleAsync(user, "Craftsman");
                    if (!roleResult.Succeeded)
                    {
                        await userManager.DeleteAsync(user);
                        return BadRequest(new { Errors = roleResult.Errors.Select(e => e.Description) });
                    }
                    var craftsman = new Craftsman()
                    {
                        Name = dtoFromRequst.UserName,
                        Craft_Type = dtoFromRequst.Craft_Type,
                        National_No = dtoFromRequst.National_No,
                        UserId = user.Id
                    };
                    await _context.Craftsmen.AddAsync(craftsman);
                    await _context.SaveChangesAsync();
                    return Ok("Register Craftsman Created");

                }
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new { Errors = errors });
            }
            var modelStateErrors = ModelState.Values.SelectMany(v => v.Errors)
                                                    .Select(e => e.ErrorMessage)
                                                    .ToList();
            return BadRequest(new { Errors = modelStateErrors });
        }
        #endregion
        //--------------------------------------------------------------
        #region Register => Client

        [HttpPost("Register_Client")]
        public async Task<IActionResult> Register([FromForm]RegisterClientDTO dtoFromRequst)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await userManager.FindByEmailAsync(dtoFromRequst.Email);
                if (existingUser != null)
                {
                    return BadRequest("هذا البريد الإلكتروني مستخدم بالفعل.");
                }
                var user = new ApplicationUser
                {
                    UserName = dtoFromRequst.UserName,
                    Email = dtoFromRequst.Email,
                    PhoneNumber = dtoFromRequst.PhoneNumber
                };

                IdentityResult result = await userManager.CreateAsync(user, dtoFromRequst.Password);

                if (result.Succeeded)
                {
                    var roleResult = await userManager.AddToRoleAsync(user, "Client");
                    if (!roleResult.Succeeded)
                    {
                        await userManager.DeleteAsync(user);
                        return BadRequest(new { Errors = roleResult.Errors.Select(e => e.Description) });
                    }
                    var client = new Client
                    {
                        Name = dtoFromRequst.UserName,
                        Address = dtoFromRequst.Address,
                        UserId = user.Id
                    };
                    await _context.Clients.AddAsync(client);
                    await _context.SaveChangesAsync();
                    return Ok("Register Client Created");
                }


                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new { Errors = errors });
            }

            var modelStateErrors = ModelState.Values.SelectMany(v => v.Errors)
                                                     .Select(e => e.ErrorMessage)
                                                     .ToList();
            return BadRequest(new { Errors = modelStateErrors });
        }
        #endregion
        //--------------------------------------------------------------
        #region Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginDTO dto)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser? user = await userManager.FindByEmailAsync(dto.Email);

                if (user == null)
                {
                    ModelState.AddModelError("", "email is not valid");
                }
                else
                {
                    if (await userManager.CheckPasswordAsync(user, dto.Password))
                    {
                        //--
                        var cliams = new List<Claim>();
                        cliams.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        cliams.Add(new Claim(ClaimTypes.Name, user.UserName));
                        cliams.Add(new Claim(ClaimTypes.Email, user.Email));
                        cliams.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        var roles = await userManager.GetRolesAsync(user);
                        foreach (var item in roles)
                        {
                            cliams.Add(new Claim(ClaimTypes.Role, item.ToString()));
                        }
                        //--
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecretKey"]));
                        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        //--
                        var token = new JwtSecurityToken
                            (
                            claims: cliams,
                            issuer: config["JWT:Issuer"],
                            audience: config["JWT:Audience"],
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: signingCredentials
                            );
                        var _token = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                        };
                        return Ok(_token);
                    }
                    else
                    {
                        return Unauthorized();
                    }
                }

            }
            return BadRequest(ModelState);
        }
        #endregion
        //--------------------------------------------------------------
        #region Create Roles
        //[HttpPost("CreateRoles")]
        //public async Task<IActionResult> CreateRoles()
        //{
        //    string[] roles = new[] { "Admin", "Craftsman", "Client" };

        //    foreach (var role in roles)
        //    {
        //        if (!await roleManager.RoleExistsAsync(role))
        //        {
        //            await roleManager.CreateAsync(new IdentityRole(role));
        //        }
        //    }

        //    return Ok("Roles Created Successfully");
        //}
        #endregion
        //-------------------------------------------------------------
        #region Image
        // upload image
        [Authorize]
        [HttpPost("Upload_ProfileImage")]
        public async Task<IActionResult> UploadProfileImage(IFormFile imageFile)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound("المستخدم غير موجود.");

            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("يرجى اختيار صورة صحيحة");

            var folderPath = Path.Combine("wwwroot", "Images");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            user.ProfileImage= $"/Images/{fileName}";
            await userManager.UpdateAsync(user);

            return Ok(new { imageUrl = user.ProfileImage });
        }
        //----------------------------------------------
        [Authorize]
        //update image
        [HttpPost("Update_ProfileImage")]
        public async Task<IActionResult> UpdateProfileImage(IFormFile imageFile)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound("المستخدم غير موجود.");

            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("يرجى اختيار صورة صحيحة");

            var folderPath = Path.Combine("wwwroot", "Images");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            if (!string.IsNullOrEmpty(user.ProfileImage))
            {
                var oldImagePath = Path.Combine("wwwroot", user.ProfileImage.TrimStart('/'));

                if (System.IO.File.Exists(oldImagePath))
                    System.IO.File.Delete(oldImagePath);
            }

            var fileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            user.ProfileImage = $"/Images/{fileName}";
            await userManager.UpdateAsync(user);

            return Ok(new { imageUrl = user.ProfileImage });
        }


        #endregion

    }
}
