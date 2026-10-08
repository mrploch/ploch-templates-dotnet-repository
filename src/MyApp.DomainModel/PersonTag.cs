using Ploch.Data.Model.CommonTypes;

namespace Ploch.MyApp.DomainModel;

public class PersonTag : Tag
{
    public virtual ICollection<Person>? Persons { get; set; }
}
