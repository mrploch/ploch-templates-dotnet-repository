using System.ComponentModel.DataAnnotations;
using Ploch.Data.Model;

namespace Ploch.MyApp.DomainModel;

public class Address : IHasId<int>
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(256)]
    public string Street1 { get; set; } = null!;

    [MaxLength(256)]
    public string? Street2 { get; set; }

    [Required]
    [MaxLength(128)]
    public string City { get; set; } = null!;

    [MaxLength(128)]
    public string? State { get; set; }

    [Required]
    [MaxLength(20)]
    public string PostalCode { get; set; } = null!;

    [Required]
    [MaxLength(128)]
    public string Country { get; set; } = null!;

    public int PersonId { get; set; }

    public virtual Person Person { get; set; } = null!;
}
