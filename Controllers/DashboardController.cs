using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[Controller]/[Action]")]
    [ApiController]
    [AllowAnonymous]
    public class DashboardController : ControllerBase
    {
        private readonly ISouqlyRepo _repo;
        private readonly IAdminRepo _adminRepo;
        private readonly IMapper _mapper;

        public DashboardController(ISouqlyRepo repo, IMapper mapper, IAdminRepo adminRepo)
        {
            _repo = repo;
            _mapper = mapper;
            _adminRepo = adminRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetCounts()
        {
            var Counts = await _repo.GetCounts();
            return Ok(Counts);
        }

        [HttpGet]
        public async Task<IActionResult> GetWithdrawRequests()
        {
            var result = await _adminRepo.GetWithdrawnRequests();
            return Ok(result); 
        }

        [HttpPut]
        public async Task<IActionResult> ConfirmWithdrawRequest([FromBody] int reqId)
        {
            var result = await _adminRepo.ConfirmWithdrawnRequests(reqId);
            if (result == 0)
                return NotFound();
            return NoContent();
        }

        [HttpPut]
        public async Task<IActionResult> CancelConfirmWithdrawRequest([FromBody] int reqId)
        {
            var result = await _adminRepo.CancelConfirmWithdrawnRequest(reqId);
            if (result == 0)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{reqId}")]
        public async Task<IActionResult> RefuseWithdrawRequest(int reqId)
        {
            var result = await _adminRepo.RejectWithdrawnRequest(reqId);
            if (result == 0)
                return NotFound();
            return NoContent();
        }
    }
}

