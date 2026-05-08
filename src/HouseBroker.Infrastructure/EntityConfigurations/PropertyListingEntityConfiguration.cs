using HouseBroker.Domain;
using HouseBroker.Domain.Contexts.HouseBroker.Commission;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HouseBroker.Infrastructure.EntityConfigurations;

public class PropertyListingEntityConfiguration : IEntityTypeConfiguration<PropertyListing>
{
    public void Configure(EntityTypeBuilder<PropertyListing> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .OwnsOne(x => x.PropertyListingAddress);

        builder
            .Navigation(x => x.PropertyListingAddress)
            .AutoInclude();
    }
}
