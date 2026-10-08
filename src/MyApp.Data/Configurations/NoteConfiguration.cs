using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ploch.MyApp.DomainModel;

namespace Ploch.MyApp.Data.Configurations;

internal sealed class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.HasOne(e => e.Person)
               .WithMany(e => e.Notes)
               .HasForeignKey(e => e.PersonId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(e => e.Categories)
               .WithMany(e => e.Notes);

        builder.HasMany(e => e.Tags)
               .WithMany(e => e.Notes);
    }
}
