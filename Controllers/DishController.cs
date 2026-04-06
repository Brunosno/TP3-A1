using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Restaurante.Data;
using Restaurante.Models;
using Restaurante.Services;

[Authorize]
public class DishController : Controller
{
    private readonly AppDbContext _context;

    private readonly MinioService _minio;

    public DishController(AppDbContext context, MinioService minio)
    {
        _context = context;
        _minio = minio;
    }

    public IActionResult Index()
    {
        var dishes = _context.Dishes.ToList();
        return View(dishes);
    }

    [Authorize(Roles = "ADMIN")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Create(Dish dish, IFormFile? image)
    {
        if (!ModelState.IsValid)
            return View(dish);

        _context.Dishes.Add(dish);
        await _context.SaveChangesAsync();

        if (image != null && image.Length > 0)
        {
            dish.ImageUrl = await _minio.UploadAsync(
                image,
                "dishes",
                dish.Name,
                dish.Id.ToString(),
                1
            );

            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Edit(int id, Dish dish, IFormFile? image)
    {
        var existing = await _context.Dishes.FindAsync(id);

        if (existing == null)
            return NotFound();

        existing.Name = dish.Name;
        existing.Description = dish.Description;
        existing.BasePrice = dish.BasePrice;

        if (image != null && image.Length > 0)
        {
            int imageIndex = 1;

            if (!string.IsNullOrEmpty(existing.ImageUrl))
            {
                var name = existing.ImageUrl.Split('/').Last();

                var match = System.Text.RegularExpressions.Regex.Match(name, @"img(\d+)");

                if (match.Success)
                {
                    imageIndex = int.Parse(match.Groups[1].Value) + 1;
                }

                await _minio.DeleteAsync(existing.ImageUrl);
            }

            existing.ImageUrl = await _minio.UploadAsync(
                image,
                "dishes",
                existing.Name,
                existing.Id.ToString(),
                imageIndex
            );
        }

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}