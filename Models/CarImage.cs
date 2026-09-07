using System.ComponentModel.DataAnnotations;

namespace LuxuryCarRental.Models;

public class CarImage
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public Car Car { get; set; } = null!;
    
    [Required]
    public string ImagePath { get; set; } = string.Empty;
    public bool IsPrimary { get; set; } = false;
    public int DisplayOrder { get; set; } = 0;
}
