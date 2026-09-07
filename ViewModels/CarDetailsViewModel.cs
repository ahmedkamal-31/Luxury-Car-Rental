using LuxuryCarRental.Models;

namespace LuxuryCarRental.ViewModels;

public class CarDetailsViewModel
{
    public Car Car { get; set; } = null!;
    public List<Car> RelatedCars { get; set; } = new();
    public RentalInquiry BookingRequest { get; set; } = new();
    public string WhatsAppNumber { get; set; } = string.Empty;
}
