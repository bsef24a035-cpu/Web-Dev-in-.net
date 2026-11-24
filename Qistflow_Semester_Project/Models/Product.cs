namespace Qistflow_Semester_Project.Models
{
    public class Product
    {
        public int productId { get; set; } 
        public int categoryId { get; set; }
        public decimal DownPayment { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal CashPrice { get; set; }
        public decimal InstallmentPrice { get; set; }
        public int InstallmentMonths { get; set; }
        public string? ImageUrl { get; set; }
        public string ? Brand { get; set; }
        public int StockQuantity { get; set; }

    }
}
