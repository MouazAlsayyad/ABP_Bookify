using ITE.Bookify.Product;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ITE.Bookify.Products
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
    {
        public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var dummyProduct = new ProductDto
            {
                Id = Guid.Empty,
                Name = "Product A",
                Description = "Product A - Description",
                Price = 19
            };

            return dummyProduct;
        }
    }
}
