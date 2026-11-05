using Microsoft.AspNetCore.Mvc;
using BookStore.Models;
using BookStore.ViewModels;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        // in-memory lists with seed data
        static List<Author> authors = new List<Author>
        {
            new Author { Id = 1, Name = "Ali Khan", Country = "Pakistan" },
            new Author { Id = 2, Name = "John Smith", Country = "USA" }
        };

        static List<Book> books = new List<Book>
        {
            new Book { Id = 1, Title = "Learn C#", Price = 1500, AuthorId = 1 },
            new Book { Id = 2, Title = "Web Basics", Price = 1200, AuthorId = 2 },
            new Book { Id = 3, Title = "ASP.NET Guide", Price = 1800, AuthorId = 1 }
        };

        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
            model.Authors = authors;
            model.Books = new List<BookCardViewModel>();

            foreach (Book b in books)
            {
                string authorName = "Unknown";
                foreach (Author a in authors)
                {
                    if (a.Id == b.AuthorId)
                    {
                        authorName = a.Name;
                    }
                }

                BookCardViewModel card = new BookCardViewModel();
                card.Id = b.Id;
                card.Title = b.Title;
                card.Price = b.Price;
                card.AuthorName = authorName;
                model.Books.Add(card);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult AddBook(string title, decimal price, int authorId)
        {
            // block empty title
            if (string.IsNullOrWhiteSpace(title))
            {
                return RedirectToAction("Index");
            }

            // new Id = last Id + 1
            int newId = 1;
            if (books.Count > 0)
            {
                newId = books[books.Count - 1].Id + 1;
            }

            Book book = new Book();
            book.Id = newId;
            book.Title = title;
            book.Price = price;
            book.AuthorId = authorId;
            books.Add(book);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddAuthor(string name, string country)
        {
            int newId = 1;
            if (authors.Count > 0)
            {
                newId = authors[authors.Count - 1].Id + 1;
            }

            Author author = new Author();
            author.Id = newId;
            author.Name = name;
            author.Country = country;
            authors.Add(author);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveBook(int id)
        {
            Book bookToRemove = null;
            foreach (Book b in books)
            {
                if (b.Id == id)
                {
                    bookToRemove = b;
                }
            }

            if (bookToRemove != null)
            {
                books.Remove(bookToRemove);
            }

            return RedirectToAction("Index");
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}