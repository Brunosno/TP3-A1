using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Restaurante.Models;
using Restaurante.Services;

[Authorize]
public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly MinioService _minio;

    public ProfileController(UserManager<ApplicationUser> userManager, MinioService minio)
    {
        _userManager = userManager;
        _minio = minio;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);

        if (!string.IsNullOrEmpty(user.ImageUrl))
        {
            user.ImageUrl = await _minio.GetPresignedUrlAsync(user.ImageUrl);
        }

        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> Update(IFormFile image)
    {
        var user = await _userManager.GetUserAsync(User);

        if (image != null && image.Length > 0)
        {
            int imageIndex = 1;

            if (!string.IsNullOrEmpty(user.ImageUrl))
            {
                var name = user.ImageUrl.Split('/').Last();

                var match = System.Text.RegularExpressions.Regex.Match(name, @"img(\d+)");

                if (match.Success)
                {
                    imageIndex = int.Parse(match.Groups[1].Value) + 1;
                }

                await _minio.DeleteAsync(user.ImageUrl);
            }

            user.ImageUrl = await _minio.UploadAsync(
                image,
                "users",
                user.UserName,
                user.Id,
                imageIndex
            );
        }

        await _userManager.UpdateAsync(user);

        return RedirectToAction("Index");
    }
}