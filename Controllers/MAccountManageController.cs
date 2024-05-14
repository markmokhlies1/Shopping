using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Dtos.User;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MAccountManageController : ControllerBase
    {
        private IMapper _mapper;
        private ISouqlyRepo _repo;
        private UserForManage userToReturn;

        public MAccountManageController(ISouqlyRepo repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetData() 
        {
            var CurrentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value); 
            var CurrentUser = await _repo.GetUser(CurrentUserId);  
            userToReturn = _mapper.Map<UserForManage>(CurrentUser);
            return Ok(userToReturn);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAccountData(UserForManage model)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value); 
            var oldUser = await _repo.GetUser(currentUserId);
            var newUser = _mapper.Map(model, oldUser); 
            await _repo.SaveAll();
            return Ok(model);
        }
    }
}

