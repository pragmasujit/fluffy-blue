using HouseBroker.Domain.Contexts.HouseBroker.Commission;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HouseBroker.Infrastructure.EntityConfigurations;

public class CommissionEntityConfiguration : IEntityTypeConfiguration<Commission>
{
    public void Configure(EntityTypeBuilder<Commission> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.MinPrice).IsRequired();
        builder.Property(x => x.MaxPrice);
        builder.Property(x => x.Percentage).IsRequired();

        builder.HasData(
            Commission.Hydrate(1, 0, 5_000_000, 0.02m),
            Commission.Hydrate(2, 5_000_001, 10_000_000, 0.0175m),
            Commission.Hydrate(3, 10_000_001, null, 0.015m)
        );
    }
}