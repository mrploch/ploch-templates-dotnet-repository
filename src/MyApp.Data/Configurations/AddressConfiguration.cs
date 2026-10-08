using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ploch.MyApp.DomainModel;

namespace Ploch.MyApp.Data.Configurations;

internal sealed class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasOne(e => e.Person)
               .WithMany(e => e.Addresses)
               .HasForeignKey(e => e.PersonId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
