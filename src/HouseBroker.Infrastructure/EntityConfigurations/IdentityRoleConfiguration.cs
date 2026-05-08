using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HouseBroker.Infrastructure.EntityConfigurations;

public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Id = "c206ab53-aeba-492d-91b5-611eb97bb1f6",
                Name = "Admin",
                ConcurrencyStamp = "c206ab53-aeba-492d-91b5-611eb97bb1f6",
                NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Id = "c206ab53-aeba-492d-91b5-68febkkbb1f6",
                Name = "Broker",
                ConcurrencyStamp = "c206ab53-aeba-492d-91b5-611eb97bb1f6",
                NormalizedName = "BROKER"
            }
        );
    }
}