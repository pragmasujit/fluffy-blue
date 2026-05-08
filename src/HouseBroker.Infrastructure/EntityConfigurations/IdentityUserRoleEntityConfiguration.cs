using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HouseBroker.Infrastructure.EntityConfigurations;

public class IdentityUserRoleEntityConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData(new IdentityUserRole<string>
        {
            RoleId = "c206ab53-aeba-492d-91b5-611eb97bb1f6",
            UserId = "c206ab53-aeba-492d-91b5-611eb97bb1f6"
        });
        
        builder.HasData(new IdentityUserRole<string>
        {
            RoleId = "c206ab53-aeba-492d-91b5-68febkkbb1f6",
            UserId = "c206ab53-aeba-492d-91b5-611eb97bb1f6"
        });
    }
}