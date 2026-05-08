using Api.Shared.Exceptions;
using HouseBroker.Application.Dtos;
using HouseBroker.Application.Extensions;
using HouseBroker.Application.Repositories;
using HouseBroker.Application.UnitOfWork;
using HouseBroker.Domain.Exceptions;
using HouseBroker.Shared.Subject;
using Microsoft.Extensions.Logging;

namespace HouseBroker.Application.Commands;

public static class RemovePropertyListing
{
    public record Command(
        Guid Guid
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
            CancellationToken cancellationToken)
        {
            try
            {
                var listing = await _writeRepository.GetByIdAsync(
                        request.Guid,
                        _subject.UserId, 
                        cancellationToken
                    );
                
                if (listing is null)
                    throw new AppValidationException(
                        message: "Listing not found",
                        identifier: nameof(request.Guid)
                    );

                _writeRepository.Remove(listing);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return listing.ToDto();
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