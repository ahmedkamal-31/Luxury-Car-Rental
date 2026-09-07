using System.ComponentModel.DataAnnotations;

namespace LuxuryCarRental.Models;

public class RentalInquiry
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public Car Car { get; set; } = null!;

    [Required, MaxLength(150)]
    public string CustomerName { get; set; } = string.Empty;
    [Required, EmailAddress, MaxLength(150)]
    public string Email { get; set; } = string.Empty;
    [Required, MaxLength(50)]
    public string Phone { get; set; } = string.Empty;
    [MaxLength(50)]
    public string? WhatsApp { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    
    public string? Message { get; set; }
    
    [MaxLength(50)]
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
