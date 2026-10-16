using Microsoft.AspNetCore.Mvc;
using Rental_Car_Application.Models;

namespace Rental_Car_Application.Controllers
{
    public class CarsController : Controller
    {
        public static List<Car> availableCars = [new Car(1,"Alto", 3000), new Car(2, "Corolla", 6000), new Car(3, "Civic", 8000)];
        public static List<Car> bookedCars = [];
        [HttpGet]
        public IActionResult Index()
        {
            return View(availableCars);
        }
        [HttpPost]
        public IActionResult BookACar(int id)
        {
            Car temp = new Car();
            foreach (var item in availableCars)
            {
                if (item.id == id)
                {
                    temp.id = item.id;
                    temp.name = item.name;
                    temp.pricePerDay = item.pricePerDay;
                    availableCars.RemoveAll(i => i.id == temp.id);
                    break;
                }
            }
            if (temp.id == 0)
            {
                return RedirectToAction("Index");
            }
            bookedCars.Add(temp);
            return RedirectToAction("BookedCar");
        }
        [HttpGet]
        public IActionResult BookedCar() { 
            return View(bookedCars);
        }
    }
}
