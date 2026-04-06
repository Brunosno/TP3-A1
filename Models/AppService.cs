namespace Restaurante.Models;

public class AppService : Service
{
    public double CommissionDay { get; set; } = 0.04;
    public double CommissionNight { get; set; } = 0.06;
}