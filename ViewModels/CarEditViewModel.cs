using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LuxuryCarRental.ViewModels;

public class CarEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "اسم السيارة مطلوب")]
    [Display(Name = "اسم السيارة")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "اختر براند موجود")]
    public int? BrandId { get; set; }

    [Display(Name = "أو أضف براند جديد")]
    public string? NewBrandName { get; set; }

    [Required(ErrorMessage = "يرجى اختيار الفئة")]
    [Display(Name = "الفئة")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "سنة الصنع مطلوبة")]
    [Range(2000, 2030, ErrorMessage = "يرجى إدخال سنة تصنيع صحيحة")]
    [Display(Name = "سنة الصنع")]
    public int Year { get; set; }

    [Required(ErrorMessage = "اللون مطلوب")]
    [Display(Name = "اللون")]
    public string Color { get; set; } = string.Empty;

    [Display(Name = "نوع الهيكل")]
    public string BodyType { get; set; } = "Coupe";

    [Display(Name = "عدد المقاعد")]
    public int Seats { get; set; } = 2;

    [Display(Name = "عدد الأبواب")]
    public int Doors { get; set; } = 2;

    [Display(Name = "ناقل الحركة")]
    public string Transmission { get; set; } = "Automatic";

    [Display(Name = "نوع الوقود")]
    public string FuelType { get; set; } = "Petrol";

    [Display(Name = "المحرك")]
    public string Engine { get; set; } = string.Empty;

    [Display(Name = "قوة المحرك (حصان)")]
    public string EnginePower { get; set; } = string.Empty;

    [Required(ErrorMessage = "وصف السيارة مطلوب")]
    [Display(Name = "الوصف")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "السعر اليومي مطلوب")]
    [Range(1, 1000000, ErrorMessage = "السعر يجب أن يكون أكبر من 0")]
    [Display(Name = "السعر اليومي (AED)")]
    public decimal DailyPrice { get; set; }

    [Display(Name = "السعر الأسبوعي (اختياري)")]
    public decimal? WeeklyPrice { get; set; }

    [Display(Name = "عرض في الصفحة الرئيسية (Featured)")]
    public bool IsFeatured { get; set; }

    [Display(Name = "صورة جديدة (اختياري للاستبدال)")]
    public IFormFile? ImageFile { get; set; }

    public string? ExistingImagePath { get; set; }
}
