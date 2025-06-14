using ITE.Bookify.Apartments.CreateApartment;
using ITE.Bookify.Apartments.SearchApartments;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ITE.Bookify.Apartments
{
    public interface IApartmentService : IApplicationService
    {
        Task<IReadOnlyList<SearchApartmentResponse>> SearchApartments(SearchApartmentDto input, CancellationToken cancellationToken = default);
        Task<ApartmentResponse> GetApartment(Guid id, CancellationToken cancellationToken);
        Task<Guid> CreateApartment(ApartmentCreateUpdateDto request, CancellationToken cancellationToken);
        Task<Guid> UpdateApartment(Guid id, ApartmentCreateUpdateDto request, CancellationToken cancellationToken);
    }
}
