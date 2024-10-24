using Foodies.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Foodies.Controllers
{
    //[Route("Customer/[action]/{id?}")]
    //[ApiController]
    public class CustomerController : Controller
	{
        private readonly ApplicationDBContext _DBContext;
        public CustomerController(ApplicationDBContext dbContext)
        {
            _DBContext = dbContext;
        }
        public IActionResult Index(int id)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier); // Retrieve user ID claim
            var userNameClaim = User.FindFirstValue(ClaimTypes.Name); // Retrieve user name claim
            ViewBag.UserName = userNameClaim;
            var user = _DBContext.Customers.FirstOrDefault(x => x.Id == id);
            //ViewData["UserName"] = user.Username;
            if (user == null)
            {
                return NotFound(); // Handle case where user is not found
            }

            var locationGroups = _DBContext.Restaurants
                .GroupBy(r => r.Location)
                .ToDictionary(g => g.Key, g => g.ToList());

            ViewBag.LocationGroups = locationGroups; // Pass locationGroups to ViewBag

            return View(user);
        }

        public IActionResult SeeMenue(int id)
        {
            var restaurant = _DBContext.Restaurants.FirstOrDefault(x => x.Id == id);
            if (restaurant == null)
            {
              
                return NotFound();
            }

            var menu = _DBContext.Menus.FirstOrDefault(x => x.Resturant_Id == restaurant.Id);
            if (menu == null)
            {
               
                return NotFound("Menu not found for this restaurant");
            }

       
            var menuItems = _DBContext.Meals.Where(m => m.Menu_Id == menu.Id).ToList();

        
            return View(menuItems);
        }

        public IActionResult MakeOrder(int id)
        {
            var meal = _DBContext.Meals.FirstOrDefault(m => m.Id == id);
            if (meal == null)
            {
                
                return NotFound();
            }
            TempData["MealID"] = meal.Id;
            return View(meal);
        }
        // Make sure to import your Reservation model


        /*public IActionResult ConfirmOrder(int? id)
        {
            if (id == null)
            {
                return NotFound("Reservation ID is missing.");
            }

            var reservation = _DBContext.Reservations
                .Include(r => r.Meal)
                .Include(r => r.Customer)
                .FirstOrDefault(r => r.Reservation_Id == id);

            if (reservation == null)
            {
                return NotFound("Reservation not found.");
            }

            return View(reservation); // Ensure you have a ConfirmOrder.cshtml view in Views/Customer/
        }



        [HttpPost]
        public IActionResult ConfirmOrder(int id, Reservation reservation, bool Delivery)
        {
            var mealId = TempData["MealID"] as int? ?? 0;

            if (mealId == 0)
            {
                return BadRequest("Meal ID is missing."); // Handle case where MealID is not found in TempData
            }

            // Eager loading the Menu along with the Meal
            var meal = _DBContext.Meals
                .Include(m => m.Menu) // Ensure that Menu is loaded
                .FirstOrDefault(x => x.Id == mealId);

            if (meal == null)
            {
                return BadRequest("Meal not found."); // Handle case where meal is not found
            }

            // Check if reservation is null
            if (reservation == null)
            {
                return BadRequest("Reservation data is missing.");
            }

            // Assign meal to reservation
            reservation.Meal = meal;
            reservation.Delivery = Delivery;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(); // Handle unauthorized access
            }

            reservation.Customer_Id = int.Parse(userId);
            reservation.Meal_Id = mealId;
            ModelState.Remove("Customer");
            ModelState.Remove("Meal");

            if (ModelState.IsValid)
            {
                try
                {
                    var newReservation = new Reservation
                    {
                        Customer_Id = reservation.Customer_Id,
                        Meal_Id = reservation.Meal_Id,
                        PaymentType = reservation.PaymentType,
                        Quantity = reservation.Quantity,
                        Delivery = reservation.Delivery
                    };

                    _DBContext.Reservations.Add(newReservation);
                    _DBContext.SaveChanges();

                    // Check if Meal and Menu are valid before redirecting
                    if (meal.Menu != null)
                    {
                        return RedirectToAction("SeeMenue", new { id = meal.Menu.Resturant_Id });
                    }
                    else
                    {
                        return NotFound("Menu not found for the meal."); // Handle case where menu is missing
                    }
                }
                catch (Exception ex)
                {
                    return View("Error", ex); // Display a generic error view
                }
            }

            // Final check if meal and its menu exist
            if (meal.Menu != null)
            {
                return RedirectToAction("SeeMenue", new { id = meal.Menu.Resturant_Id });
            }
            else
            {
                return NotFound("Menu not found for the meal."); // Handle case where menu is missing
            }
        }*/




        /*[HttpPost]
        public IActionResult ConfirmOrder(Reservation reservation, bool Delivery)
        {
            // Retrieve the Meal ID from TempData
            var mealId = TempData["MealID"] as int? ?? 0;

            if (mealId == 0)
            {
                return BadRequest("Meal ID is missing."); // Handle case where MealID is not found in TempData
            }

            // Fetch the meal from the database
            var meal = _DBContext.Meals.Include(m => m.Menu).FirstOrDefault(x => x.Id == mealId);
            if (meal == null)
            {
                return BadRequest("Meal not found."); // Handle case where meal is not found
            }

            // Assign the Meal_Id and Delivery status to the reservation
            reservation.Meal_Id = mealId;
            reservation.Delivery = Delivery;

            // Get the Customer ID from claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized(); // Handle unauthorized access
            }

            // Assign Customer_Id to the reservation
            reservation.Customer_Id = int.Parse(userId);

            // Ensure the ModelState is valid before proceeding
            if (ModelState.IsValid)
            {
                try
                {
                    // Add the reservation to the database
                    _DBContext.Reservations.Add(reservation);
                    _DBContext.SaveChanges();

                    // Redirect to the menu after a successful reservation
                    if (meal.Menu != null)
                    {
                        return RedirectToAction("SeeMenue", new { id = meal.Menu.Resturant_Id });
                    }
                    else
                    {
                        return NotFound("Menu not found for the meal."); // Handle case where menu is missing
                    }
                }
                catch (Exception ex)
                {
                    // Handle any exceptions that occur during save
                    return View("Error", ex); // Display a generic error view
                }
            }

            // If the ModelState is invalid, return to the view with the current reservation data
            return View(reservation); // Ensure you have a view to handle this scenario
        }*/
        [HttpPost]
        public IActionResult ConfirmOrder(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                // Save the reservation to the database
                _DBContext.Reservations.Add(reservation);
                _DBContext.SaveChanges();

                // Retrieve the meal details
                var meal = _DBContext.Meals.Find(reservation.Meal_Id);
                if (meal == null)
                {
                    return NotFound("Meal not found.");
                }

                // Store both in ViewBag or TempData
                ViewBag.Meal = meal;
                return View("ConfirmOrder", reservation);
            }

            // If the model is invalid, return to the MakeOrder view with the same meal
            var mealForMakeOrder = _DBContext.Meals.Find(reservation.Meal_Id);
            return View("MakeOrder", mealForMakeOrder);
        }



        public IActionResult CancelOrder(int id)
        {
            return RedirectToAction("SeeMenue", new { id = id });
        }
    }
}
