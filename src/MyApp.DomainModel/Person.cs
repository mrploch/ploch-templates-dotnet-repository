using System.ComponentModel.DataAnnotations;
using Ploch.Data.Model;

namespace Ploch.MyApp.DomainModel;

public class Person : IHasId<int>, IHasAuditProperties, IHasCategories<PersonCategory>, IHasTags<PersonTag>
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(128)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(128)]
    public string LastName { get; set; } = null!;

    [MaxLength(256)]
    public string? Email { get; set; }

    [MaxLength(32)]
    public string? PhoneNumber { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = [];

    public virtual ICollection<Note> Notes { get; set; } = [];

    public ICollection<PersonCategory>? Categories { get; set; }

    public ICollection<PersonTag> Tags { get; set; } = [];

    public DateTimeOffset? CreatedTime { get; set; }

    public DateTimeOffset? ModifiedTime { get; set; }

    public DateTimeOffset? AccessedTime { get; set; }

    public string? CreatedBy { get; set; }

    public string? LastModifiedBy { get; set; }

    public string? LastAccessedBy { get; set; }
}
