using ITE.Bookify.Product;
using MediatR;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ITE.Bookify.Products
{
    public class ProductAppService : ApplicationService, IProductAppService
    {
        private readonly IMediator _mediator;

        public ProductAppService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ProductDto> CreateAsync(CreateProductCommand command)
        {
            return await _mediator.Send(command);
        }

        public async Task<ProductDto> GetAsync(Guid id)
        {
            return await _mediator.Send(new GetProductQuery { Id = id });
        }
    }
}
