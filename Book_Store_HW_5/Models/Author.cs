namespace Book_Store_HW_5
{
    public class Author
    {
        public int Id{ get; set; }
        public string Name{ get; set; }
        public string Country { get; set; }

        public Author(int id, string name, string country)
        {
            this.Id = id;
            this.Name = name;
            this.Country = country;
        }
    }
}
