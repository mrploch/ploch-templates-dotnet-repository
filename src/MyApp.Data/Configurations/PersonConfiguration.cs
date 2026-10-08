using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ploch.MyApp.DomainModel;

namespace Ploch.MyApp.Data.Configurations;

internal sealed class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasMany(e => e.Addresses)
               .WithOne(e => e.Person)
               .HasForeignKey(e => e.PersonId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Notes)
               .WithOne(e => e.Person)
               .HasForeignKey(e => e.PersonId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Categories)
               .WithMany(e => e.Persons);

        builder.HasMany(e => e.Tags)
               .WithMany(e => e.Persons);
    }
}
