using Ploch.Data.Model.CommonTypes;

namespace Ploch.MyApp.DomainModel;

/// <summary>
/// A category for notes.
/// </summary>
public class NoteCategory : Category<NoteCategory>
{
    /// <summary>
    /// The notes in this category.
    /// </summary>
    public virtual ICollection<Note>? Notes { get; set; }
}
