using FluentValidation;
using OrderOps.Application.Customers.Dtos;
using OrderOps.Application.Customers.Interfaces;
using OrderOps.Application.Customers.Requests;
using OrderOps.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderOps.Application.Customers.Services
{
    public sealed class CustomerService(
        ICustomerReadRepository readRepository,
        ICustomerWriteRepository writeRepository,
        IValidator<CreateCustomerRequest> createValidator,
        IValidator<UpdateCustomerRequest> updateValidator) : ICustomerService
    {
        private readonly ICustomerReadRepository _readRepository = readRepository;
        private readonly ICustomerWriteRepository _writeRepository = writeRepository;
        private readonly IValidator<CreateCustomerRequest> _createValidator = createValidator;
        private readonly IValidator<UpdateCustomerRequest> _updateValidator = updateValidator;

        public Task<IReadOnlyList<CustomerDto>> GetAllAsync()
        {
            return _readRepository.GetAllAsync();
        }

        public Task<CustomerDto?> GetByIdAsync(int id)
        {
            return _readRepository.GetByIdAsync(id);
        }

        public async Task<UpdateCustomerRequest?> GetForEditAsync(int id)
        {
            var customer = await _readRepository.GetByIdAsync(id);

            return customer is null
                ? null
                : new UpdateCustomerRequest(customer.Id, customer.Name, customer.Email);
        }

        public async Task CreateAsync(CreateCustomerRequest request)
        {
            await _createValidator.ValidateAndThrowAsync(request);

            var exists = await _writeRepository.ExistsByEmailAsync(request.Email);

            if (exists)
                throw new InvalidOperationException("A customer with the same email already exists.");

            var customer = new Customer
            {
                Name = request.Name.Trim(),
                Email = request.Email.Trim()
            };

            await _writeRepository.AddAsync(customer);
            await _writeRepository.SaveChangesAsync();
        }

        public async Task UpdateAsync(UpdateCustomerRequest request)
        {
            await _updateValidator.ValidateAndThrowAsync(request);

            var customer = await _writeRepository.GetByIdAsync(request.Id) ?? throw new InvalidOperationException("Customer not found.");

            var exists = await _writeRepository.ExistsByEmailAsync(request.Email, request.Id);

            if (exists)
                throw new InvalidOperationException("A customer with the same email already exists.");

            customer.Name = request.Name.Trim();
            customer.Email = request.Email.Trim();

            await _writeRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var customer = await _writeRepository.GetByIdAsync(id) ?? throw new InvalidOperationException("Customer not found.");

            _writeRepository.Delete(customer);
            await _writeRepository.SaveChangesAsync();
        }
    }
}
