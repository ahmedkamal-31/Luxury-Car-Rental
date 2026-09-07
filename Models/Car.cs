using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LuxuryCarRental.Models;

public class Car
{
    public int Id { get; set; }
    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    [Required, MaxLength(250)]
    public string Slug { get; set; } = string.Empty;

    public int BrandId { get; set; }
    public Brand Brand { get; set; } = null!;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public int Year { get; set; }
    [MaxLength(50)]
    public string Color { get; set; } = string.Empty;
    [MaxLength(50)]
    public string BodyType { get; set; } = string.Empty;
    public int Seats { get; set; }
    public int Doors { get; set; }
    [MaxLength(50)]
    public string Transmission { get; set; } = string.Empty;
    [MaxLength(50)]
    public string FuelType { get; set; } = string.Empty;
    [MaxLength(100)]
    public string Engine { get; set; } = string.Empty;
    [MaxLength(50)]
    public string EnginePower { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal DailyPrice { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal? WeeklyPrice { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal? MonthlyPrice { get; set; }

    public bool IsAvailable { get; set; } = true;
    public bool IsFeatured { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<CarImage> CarImages { get; set; } = new List<CarImage>();
    public ICollection<RentalInquiry> RentalInquiries { get; set; } = new List<RentalInquiry>();
}
