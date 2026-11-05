using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using BookStore.ViewModels;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        public static List<Author> Authors = new List<Author>
        {
            new Author { Id = 1, Name = "George Orwell", Country = "United Kingdom" },
            new Author { Id = 2, Name = "Paulo Coelho", Country = "Brazil" }
        };

        public static List<Book> Books = new List<Book>
        {
            new Book { Id = 1, Title = "1984", Price = 15.99m, AuthorId = 1 },
            new Book { Id = 2, Title = "Animal Farm", Price = 12.50m, AuthorId = 1 },
            new Book { Id = 3, Title = "The Alchemist", Price = 18.00m, AuthorId = 2 }
        };

        public IActionResult Index()
        {
            HomeViewModel viewModel = new HomeViewModel
            {
                Books = new List<BookCardViewModel>(),
                Authors = Authors
            };

            foreach (Book book in Books)
            {
                Author author = Authors.FirstOrDefault(a => a.Id == book.AuthorId);
                string authorName = author.Name;

                viewModel.Books.Add(new BookCardViewModel
                {
                    Id = book.Id,
                    Title = book.Title,
                    Price = book.Price,
                    AuthorName = authorName
                });
            }

            return View(viewModel);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBook(string title, decimal price, int authorId)
        {
            if (!string.IsNullOrWhiteSpace(title))
            {
                int newId = Books.Count > 0 ? Books[Books.Count - 1].Id + 1 : 1;

                Books.Add(new Book{
                    Id = newId,
                    Title = title,
                    Price = price,
                    AuthorId = authorId
                });
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddAuthor(string name, string country)
        {
            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(country))
            {
                int newId = Authors.Count > 0 ? Authors[Authors.Count - 1].Id + 1 : 1;

                Authors.Add(new Author
                {
                    Id = newId,
                    Name = name,
                    Country = country
                });
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveBook(int id)
        {
            Book bookToRemove = Books.FirstOrDefault(b => b.Id == id);
            if (bookToRemove != null)
            {
                Books.Remove(bookToRemove);
            }

            return RedirectToAction("Index");
        }
    }
}