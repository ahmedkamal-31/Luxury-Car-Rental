using System.ComponentModel.DataAnnotations;

namespace LuxuryCarRental.ViewModels;

public class RentalInquiryViewModel
{
    public int CarId { get; set; }
    public string CarName { get; set; } = string.Empty;
    public string CarImage { get; set; } = string.Empty;
    public decimal DailyPrice { get; set; }

    [Required(ErrorMessage = "الاسم بالكامل مطلوب")]
    [Display(Name = "الاسم بالكامل")]
    public string CustomerName { get; set; } = string.Empty;

    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
    [Display(Name = "البريد الإلكتروني")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "رقم الهاتف مطلوب")]
    [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
    [Display(Name = "رقم الهاتف / الواتساب")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "تاريخ بداية الحجز مطلوب")]
    [DataType(DataType.Date)]
    [Display(Name = "تاريخ البدء")]
    public DateTime StartDate { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "تاريخ نهاية الحجز مطلوب")]
    [DataType(DataType.Date)]
    [Display(Name = "تاريخ الانتهاء")]
    public DateTime EndDate { get; set; } = DateTime.Today.AddDays(1);

    [Display(Name = "ملاحظات إضافية")]
    public string? Notes { get; set; }
}
