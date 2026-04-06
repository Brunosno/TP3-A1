using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Restaurante.Data;
using Restaurante.Models;

[Authorize]
public class ReservationController : Controller
{
    private readonly AppDbContext _context;

    public ReservationController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Reservation reservation)
    {
        if (reservation.Date.Hour < 19 || reservation.Date.Hour > 22)
        {
            ModelState.AddModelError("", "Reservas apenas entre 19h e 22h");
            return View(reservation);
        }

        _context.Reservations.Add(reservation);
        _context.SaveChanges();

        return RedirectToAction("Index", "Home");
    }
}