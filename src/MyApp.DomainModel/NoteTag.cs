using Ploch.Data.Model.CommonTypes;

namespace Ploch.MyApp.DomainModel;

public class NoteTag : Tag
{
    public virtual ICollection<Note>? Notes { get; set; }
}
