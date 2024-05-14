using System.Security.Claims;
using API.Dtos.User;
using API.Models;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISouqlyRepo _repo;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public UsersController(ISouqlyRepo repo, IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _repo = repo;
            _userManager = userManager;
        }

        [HttpGet("{id}",Name ="GetUser")]
        public async Task<IActionResult> Getuser(int id)
        {
            var user = await _repo.GetUser(id);
            var userToReturn = _mapper.Map<UserForDetails>(user);
            return Ok(userToReturn);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllUser()
        { 
            var allusers = await _repo.GetAllUsers();
            var data = _mapper.Map<System.Collections.Generic.IEnumerable<UserForManage>>(allusers);
            return Ok(data);
        }

        [HttpGet("GetByRole")]
        public async Task<IActionResult> GetUserByRole()
        {
            string Name = "Supplier";
            var allusers = await _repo.GetAllUsers();
            var rolesForUser = _userManager.GetUsersInRoleAsync(Name).Result;
            return Ok(rolesForUser);
        }

        [HttpGet("GetUserData/{id}")]
        public async Task<IActionResult> Getuserr(int id)
        {
            var user = await _repo.GetUser(id);
            var userToReturn = _mapper.Map<UserForManage>(user);
            return Ok(userToReturn);
        }

        [HttpGet("GetVisaData/{id}")]
        public async Task<IActionResult> GetVisaData(int id)
        { 
            var user = await _repo.GetUser(id);
            var userToReturn = _mapper.Map<UserVisa>(user);
            return Ok(userToReturn);
        }

        [HttpPost("editUser/{id}")]
        public async Task<IActionResult> updateuser(UserForManage model, int id)
        {
            var oldUser = await _repo.GetUser(id);
            var newUser = _mapper.Map(model, oldUser);
            await _repo.SaveAll();
            return Ok(newUser);
        }

        [HttpDelete("deleteUser/{id}")]
        public async Task<IActionResult> deleteUser(int id)
        {
            var user = await _repo.GetUser(id);
            await _repo.Delete(user);
            await _repo.SaveAll();
            return Ok("deleted");
        }

        [HttpGet("profits/{user_id}")]
        public async Task<IActionResult> GetUserProfits(int user_id)
        {
            var result = await _repo.GetUserProfits(user_id);
            return Ok(result);
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> PostWithdrawRequest([FromBody] int money)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = await _repo.AddWithdrawnRequest(userId, money);
            if (result == 0)
                return NotFound();
            return NoContent();
        }

        [HttpPost("paymentdetails")]
        public async Task<IActionResult> paymentdetails(UserVisa model)
        {
            var currentuserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await _repo.GetUser(currentuserId);
            var newUser = _mapper.Map(model, user);
            _repo.Update(newUser);
            await _repo.SaveAll();
            return Ok(newUser);
        }
    }
}