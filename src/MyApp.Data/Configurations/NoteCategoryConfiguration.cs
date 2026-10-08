using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ploch.MyApp.DomainModel;

namespace Ploch.MyApp.Data.Configurations;

internal sealed class NoteCategoryConfiguration : IEntityTypeConfiguration<NoteCategory>
{
    public void Configure(EntityTypeBuilder<NoteCategory> builder)
    {
        builder.HasOne(e => e.Parent)
               .WithMany(e => e.Children)
               .IsRequired(false);
    }
}
