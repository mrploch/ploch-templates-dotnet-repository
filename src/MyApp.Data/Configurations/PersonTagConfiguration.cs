using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ploch.MyApp.DomainModel;

namespace Ploch.MyApp.Data.Configurations;

internal sealed class PersonTagConfiguration : IEntityTypeConfiguration<PersonTag>
{
    public void Configure(EntityTypeBuilder<PersonTag> builder)
    {
        builder.HasMany(e => e.Persons)
               .WithMany(e => e.Tags);
    }
}
