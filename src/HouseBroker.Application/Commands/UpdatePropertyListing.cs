using Api.Shared.Exceptions;
using HouseBroker.Application.Dtos;
using HouseBroker.Application.Extensions;
using HouseBroker.Application.Repositories;
using HouseBroker.Application.UnitOfWork;
using HouseBroker.Domain.Enums;
using HouseBroker.Domain.Exceptions;
using HouseBroker.Domain.ValueObjects;
using HouseBroker.Shared.Subject;
using Microsoft.Extensions.Logging;

namespace HouseBroker.Application.Commands;

public static class UpdatePropertyListing
{
    public record Command(
        Guid Guid,
        string Name,
        string CurrencyCode,
        decimal Price,
        PropertyType PropertyType,
        IEnumerable<string> ImageUrls,
        string Country,
        string Street,
        string City
    ) : IRequest<PropertyListingDto>;

    public class Handler : IRequestHandler<Command, PropertyListingDto>
    {
        private readonly IPropertyListingWriteRepository _writeRepository;
        private readonly IHouseBrokerUnitOfWork _unitOfWork;
        private readonly Subject _subject;
        private readonly ILogger<Handler> _logger;

        public Handler(
            IPropertyListingWriteRepository writeRepository,
            IHouseBrokerUnitOfWork unitOfWork,
            Subject subject,
            ILogger<Handler> logger
        )
        {
            _writeRepository = writeRepository;
            _unitOfWork = unitOfWork;
            _subject = subject;
            _logger = logger;
        }

        public async Task<PropertyListingDto> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var listing =
                    (await _writeRepository.GetByIdAsync(
                        request.Guid, 
                        _subject.UserId,
                        cancellationToken));

                if (listing is null)
                    throw new AppValidationException(
                        message: "Listing not found",
                        identifier: nameof(request.Guid)
                    );

                var address = PropertyListingAddress.Create(
                    country: request.Country,
                    street: request.Street,
                    city: request.City
                );


                var updated = listing.Update(
                    name: request.Name,
                    currencyCode: request.CurrencyCode,
                    price: request.Price,
                    propertyType: request.PropertyType,
                    imageUrls: request.ImageUrls,
                    propertyListingAddress: address,
                    updatedBy: _subject.UserId
                );

                _writeRepository.Update(updated);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return updated.ToDto();
            }
            catch (DomainValidationException ex)
            {
                throw new AppValidationException(ex.Identifier, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}