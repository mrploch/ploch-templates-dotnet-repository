using System.ComponentModel.DataAnnotations;
using Ploch.Data.Model;

namespace Ploch.MyApp.DomainModel;

public class Note : IHasId<int>, IHasTitle, IHasContents, IHasAuditProperties, IHasCategories<NoteCategory>, IHasTags<NoteTag>
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(256)]
    public string Title { get; set; } = null!;

    public string? Contents { get; set; }

    public int PersonId { get; set; }

    public virtual Person Person { get; set; } = null!;

    public ICollection<NoteCategory>? Categories { get; set; }

    public ICollection<NoteTag> Tags { get; set; } = [];

    public DateTimeOffset? CreatedTime { get; set; }

    public DateTimeOffset? ModifiedTime { get; set; }

    public DateTimeOffset? AccessedTime { get; set; }

    public string? CreatedBy { get; set; }

    public string? LastModifiedBy { get; set; }

    public string? LastAccessedBy { get; set; }
}
