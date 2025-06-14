using ITE.Bookify.Product;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ITE.Bookify.Products
{
    internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            return new ProductDto
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price
            };
        }
    }
}
