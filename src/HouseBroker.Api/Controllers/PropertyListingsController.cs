using HouseBroker.Api.Extensions;
using HouseBroker.Api.ViewModels;
using HouseBroker.Api.ViewModels.Requests;
using HouseBroker.Application.Commands;
using HouseBroker.Application.Queries;
using HouseBroker.Shared.Defaults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseBroker.Api.Controllers;

[ApiController]
[Route("api/v1")]
public class PropertyListingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PropertyListingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("property-listings-with-commission/search")]
    [Authorize(Roles = RoleDefaults.Broker)]
    public async Task<ActionResult<IEnumerable<PropertyListingWithCommissionViewModel>>> GetPropertyListingWithCommission(
        [FromBody] GetPropertyListingsRequestViewModel request,
        CancellationToken cancellationToken)
    {
        var query = new GetPropertyListingsWithCommission.Query(
            Guid: request.Guid,
            PropertyType: request.PropertyType,
            PriceFrom: request.PriceFrom,
            PriceTo: request.PriceTo,
            Country: request.Country,
            City: request.City,
            Street: request.Street
        );

        var items = await _mediator.Send(query, cancellationToken);

        var viewModels = items.Select(x => x.ToViewModel());

        return Ok(viewModels);
    }
    
    [HttpPost("public/property-listings/search")]
    public async Task<ActionResult<IEnumerable<PropertyListingViewModel>>> GetPublicPropertyListing(
        [FromBody] GetPropertyListingsRequestViewModel request,
        CancellationToken cancellationToken)
    {
        var query = new GetPublicPropertyListings.Query(
            Guid: request.Guid,
            PropertyType: request.PropertyType,
            PriceFrom: request.PriceFrom,
            PriceTo: request.PriceTo,
            Country: request.Country,
            City: request.City,
            Street: request.Street
        );

        var items = await _mediator.Send(query, cancellationToken);

        var viewModels = items.Select(x => x.ToViewModel());

        return Ok(viewModels);
    }

    [Authorize(Roles = RoleDefaults.Broker)]
    [HttpPost("property-listings")]
    public async Task<ActionResult<Guid>> CreatePropertyListing(
        [FromBody] CreatePropertyListingRequestViewModel request,
        CancellationToken cancellationToken)
    {
        var command = new CreatePropertyListing.Command(
            Name: request.Name,
            CurrencyCode: request.CurrencyCode,
            Price: request.Price,
            PropertyType: request.PropertyType,
            ImageUrls: request.ImageUrls,
            Country: request.Country,
            Street: request.Street,
            City: request.City
        );

        var createdId = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(
            nameof(CreatePropertyListing),
            new { id = createdId },
            createdId);
    }

    [HttpPut("property-listings/{guid}")]
    [Authorize(Roles = RoleDefaults.Broker)]
    public async Task<ActionResult> UpdatePropertyListing(
        Guid guid,
        [FromBody] UpdatePropertyListingRequestViewModel request,
        CancellationToken cancellationToken)
    {
        var command = new UpdatePropertyListing.Command(
            Guid: guid,
            Name: request.Name,
            CurrencyCode: request.CurrencyCode,
            Price: request.Price,
            PropertyType: request.PropertyType,
            ImageUrls: request.ImageUrls,
            Country: request.Country,
            Street: request.Street,
            City: request.City
        );

        await _mediator.Send(command, cancellationToken);

        return Ok();
    }

    [HttpDelete("property-listings/{guid}")]
    [Authorize(Roles = RoleDefaults.Broker)]
    public async Task<ActionResult> RemovePropertyListing(
        Guid guid,
        CancellationToken cancellationToken)
    {
        var command = new RemovePropertyListing.Command(
            Guid: guid
        );

        await _mediator.Send(command, cancellationToken);

        return Ok();
    }
}