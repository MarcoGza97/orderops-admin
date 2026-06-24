using OrderOps.Application.Products.Dtos;
using OrderOps.Application.Products.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application.Products.Interfaces
{
    public interface IProductService
    {
        Task<IReadOnlyList<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
        Task<UpdateProductRequest?> GetForEditAsync(int id);
        Task CreateAsync(CreateProductRequest request);
        Task UpdateAsync(UpdateProductRequest request);
        Task DeleteAsync(int id);
    }
}
