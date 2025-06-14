using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ITE.Bookify.Product
{
    public interface IProductAppService : IApplicationService
    {
        Task<ProductDto> CreateAsync(CreateProductCommand command);
        Task<ProductDto> GetAsync(Guid id);
    }
}
