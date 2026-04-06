namespace Restaurante.Models;

public class Table
{
    public int Id { get; set; }
    public int Number { get; set; }
    public int SupportedPeoples { get; set; }

    public List<Reservation> Reservations { get; set; }
}