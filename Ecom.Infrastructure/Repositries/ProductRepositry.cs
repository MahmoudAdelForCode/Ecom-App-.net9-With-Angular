using AutoMapper;
using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositries
{
    public class ProductRepositry : GenericRepositry<Product>, IProductRepositry
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;
        private readonly IImageManagementService imageManagementService;

        public ProductRepositry(AppDbContext context, IMapper mapper, IImageManagementService imageManagementService) : base(context)
        {
            this.context = context;
            this.mapper = mapper;
            this.imageManagementService = imageManagementService;
        }

        public async Task<bool> AddAsync(AddProductDTO addProductDTO)
        {
            if (addProductDTO == null) return false;
            var Product = mapper.Map<Product>(addProductDTO);
            await context.Products.AddAsync(Product);
            await context.SaveChangesAsync();

            var ImagePath = await imageManagementService.AddImagesAsync(addProductDTO.Photo, addProductDTO.Name);
            var photo = ImagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = Product.Id
            }).ToList();
            await context.Photos.AddRangeAsync(photo);
            await context.SaveChangesAsync();
            return true;



        }

        public async Task<bool> UpdateAsync(UpdateProductDTO updateProductDTO)
        {
            if (updateProductDTO == null)
            {
                return false;
            }
            var FindProduct = await context.Products.Include(x => x.Photos).Include(x => x.Category)
                  .FirstOrDefaultAsync(x => x.Id == updateProductDTO.Id);
            if (FindProduct is null)
            {
                return false;
            }
            mapper.Map(updateProductDTO, FindProduct);
            var FindPhotos = await context.Photos.Where(x => x.ProductId == updateProductDTO.Id).ToListAsync();
            foreach (var photo in FindPhotos)
            {
                imageManagementService.DeleteImageAsync(photo.ImageName);
            }
            context.Photos.RemoveRange(FindPhotos);
            var ImagePath = await imageManagementService.AddImagesAsync(updateProductDTO.Photo, updateProductDTO.Name);
            var photos = ImagePath.Select(path => new Photo
            {
                ImageName = path,
                ProductId = FindProduct.Id
            }).ToList();
            await context.Photos.AddRangeAsync(photos);
            await context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> DeleteAsync(Product product)
        {
            var photo=await context.Photos.Where(x => x.ProductId == product.Id).ToListAsync();
            foreach (var item in photo)
            {
                 imageManagementService.DeleteImageAsync(item.ImageName);
            }
            context.Products.Remove(product);
            await context.SaveChangesAsync();
            return true;
        }

    }
}
