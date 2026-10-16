namespace Rental_Car_Application.Models
{
    public class Car
    {
        public int id { get; set; }
        public string name { get; set; }
        public decimal pricePerDay { get; set; }
        public Car(int id = 0, string name="", decimal pricePerDay=0)
        {
            this.id = id;
            this.name = name;
            this.pricePerDay = pricePerDay;
        }
    }
}
