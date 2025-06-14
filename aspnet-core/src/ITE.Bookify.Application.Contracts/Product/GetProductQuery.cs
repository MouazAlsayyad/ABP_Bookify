using MediatR;
using System;

namespace ITE.Bookify.Product
{
    public class GetProductQuery : IRequest<ProductDto>
    {
        public Guid Id { get; set; }
    }
}
