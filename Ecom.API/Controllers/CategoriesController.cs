using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    public class CategoriesController : BaseController
    {
        public CategoriesController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var categories = await work.CategoryRepositry.GetAllAsync();
                if (categories == null || !categories.Any())
                    return NotFound(new ResponseAPI(404,"No categories found."));
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var category = await work.CategoryRepositry.GetByIdAsync(id);
                if (category == null)
                    return NotFound(new ResponseAPI(404,$"Category with id {id} not found."));
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] CategoryDTO categoryDTO)
        {
            try
            {
           
               if (categoryDTO == null || string.IsNullOrWhiteSpace(categoryDTO.Name))
                    return BadRequest(new ResponseAPI(400,"Category is null."));
                var category = mapper.Map<Category>(categoryDTO);
                var result = await work.CategoryRepositry.AddAsync(category);
                if (result == null)
                    return BadRequest(new ResponseAPI(400,"Failed to add category."));
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryDTO categoryDTO)
        {
            try
            {
                if (categoryDTO == null)
                    return BadRequest(new ResponseAPI(400,"Category is null."));
                var category = await work.CategoryRepositry.GetByIdAsync(id);
                if (category == null)
                    return NotFound(new ResponseAPI(404,$"Category with id {id} not found."));
                category.Name = categoryDTO.Name;
                category.Description = categoryDTO.Description;
                var result = await work.CategoryRepositry.UpdateAsync(category);
                if (result == null)
                    return BadRequest(new ResponseAPI(400,"Failed to update category."));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await work.CategoryRepositry.GetByIdAsync(id);
                if (category == null)
                    return NotFound(new ResponseAPI(404,$"Category with id {id} not found."));
                var result = await work.CategoryRepositry.DeleteAsync(id);
                if (result == null)
                    return BadRequest(new ResponseAPI(404,"Failed to delete category."));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

    }
}
