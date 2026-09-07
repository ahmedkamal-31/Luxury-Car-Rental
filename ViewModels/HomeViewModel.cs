using LuxuryCarRental.Models;

namespace LuxuryCarRental.ViewModels;

public class HomeViewModel
{
    public List<Car> FeaturedCars { get; set; } = new();
    public List<Category> Categories { get; set; } = new();
    public List<Brand> Brands { get; set; } = new();
}
