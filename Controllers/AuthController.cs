using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using API.Dtos.User;
using API.Services;

namespace API.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IAuthRepo _repo;
        private readonly ISouqlyRepo _souqlyRepo;

        public AuthController(UserManager<User> userManager, RoleManager<Role> roleManager,SignInManager<User> signInManager, IConfiguration config,
	    IMapper mapper, IAuthRepo repo, ISouqlyRepo souqlyRepo)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _config = config;
            _mapper = mapper;
            _repo = repo;
            _souqlyRepo = souqlyRepo;
        }

        private async Task CreateRoles()
        {
            if (_roleManager.Roles.Count() < 1)
            {
                var role = new Role
                {
                    Name = "Admin"
                };

                await _roleManager.CreateAsync(role);

                role = new Role
                {
                    Name = "Marketing"
                };

                await _roleManager.CreateAsync(role);

                role = new Role
                {
                    Name = "Supplier"
                };

                await _roleManager.CreateAsync(role);

                role = new Role
                {
                    Name = "Shipping"
                };

                await _roleManager.CreateAsync(role);
            }
        }

        private async Task CreateAdmin()
        {
            var admin = await _userManager.FindByNameAsync("Admin");

            if (admin == null)
            {
                var newUser = new User
                {
                    Email = "admin@admin.com",
                    UserName = "Admin",
                    PhoneNumber = "0796544854",
                    EmailConfirmed = true
                };

                var createAdmin = await _userManager.CreateAsync(newUser, "1234");

                if (createAdmin.Succeeded)
                {
                    if (await _roleManager.RoleExistsAsync("Admin"))
                        await _userManager.AddToRoleAsync(newUser, "Admin");
                }

                var newShippCompany = new ShippingCompany
                {
                    Id = 1,
                    companyName = "Not Attached",
                    companyPhone = null
                };

                await _souqlyRepo.Add(newShippCompany);
                await _souqlyRepo.SaveAll();
            }
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            // IdentityModelEventSource.ShowPII = true;
            var claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLogin userForLogin)
        {
            await CreateRoles();
            await CreateAdmin();

            if (Response.StatusCode == 400)
            {
                return BadRequest("يجب إدخال كامل بيناتك");
            }

            var user = await _userManager.FindByNameAsync(userForLogin.UserName);

            if (user == null)
                return NotFound("البريدد الاكتروني او الاسم غير صحيح");

            var result = await _signInManager.CheckPasswordSignInAsync(user, userForLogin.Password, false);

            if (result.Succeeded)
            {
                var appUser = await _userManager.Users.FirstOrDefaultAsync(
                    u => u.NormalizedUserName == userForLogin.UserName.ToUpper()
                );

                var userToReturn = _mapper.Map<UserForDetails>(appUser);

                return Ok(new
                {
                    token = GenerateJwtToken(appUser).Result,
                    user = userToReturn
                });
            }
            return Unauthorized();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserForRegister model)
        {
            await CreateRoles();

            if (model == null) { return NotFound(); }

            if (ModelState.IsValid)
            {
                if (await _repo.EmailExists(model.Email))
                    return BadRequest("هذا البريد الاكتروني موجود بالفعل");
                if (await _repo.UserNameExists(model.UserName))
                    return BadRequest("هذا الاسم موجود بالفعل");
            }

            var userToCreate = _mapper.Map<User>(model);
            var result = await _userManager.CreateAsync(userToCreate, model.Password);
            var result2 = await _userManager.AddToRoleAsync(userToCreate, model.RoleName);
            var userToReturn = _mapper.Map<UserForDetails>(userToCreate);

            if (result.Succeeded)
            {
                return CreatedAtRoute("GetUser", new { controller = "Users", id = userToCreate.Id }, userToReturn);
            }

            return BadRequest(result.Errors);
        }
    }
}

