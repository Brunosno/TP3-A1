using Microsoft.AspNetCore.Identity;
namespace Restaurante.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }

    public Perfil Perfil { get; set; }

    public List<Address> Addresses { get; set; }

    public string? ImageUrl { get; set; }
}