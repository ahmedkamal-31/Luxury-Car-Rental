using System.ComponentModel.DataAnnotations;

namespace LuxuryCarRental.Models;

public class Brand
{
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [Required, MaxLength(150)]
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? LogoPath { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Car> Cars { get; set; } = new List<Car>();
}
