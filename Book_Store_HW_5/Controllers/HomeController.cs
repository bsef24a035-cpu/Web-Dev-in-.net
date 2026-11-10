using Book_Store_HW_5.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Book_Store_HW_5.Controllers
{
    public class HomeController : Controller
    {
        public static List<Book> books = [new Book(1, "Divane Ghalib", 1500, 2), new Book(2, "Divane Mir Taqi Mir", 1700, 2), new Book(3, "Silent Killer", 5700, 1)];
        public static List<Author> authors = [new Author(1, "John Doe", "USA"), new Author(2, "Muhammad Ali", "Pakistan")];
        private Book addBook(Book book)
        {
            int size = books.Count;
            book.Id = size > 0 ? books[size - 1].Id + 1 : 1;
            books.Add(book);
            return book;
        }
        private Author addAuthor(Author author)
        {
            int size = authors.Count;
            author.Id = size > 0 ? authors[size - 1].Id + 1 : 1;
            authors.Add(author);
            return author;
        }
        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
            model.authors = authors;
            List<BookCardViewModel> booksDisplay = new List<BookCardViewModel>();
            foreach (var item in books)
            {
                BookCardViewModel bcvm = new BookCardViewModel();
                bcvm.Id = item.Id;
                bcvm.Title = item.Title;
                bcvm.Price = item.Price;
                foreach (var item1 in authors)
                {
                    Console.WriteLine(item1.Id);
                    if(item1.Id == item.AuthorId){
                        bcvm.AuthorName = item1.Name;
                        System.Console.WriteLine(bcvm.AuthorName);
                        break;
                    }
                }
                booksDisplay.Add(bcvm);
            }
            model.books = booksDisplay;
            return View(model);
        }
        [HttpPost]
        public IActionResult AddBook(string title, decimal price, string authorId){
            Book bk = new Book(1, title, price, int.Parse(authorId));
            addBook(bk);
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult AddAuthor(string name, string country)
        {
            Author bk = new Author(1, name, country);
            addAuthor(bk);
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult RemoveBook(int id)
        {
            Console.WriteLine(id);
            foreach (var item in books) {
                if (item.Id == id) { 
                    books.Remove(item);
                    break;
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
