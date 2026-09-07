using LuxuryCarRental.Models;

namespace LuxuryCarRental.ViewModels;

public class CarListViewModel
{
    public List<Car> Cars { get; set; } = new();
    public string? SelectedBrandSlug { get; set; }
    public string? SelectedCategorySlug { get; set; }
    public string? SearchTerm { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
}
