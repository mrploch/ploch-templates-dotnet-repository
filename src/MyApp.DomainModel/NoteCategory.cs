using Ploch.Data.Model.CommonTypes;

namespace Ploch.MyApp.DomainModel;

public class NoteCategory : Category<NoteCategory>
{
    public virtual ICollection<Note>? Notes { get; set; }
}
