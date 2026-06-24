using FluentValidation;
using OrderOps.Application.Products.Dtos;
using OrderOps.Application.Products.Interfaces;
using OrderOps.Application.Products.Request;
using OrderOps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application.Products.Services
{
    public sealed class ProductService(IProductReadRepository readRepository,
        IProductWriteRepository writeRepository,
        IValidator<CreateProductRequest> createValidator,
        IValidator<UpdateProductRequest> updateValidator) : IProductService
    {
        private readonly IProductReadRepository _readRepository = readRepository;
        private readonly IProductWriteRepository _writeRepository = writeRepository;
        private readonly IValidator<CreateProductRequest> _createValidator = createValidator;
        private readonly IValidator<UpdateProductRequest> _updateValidator = updateValidator;

        public Task<IReadOnlyList<ProductDto>> GetAllAsync()
        {
            return _readRepository.GetAllAsync();
        }

        public Task<ProductDto?> GetByIdAsync(int id)
        {
            return _readRepository.GetByIdAsync(id);
        }

        public async Task<UpdateProductRequest?> GetForEditAsync(int id)
        {
            var product = await _readRepository.GetByIdAsync(id);

            if (product is null)
                return null;

            return new UpdateProductRequest(
                product.Id,
                product.Name,
                product.Price,
                product.Stock);
        }

        public async Task CreateAsync(CreateProductRequest request)
        {
            await _createValidator.ValidateAndThrowAsync(request);

            var exists = await _writeRepository.ExistsByNameAsync(request.Name);

            if (exists)
                throw new InvalidOperationException("A product with the same name already exists.");


            var product = new Product
            {
                Name = request.Name.Trim(),
                Price = request.Price,
                Stock = request.Stock
            };

            await _writeRepository.AddAsync(product);
            await _writeRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateProductRequest request)
        {
            await _updateValidator.ValidateAndThrowAsync(request);

            var product = await _writeRepository.GetByIdAsync(request.Id) ?? throw new InvalidOperationException("Product not found.");
            
            var exists = await _writeRepository.ExistsByNameAsync(
                request.Name,
                request.Id);

            if (exists)
                throw new InvalidOperationException("A product with the same name already exists.");


            product.Name = request.Name.Trim();
            product.Price = request.Price;
            product.Stock = request.Stock;

            await _writeRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _writeRepository.GetByIdAsync(id) ?? throw new InvalidOperationException("Product not found.");
            
            _writeRepository.Delete(product);
            await _writeRepository.SaveChangesAsync();
        }
    }
}
