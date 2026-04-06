namespace Restaurante.Models;

public class Address
{
    public int Id { get; set; }
    public string Street { get; set; }
    public int Number { get; set; }
    public string ZipCode { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
}