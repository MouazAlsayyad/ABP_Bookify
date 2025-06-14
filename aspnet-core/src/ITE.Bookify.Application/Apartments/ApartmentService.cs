using ITE.Bookify.Apartments.CreateApartment;
using ITE.Bookify.Apartments.GetApartment;
using ITE.Bookify.Apartments.SearchApartments;
using ITE.Bookify.Apartments.UpdateApartment;
using ITE.Bookify.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace ITE.Bookify.Apartments
{
    public class ApartmentService(ISender sender) : ApplicationService, IApartmentService
    {
        public async Task<Guid> CreateApartment(ApartmentCreateUpdateDto request, CancellationToken cancellationToken)
        {
            var command = new CreateApartmentCommand(
                Name: new Name(request.Name),
                Description: new Description(request.Description),
                Address: new Address(
                    request.Address.Country,
                    request.Address.State,
                    request.Address.ZipCode,
                    request.Address.City,
                    request.Address.Street
                ),
                Price: new Money(request.PriceAmount, Currency.FromCode(request.Currency)),
                CleaningFee: new Money(request.CleaningFeeAmount, Currency.FromCode(request.Currency)),
                Amenities: request.Amenities
            );

            var result = await sender.Send(command, cancellationToken);

            return result.Value;
        }

        public async Task<ApartmentResponse> GetApartment(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetApartmentQuery(id);

            var result = await sender.Send(query, cancellationToken);

            return result.Value;
        }

        public async Task<IReadOnlyList<SearchApartmentResponse>> SearchApartments(SearchApartmentDto input, CancellationToken cancellationToken = default)
        {
            var query = new SearchApartmentsQuery(input.StartDate, input.EndDate, input.Page, input.PageSize, input.SearchKey);

            var result = await sender.Send(query, cancellationToken);

            return result.Value;
        }

        public async Task<Guid> UpdateApartment(Guid id, ApartmentCreateUpdateDto request, CancellationToken cancellationToken)
        {
            var command = new UpdateApartmentCommand(
                Id: id,
                Name: new Name(request.Name),
                Description: new Description(request.Description),
                Address: new Address(
                    request.Address.Country,
                    request.Address.State,
                    request.Address.ZipCode,
                    request.Address.City,
                    request.Address.Street
                ),
                Price: new Money(request.PriceAmount, Currency.FromCode(request.Currency)),
                CleaningFee: new Money(request.CleaningFeeAmount, Currency.FromCode(request.Currency)),
                Amenities: request.Amenities
            );

            var result = await sender.Send(command, cancellationToken);

            return result.Value;
        }
    }
}
