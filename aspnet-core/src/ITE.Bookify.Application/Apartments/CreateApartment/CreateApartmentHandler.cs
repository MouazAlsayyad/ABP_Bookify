using ITE.Bookify.Abstractions;
using ITE.Bookify.Apartments.CreateApartment;
using ITE.Bookify.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;


namespace ITE.Bookify.Apartments.CreateApartments
{
    internal class CreateApartmentCommandHandler : ICommandHandler<CreateApartmentCommand, Guid>
    {
        private readonly IRepository<Apartments.Apartment, Guid> _apartmentRepository;

        public CreateApartmentCommandHandler(IRepository<Apartments.Apartment, Guid> apartmentRepository)
        {
            _apartmentRepository = apartmentRepository;
        }

        public async Task<Result<Guid>> Handle(CreateApartmentCommand request, CancellationToken cancellationToken)
        {
            var apartment = Apartments.Apartment.Create(
                request.Name,
                request.Description,
                request.Address,
                request.Price,
                request.CleaningFee,
                request.Amenities
            );

            await _apartmentRepository.InsertAsync(apartment, cancellationToken: cancellationToken);

            return apartment.Id;

        }
    }
}
