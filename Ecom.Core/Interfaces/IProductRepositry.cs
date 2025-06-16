using Ecom.Core.DTO;
using Ecom.Core.Entites.Product;
using Ecom.Core.Sharing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Interfaces
{
    public interface IProductRepositry:IGenericRepositry<Product>
    {
        //for future
        Task<IEnumerable<ProductDTO>> GetAllAsync(ProductParams productParams);
        Task<bool> AddAsync(AddProductDTO addProductDTO);
        Task<bool> UpdateAsync(UpdateProductDTO updateProductDTO);
        Task<bool> DeleteAsync(Product product);

    }
}
