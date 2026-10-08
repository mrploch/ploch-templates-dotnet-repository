using Microsoft.EntityFrameworkCore;
using Ploch.Data.Model;
using Ploch.MyApp.DomainModel;

namespace Ploch.MyApp.Data;

public class MyAppDbContext : DbContext
{
    public MyAppDbContext(DbContextOptions<MyAppDbContext> options) : base(options)
    { }

    protected MyAppDbContext()
    { }

    public DbSet<Person> Persons { get; set; }

    public DbSet<Address> Addresses { get; set; }

    public DbSet<Note> Notes { get; set; }

    public DbSet<PersonCategory> PersonCategories { get; set; }

    public DbSet<PersonTag> PersonTags { get; set; }

    public DbSet<NoteCategory> NoteCategories { get; set; }

    public DbSet<NoteTag> NoteTags { get; set; }

    public override int SaveChanges()
    {
        SetAuditTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAuditTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyAppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    private void SetAuditTimestamps()
    {
        var now = DateTimeOffset.UtcNow;
        foreach (var entry in ChangeTracker.Entries<IHasAuditTimeProperties>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedTime = now;
                    entry.Entity.ModifiedTime = now;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedTime = now;
                    break;
            }
        }
    }
}
