using Microsoft.AspNetCore.Mvc;
using Foodies.Models;
using System.Linq;
using System;

namespace Foodies.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ApplicationDBContext _context;
        public ReservationController(ApplicationDBContext dbContext)
        {
            _context = dbContext;
        }

        [HttpGet]
        public IActionResult ConfirmOrder(int reservationId)
        {
            var reservation = _context.Reservations
                .Where(r => r.Reservation_Id == reservationId)
                .Select(r => new Reservation
                {
                    Customer = r.Customer,
                    Meal = r.Meal,
                    Quantity = r.Quantity,
                    PaymentType = r.PaymentType,
                    Delivery = r.Delivery
                }).FirstOrDefault();

            if (reservation == null)
            {
                return NotFound("Reservation not found.");
            }

            return View(reservation);
        }

        [HttpPost]
        public IActionResult ConfirmOrder(Reservation model)
        {
            if (ModelState.IsValid)
            {
                _context.Reservations.Add(model);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Order confirmed successfully!";
                return RedirectToAction("OrderSummary", new { id = model.Reservation_Id });
            }

            TempData["ErrorMessage"] = "There was an issue confirming the order. Please try again.";
            return View(model);
        }

        public IActionResult CancelOrder()
        {
            TempData["InfoMessage"] = "Order canceled successfully.";
            return RedirectToAction("Index", "Home");
        }
    }
}
