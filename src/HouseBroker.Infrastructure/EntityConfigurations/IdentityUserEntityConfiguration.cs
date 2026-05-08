using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HouseBroker.Infrastructure.EntityConfigurations;

public class IdentityUserEntityConfiguration : IEntityTypeConfiguration<IdentityUser>
{
    public void Configure(EntityTypeBuilder<IdentityUser> builder)
    {
        var user = new IdentityUser
        {
            Id = "c206ab53-aeba-492d-91b5-611eb97bb1f6",
            UserName = "user",
            NormalizedUserName = "USER",
            Email = "user@example.com",
            NormalizedEmail = "USER@EXAMPLE.COM",
            EmailConfirmed = true,
            PasswordHash = "AQAAAAIAAYagAAAAEDW7vkrUrl9MHdeL1AXKqezm0vIrBu1S7dXUwgfWvnTGbks5ZMZwuwLw8vjj4NuhoQ==", // password: User@123
            SecurityStamp = "c206ab53-aeba-492d-91b5-611eb97bb1f6",
            ConcurrencyStamp = "c206ab53-aeba-492d-91b5-611eb97bb1f6",
        };

        builder.HasData(user);
    }
}