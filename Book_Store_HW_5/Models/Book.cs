namespace Book_Store_HW_5
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int AuthorId { get; set; }

        public Book(int id, string title, decimal price, int authorId)
        {
            Id = id;
            Title = title;
            Price = price;
            AuthorId = authorId;
        }
    }
}
