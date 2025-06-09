using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.DTO;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{

    public class ProductController : BaseController
    {
        private readonly IUnitOfWork work;
        private readonly IMapper mapper;

        public ProductController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
            this.work = work;
            this.mapper = mapper;
        }

        [HttpGet("get-all-Product")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var Product = await work.ProductRepositry
                    .GetAllAsync(x => x.Category, x => x.Photos);
                var result = mapper.Map<List<ProductDTO>>(Product);
                if (result is null)
                {
                    return BadRequest(new ResponseAPI(400));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("get-Product-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var Product = await work.ProductRepositry
                    .GetByIdAsync(id, x => x.Category, x => x.Photos);
                var result = mapper.Map<ProductDTO>(Product);
                if (result is null)
                {
                    return BadRequest(new ResponseAPI(400));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost("add-Product")]
        public async Task<IActionResult> Add(AddProductDTO addProductDTO)
        {
            try
            {
                await work.ProductRepositry.AddAsync(addProductDTO);
                return Ok(new ResponseAPI(200, "Product added successfully"));

            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }
        [HttpPut("update-Product")]
        public async Task<IActionResult> Update(UpdateProductDTO updateProductDTO)
        {
            try
            {
               var result= await work.ProductRepositry.UpdateAsync(updateProductDTO);
                if (!result)
                {
                    return NotFound(new ResponseAPI(404, "Product not found"));
                }
                return Ok(new ResponseAPI(200, "Product updated successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }

        }

        [HttpDelete("delete-Product/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
               var product = await work.ProductRepositry.GetByIdAsync(id,x=>x.Photos,x=>x.Category);
                await work.ProductRepositry.DeleteAsync(product);
                return Ok(new ResponseAPI(200, "Product deleted successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseAPI(400, ex.Message));
            }
        }

    }
}