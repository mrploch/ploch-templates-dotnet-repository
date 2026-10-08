using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ploch.MyApp.DomainModel;

namespace Ploch.MyApp.Data.Configurations;

internal sealed class PersonCategoryConfiguration : IEntityTypeConfiguration<PersonCategory>
{
    public void Configure(EntityTypeBuilder<PersonCategory> builder)
    {
        builder.HasOne(e => e.Parent)
               .WithMany(e => e.Children)
               .IsRequired(false);
    }
}
