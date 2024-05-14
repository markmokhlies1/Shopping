using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using API.Services;
using API.Dtos.User;

namespace API.Controllers
{
    [Route("[Controller]/[Action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    [AllowAnonymous]
    public class AdminController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IAuthRepo _Authrepo;
        public AdminController(UserManager<User> userManager, RoleManager<Role> roleManager,
        SignInManager<User> signInManager, IConfiguration config,
        IMapper mapper, IAuthRepo Authrepo, ISouqlyRepo repo)
        {
            _userManager = userManager;
            _mapper = mapper;
            _Authrepo = Authrepo;
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserForRegister model)
        {
            if (model == null) { return NotFound(); }
            if (ModelState.IsValid)
            {
                if (await _Authrepo.EmailExists(model.Email))
                    return BadRequest("هذا البريد الاكتروني موجود بالفعل");
                if (await _Authrepo.UserNameExists(model.UserName))
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

