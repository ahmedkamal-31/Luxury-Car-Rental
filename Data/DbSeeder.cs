using LuxuryCarRental.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LuxuryCarRental.Data;

public static class DbSeeder
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync();

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        var adminEmail = "admin@luxurycar.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            var user = new IdentityUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
            var result = await userManager.CreateAsync(user, "AdminPassword123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }

        if (!context.Categories.Any())
        {
            context.Categories.AddRange(
                new Category { Name = "Exotic", Slug = "exotic", Description = "Supercars and hypercars." },
                new Category { Name = "Luxury SUV", Slug = "luxury-suv", Description = "Premium SUVs." }
            );
            await context.SaveChangesAsync();
        }

        if (!context.Brands.Any())
        {
            context.Brands.AddRange(
                new Brand { Name = "Lamborghini", Slug = "lamborghini" },
                new Brand { Name = "Rolls-Royce", Slug = "rolls-royce" }
            );
            await context.SaveChangesAsync();
        }

        if (!context.Cars.Any())
        {
            var lambo = context.Brands.First(b => b.Slug == "lamborghini");
            var exotic = context.Categories.First(c => c.Slug == "exotic");

            var car = new Car
            {
                Name = "Lamborghini Revuelto 2025",
                Slug = "lamborghini-revuelto-2025",
                BrandId = lambo.Id,
                CategoryId = exotic.Id,
                Year = 2025,
                Color = "Orange",
                BodyType = "Coupe",
                Seats = 2,
                Doors = 2,
                Transmission = "Automatic",
                FuelType = "Hybrid",
                Engine = "6.5L V12 + Electric",
                EnginePower = "1001 HP",
                Description = "The ultimate V12 hybrid super sports car. Unmatched performance and luxury in Dubai.",
                DailyPrice = 8500,
                WeeklyPrice = 52000,
                IsAvailable = true,
                IsFeatured = true
            };

            context.Cars.Add(car);
            await context.SaveChangesAsync();

            context.CarImages.Add(new CarImage
            {
                CarId = car.Id,
                ImagePath = "https://images.unsplash.com/photo-1544829099-b9a0c07fad1a?auto=format&fit=crop&w=1200&q=80",
                IsPrimary = true,
                DisplayOrder = 1
            });
            await context.SaveChangesAsync();
        }
    }
}

