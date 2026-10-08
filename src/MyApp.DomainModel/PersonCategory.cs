using Ploch.Data.Model.CommonTypes;

namespace Ploch.MyApp.DomainModel;

public class PersonCategory : Category<PersonCategory>
{
    public virtual ICollection<Person>? Persons { get; set; }
}
