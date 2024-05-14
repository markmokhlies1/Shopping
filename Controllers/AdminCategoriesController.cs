using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos.Admin;
using API.Models;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class AdminCategoriesController : ControllerBase
    {
        private IMapper _mapper;
        private ISouqlyRepo _repo;
        public AdminCategoriesController(ISouqlyRepo repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var allcategories = await _repo.GetallCategories();
            var categoriesReturn = _mapper.Map<IEnumerable<ManageCategoriesDto>>(allcategories);
            return Ok(categoriesReturn);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(ManageCategoriesDto newcat)
        {
            var catToCreate = _mapper.Map<Category>(newcat);
            var catToretun = _repo.Add(catToCreate);
            await _repo.SaveAll();
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var SelectedCat = _repo.GetCatById(id);
            var categoriesReturn = _mapper.Map<ManageCategoriesDto>(SelectedCat);
            return Ok(categoriesReturn);
        }

        [HttpPut("{Id}/{updatedCatName}")]
        public async Task<IActionResult> UpdateCategoryName(int Id, string updatedCatName)
        {
            await _repo.UpdateCategory(Id, updatedCatName);
            return Ok();
        }
    }
}

