using System;
using System.ComponentModel.DataAnnotations;

namespace LuxuryCarRental.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CarId { get; set; }
        public string? UserName { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        [Required]
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
