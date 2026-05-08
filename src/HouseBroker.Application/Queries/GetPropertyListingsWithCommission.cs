using HouseBroker.Application.Dtos;
using HouseBroker.Application.Extensions;
using HouseBroker.Application.Repositories;
using HouseBroker.Application.Services;
using HouseBroker.Application.Specifications;
using HouseBroker.Application.Specifications.Abstracts;
using HouseBroker.Domain;
using HouseBroker.Domain.Enums;
using HouseBroker.Domain.ValueObjects;
using HouseBroker.Shared.Subject;
using Microsoft.Extensions.Logging;

namespace HouseBroker.Application.Queries;

public static class GetPropertyListingsWithCommission
{
    public record Query(
        Guid? Guid,
        PropertyType? PropertyType,
        int? PriceFrom,
        int? PriceTo,
        string? Country,
        string? City,
        string? Street)
        : IRequest<IEnumerable<PropertyListingWithCommissionDto>>;
    
    public record GetPropertyListingsRequest
    {
        public Guid? Guid { get; init; }
    }

    public class Handler : IRequestHandler<Query, IEnumerable<PropertyListingWithCommissionDto>>
    {
        private readonly IPropertyListingReadRepository _repository;
        private readonly ICommissionEngine _commissionEngine;
        private readonly Subject _subject;
        private readonly ILogger<Handler> _logger;

        public Handler(
            IPropertyListingReadRepository repository,
            ICommissionEngine commissionEngine,
            ICommissionWriteRepository commissionWriteRepository,
            Subject subject,
            ILogger<Handler> logger
        )
        {
            _repository = repository;
            _commissionEngine = commissionEngine;
            _subject = subject;
            _logger = logger;
        }

        public async Task<IEnumerable<PropertyListingWithCommissionDto>> Handle(Query query, CancellationToken cancellationToken)
        {
            try
            {
                var specs = new List<ISpecification<PropertyListing>>();
                
                //explicitly filter only current user listings
                specs.Add(ShouldBeCreatedBySpecification.Create(_subject.UserId));

                if (query.Guid != null)
                {
                    specs.Add(ShouldHaveGuidSpecification.Create(query.Guid.Value));
                }

                if (query.PropertyType != null)
                {
                    specs.Add(ShouldBePropertyTypeSpecification.Create(query.PropertyType.Value));
                }

                if (query.PriceFrom != null && query.PriceTo != null)
                {
                    specs.Add(ShouldBeBetweenPriceRangeSpecification.Create(query.PriceFrom.Value,
                        query.PriceTo.Value));
                }

                if (query.Country != null && query.Street != null && query.City != null)
                {
                    var address = PropertyListingAddress.Create(
                        street: query.Street,
                        city: query.City,
                        country: query.Country
                    );

                    specs.Add(ShouldBeInLocationSpecification.Create(address));
                }

                var listings = await _repository.GetAllAsync(specs, cancellationToken);

                var result = new List<PropertyListingWithCommissionDto>();

                foreach (var x in listings)
                {
                    var commission = await _commissionEngine
                        .CalculateCommissionAsync(x.Price, cancellationToken);

                    result.Add(x.ToDto(commission));
                }

                return result;
            }
            catch (Exception e)
            {   
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}