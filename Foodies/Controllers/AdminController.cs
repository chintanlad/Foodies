using Foodies.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Foodies.Controllers
{

    public class AdminController : Controller
    {
        private readonly ApplicationDBContext _DBContext;
        public AdminController(ApplicationDBContext dbContext)
        {
            _DBContext = dbContext;
        }
        public IActionResult Index()
        {
            var customers = _DBContext.Customers.ToList();


            ViewBag.Customers = customers;
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var admin = _DBContext.Admins.FirstOrDefault(a => a.Email == email && a.Password == password);
                if (admin != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid email or password");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult AddCustomer()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCustomer(Customer customer)
        {
            if(ModelState.IsValid)
            {
                _DBContext.Customers.Add(customer);
                _DBContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        [HttpGet]
        public IActionResult UpdateCustomer(int id)
        {
            var customer=_DBContext.Customers.FirstOrDefault(a => a.Id == id);
            if(customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }
        [HttpPost]
        public IActionResult UpdateCustomer(Customer customer)
        {
            if(ModelState.IsValid && customer != null)
            {
                _DBContext.Entry(customer).State = EntityState.Modified;
                _DBContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }
        
        public IActionResult DeleteCustomer(int id)
        {
            var customerToDelete = _DBContext.Customers.FirstOrDefault(c => c.Id == id);
            if (customerToDelete != null)
            {
                _DBContext.Customers.Remove(customerToDelete);
                _DBContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //end crud customer 


        [HttpGet]
        public IActionResult AllResturants()
        {
            var resturants = _DBContext.Restaurants.ToList();
            ViewBag.Resturants = resturants;
            return View();
        }

        [HttpGet]
        public IActionResult AddRestaurant()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddRestaurant(Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                _DBContext.Restaurants.Add(restaurant);
                _DBContext.SaveChanges();

                //menu 
                var newMenu = new Menu { Resturant_Id = restaurant.Id };
                _DBContext.Menus.Add(newMenu);
                _DBContext.SaveChanges();
                return RedirectToAction("AllResturants");
            }
            return View(restaurant);
        }
       /* [HttpPost]
        public IActionResult AddRestaurant(Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                // Add the restaurant to the database
                _DBContext.Restaurants.Add(restaurant);
                _DBContext.SaveChanges();
                return RedirectToAction("AllRestaurants");
            }
            return View(restaurant);
        }*/

        [HttpGet]
        public IActionResult UpdateRestaurant(int id)
        {
            var resturant = _DBContext.Restaurants.FirstOrDefault(a => a.Id == id);
            if(resturant == null)
            {
                return NotFound();
            }
            return View(resturant);
        }
       /* [HttpPost]
        public IActionResult UpdateRestaurant(Restaurant restaurant, IFormFile ImageFile)
        {
            if (ModelState.IsValid)
            {
                // Handle the image file upload
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Define the path where the image will be saved
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", ImageFile.FileName);

                    // Save the image to the specified path
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    // Update the ImgPath field in the database to the new file path
                    restaurant.ImgPath = "/images/" + ImageFile.FileName;
                }

                _DBContext.Entry(restaurant).State = EntityState.Modified;
                _DBContext.SaveChanges();
                return RedirectToAction("AllResturants");
            }

            return View(restaurant);
        }*/

        [HttpPost]
        public IActionResult UpdateRestaurant(Restaurant restaurant)
        {
            if(ModelState.IsValid)
            {
                _DBContext.Entry(restaurant).State = EntityState.Modified;
                _DBContext.SaveChanges();
                return RedirectToAction("AllResturants");
            }
            return View(restaurant);
        }


        public IActionResult DeleteRestaurant(int id)
        {
            var restaurant = _DBContext.Restaurants.FirstOrDefault(r => r.Id == id);
            if (restaurant != null)
            {
                _DBContext.Restaurants.Remove(restaurant);
                _DBContext.SaveChanges();
            }
            return RedirectToAction("AllResturants");
        }
        //end resturant

        [HttpGet("/Admin/EditMenue/{id}")]

        public IActionResult EditMenue(int id)
        {
            var menuId = _DBContext.Menus.FirstOrDefault(m => m.Resturant_Id == id)?.Id;
            if (menuId != null)
            {

                ViewBag.MenuId = menuId;
                TempData["MenuId"] = menuId;
                //var menuItems = _DBContext.Meals.Where(m => m.Menu_Id == menuId).ToList();

                var menuItems = _DBContext.Meals
            .Include(m => m.Menu)
            .ThenInclude(menu => menu.Restaurant)
            .Where(m => m.Menu_Id == menuId)
            .ToList();

                return View(menuItems);
            }
            return NotFound();
        }
        /*public IActionResult EditMenue(int menuId)
        {
            // Get the menu and ensure it exists
            var menu = _DBContext.Menus.Include(m => m.Restaurant)
                                       .FirstOrDefault(m => m.Id == menuId);

            if (menu != null)
            {
                ViewBag.MenuId = menuId;
                TempData["MenuId"] = menuId;

                // Fetch all meals related to the menu
                var menuItems = _DBContext.Meals
                    .Include(m => m.Menu)
                    .ThenInclude(m => m.Restaurant)
                    .Where(m => m.Menu_Id == menuId)
                    .ToList();

                return View(menuItems);
            }

            return NotFound();
        }
*/

        [HttpGet]
        public IActionResult AddMeal()
        {
            //int MenuId = 0; // Default value in case TempData["MenuId"] is null or not convertible to int
            //if (TempData["MenuId"] != null && int.TryParse(TempData["MenuId"].ToString(), out int tempMenuId))
            //{
            //    MenuId = tempMenuId; 
            //    ViewBag.MenuId= MenuId;
            //}
            return View();
        }

        /*[HttpPost]
        public IActionResult AddMeal(Meal meal)
        {
            ModelState.Remove("Menu");

            //int MenuId = 0; // Default value in case TempData["MenuId"] is null or not convertible to int
            //if (TempData["MenuId"] != null && int.TryParse(TempData["MenuId"].ToString(), out int tempMenuId))
            //{
            //    MenuId = tempMenuId;
            //    meal.Menu_Id = MenuId;
            //}


            if (TempData["MenuId"] != null && int.TryParse(TempData["MenuId"].ToString(), out int menuId))
            {
                meal.Menu_Id = menuId;
            }


            if (ModelState.IsValid)
            {
                _DBContext.Meals.Add(meal);
                _DBContext.SaveChanges();
                return RedirectToAction("EditMenue", new { id = meal.Menu_Id });
            }
            return View(meal);
        }*/
        [HttpPost]
        public IActionResult AddMeal(Meal meal)
        {
            // Remove validation for Menu if necessary
            ModelState.Remove("Menu");

            // Fetch the Restaurant_Id from the Menus table using the MenuId from TempData
            if (TempData["MenuId"] != null && int.TryParse(TempData["MenuId"].ToString(), out int menuId))
            {
                var menu = _DBContext.Menus.FirstOrDefault(m => m.Id == menuId);
                if (menu != null)
                {
                    int restaurantId = menu.Resturant_Id; // Fetch the associated Restaurant_Id
                                                          // Optionally set the Menu_Id for the Meal if it's required for the relationship
                    meal.Menu_Id = menuId;

                    // Proceed if the ModelState is valid
                    if (ModelState.IsValid)
                    {
                        _DBContext.Meals.Add(meal);
                        _DBContext.SaveChanges();
                        // Redirect to an action that displays or edits the restaurant or menu
                        return RedirectToAction("EditMenue", new { id = restaurantId });
                    }
                }
                else
                {
                    // Handle case where Menu is not found
                    ModelState.AddModelError("", "Invalid MenuId");
                }
            }

            // If something goes wrong, return the view with the model data
            return View(meal);
        }


        [HttpGet]
        public IActionResult UpdateMeal(int id)
        {
            var meal = _DBContext.Meals.FirstOrDefault(a => a.Id == id);
            if (meal == null)
            {
                return NotFound(); // Handle meal not found case
            }
            return View(meal);
        }
        /*[HttpPost]
        public IActionResult UpdateMeal(Meal meal)
        {
            if (ModelState.IsValid)
            {
                _DBContext.Entry(meal).State = EntityState.Modified;
                _DBContext.SaveChanges();
                return RedirectToAction("EditMenue", new { id = meal.Menu_Id });
            }
            return View(meal);
        }*/

        [HttpPost]
        public IActionResult UpdateMeal(Meal updatedMeal)
        {
            // Remove validation for Menu if necessary
            ModelState.Remove("Menu");

            // Fetch the existing Meal record from the database using the provided Meal Id
            var existingMeal = _DBContext.Meals.FirstOrDefault(m => m.Id == updatedMeal.Id);
            if (existingMeal == null)
            {
                // Handle case where Meal is not found
                ModelState.AddModelError("", "Meal not found.");
                return View(updatedMeal);
            }

            // Fetch the Restaurant_Id from the Menus table using the existing Menu_Id
            var menu = _DBContext.Menus.FirstOrDefault(m => m.Id == existingMeal.Menu_Id);
            if (menu != null)
            {
                int restaurantId = menu.Resturant_Id; // Fetch the associated Restaurant_Id

                // Update the Meal's fields, but preserve the Menu_Id
                existingMeal.MealName = updatedMeal.MealName;
                existingMeal.Price = updatedMeal.Price;
                existingMeal.Description = updatedMeal.Description;

                // Proceed if the ModelState is valid
                if (ModelState.IsValid)
                {
                    _DBContext.Meals.Update(existingMeal);  // Mark the Meal as updated
                    _DBContext.SaveChanges();               // Save changes to the database

                    // Redirect to an action, e.g., editing the restaurant's menu
                    return RedirectToAction("EditMenue", new { id = restaurantId });
                }
            }
            else
            {
                // Handle case where Menu is not found
                ModelState.AddModelError("", "Invalid MenuId");
            }

            // If something goes wrong, return the view with the updated meal data
            return View(updatedMeal);
        }


        /* public IActionResult DeleteMeal(int id)
         {
             var meal = _DBContext.Meals.FirstOrDefault(m => m.Id == id);
             if (meal != null)
             {

                 _DBContext.Meals.Remove(meal);
                 _DBContext.SaveChanges();

                 return RedirectToAction("EditMenue", new { id = meal.Menu_Id });
             }
             //return RedirectToAction("EditMenu", new { id = meal.Menu_Id });
             return NotFound(); // Handle if meal was not found
         }*/

        public IActionResult DeleteMeal(int id)
        {
            var meal = _DBContext.Meals.FirstOrDefault(m => m.Id == id);
            if (meal != null)
            {
                // Fetch the associated Menu to get the Restaurant_Id
                var menu = _DBContext.Menus.FirstOrDefault(m => m.Id == meal.Menu_Id);
                if (menu != null)
                {
                    // Remove the meal
                    _DBContext.Meals.Remove(meal);
                    _DBContext.SaveChanges();

                    // Redirect to the EditMenue action with the restaurantId
                    return RedirectToAction("EditMenue", new { id = menu.Resturant_Id });
                }

                // Handle case where Menu is not found (this should be rare)
                ModelState.AddModelError("", "Associated menu not found.");
                return RedirectToAction("MenuList"); // Adjust this as needed
            }

            // Handle if meal was not found
            return NotFound();
        }





    }
}
