using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ploch.MyApp.DomainModel;

namespace Ploch.MyApp.Data.Configurations;

internal sealed class NoteTagConfiguration : IEntityTypeConfiguration<NoteTag>
{
    public void Configure(EntityTypeBuilder<NoteTag> builder)
    {
        builder.HasMany(e => e.Notes)
               .WithMany(e => e.Tags);
    }
}
