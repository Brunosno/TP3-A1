namespace Restaurante.Models;

public class Reservation
{
    public int Id { get; set; }
    public DateTime Date { get; set; }

    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public int TableId { get; set; }
    public Table Table { get; set; }
}