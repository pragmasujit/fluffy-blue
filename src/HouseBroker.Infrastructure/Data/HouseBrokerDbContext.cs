using HouseBroker.Domain;
using HouseBroker.Domain.Contexts.HouseBroker.Commission;
using HouseBroker.Infrastructure.EntityConfigurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HouseBroker.Infrastructure.Data;

public class HouseBrokerDbContext: IdentityDbContext
{
    public HouseBrokerDbContext(DbContextOptions<HouseBrokerDbContext> options)
        : base(options)
    {
        
    }

    public DbSet<PropertyListing> PropertyListings { get; set; }
    public DbSet<Commission> Commissions { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .ApplyConfiguration(new IdentityUserEntityConfiguration())
            .ApplyConfiguration(new CommissionEntityConfiguration())
            .ApplyConfiguration(new IdentityRoleConfiguration())
            .ApplyConfiguration(new IdentityUserRoleEntityConfiguration())
            .ApplyConfiguration(new PropertyListingEntityConfiguration());
    }
}