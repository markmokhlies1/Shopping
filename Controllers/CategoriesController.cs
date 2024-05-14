
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api")]
    [ApiController]
    [AllowAnonymous]
    public class CategoriesController : ControllerBase
    {
        private readonly ISouqlyRepo _service;
        private readonly IMapper _mapper;

        public CategoriesController(ISouqlyRepo service, IMapper mapper, ISouqlyRepo IsouqlyRepo)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("getallcategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categoriess = await _service.GetAllCategories();
            return Ok(categoriess);
        }
    }
}

