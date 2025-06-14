using ITE.Bookify.Abstractions;
using ITE.Bookify.Apartments.ApartmentErrors;
using ITE.Bookify.Messaging;
using Microsoft.EntityFrameworkCore; // Required for ExecuteUpdateAsync
using System;
using System.Linq; // For IQueryable
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace ITE.Bookify.Apartments.UpdateApartment
{
    internal class UpdateApartmentCommandHandler(IApartmentRepository apartmentRepository) : ICommandHandler<UpdateApartmentCommand, Guid>
    {
        private readonly IApartmentRepository _apartmentRepository = apartmentRepository;

        public async Task<Result<Guid>> Handle(UpdateApartmentCommand request, CancellationToken cancellationToken)
        {
            var rowsAffected = await (await _apartmentRepository.GetQueryableAsync())
                .Where(a => a.Id == request.Id)
                .ExecuteUpdateAsync(setters => setters
                        // For Name (uses HasConversion to its .Value for DB storage)
                        .SetProperty(a => a.Name, request.Name) // Pass the Name object
                                                                // For Description (uses HasConversion to its .Value for DB storage)
                        .SetProperty(a => a.Description, request.Description) // Pass the Description object

                        // Address (Owned Entity Type, direct column mapping for sub-properties)
                        .SetProperty(a => a.Address.Country, request.Address.Country)
                        .SetProperty(a => a.Address.State, request.Address.State)
                        .SetProperty(a => a.Address.ZipCode, request.Address.ZipCode)
                        .SetProperty(a => a.Address.City, request.Address.City)
                        .SetProperty(a => a.Address.Street, request.Address.Street)

                        // Price (Owned Entity Type)
                        .SetProperty(a => a.Price.Amount, request.Price.Amount)
                        // Price.Currency uses HasConversion (currency.Code for DB)
                        .SetProperty(a => a.Price.Currency, request.Price.Currency) // Pass the Currency object/enum

                        // CleaningFee (Owned Entity Type)
                        .SetProperty(a => a.CleaningFee.Amount, request.CleaningFee.Amount)
                        // CleaningFee.Currency uses HasConversion (currency.Code for DB)
                        .SetProperty(a => a.CleaningFee.Currency, request.CleaningFee.Currency) // Pass the Currency object/enum
                    , cancellationToken);


            if (rowsAffected == 0)
            {
                var exists = await (await _apartmentRepository.GetQueryableAsync())
                                      .AnyAsync(a => a.Id == request.Id, cancellationToken);
                if (!exists)
                {
                    throw new ApartmentNotFoundException(typeof(Apartments.Apartment), request.Id);
                }
            }

            var apartmentToUpdateAmenities = await _apartmentRepository.FindAsync(request.Id, cancellationToken: cancellationToken);
            if (apartmentToUpdateAmenities == null)
            {
                throw new ApartmentNotFoundException(typeof(Apartments.Apartment), request.Id);
            }

            apartmentToUpdateAmenities.Amenities.Clear();
            if (request.Amenities != null)
            {
                apartmentToUpdateAmenities.Amenities.AddRange(request.Amenities);
            }
            await _apartmentRepository.UpdateAsync(apartmentToUpdateAmenities, cancellationToken: cancellationToken);


            return apartmentToUpdateAmenities.Id;
        }
    }
}
