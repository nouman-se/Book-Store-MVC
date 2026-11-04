using System.Diagnostics;
using BookStoreHW5.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BookStoreHW5.ViewModels;

namespace BookStoreHW5.Controllers
{
    public class HomeController : Controller
    {
        private static List<Author> authors = new List<Author>()
{
    new Author()
    {
        Id = 1,
        Name = "Ahmed",
        Country = "Pakistan"
    },

    new Author()
    {
        Id = 2,
        Name = "Sara",
        Country = "Pakistan"
    }

};
        private static List<Book> books = new List<Book>()
{
    new Book()
    {
        Id = 1,
        Title = "Learning C#",
        Price = 1200m,
        AuthorId = 1
    },

    new Book()
    {
        Id = 2,
        Title = "Web Basics",
        Price = 950m,
        AuthorId = 2
    },

    new Book()
    {
        Id = 3,
        Title = "MVC Practice",
        Price = 1500m,
        AuthorId = 1
    }
};



        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();

            foreach (Book book in books)
            {
                string authorName = "Unknown";

                foreach (Author author in authors)
                {
                    if (author.Id == book.AuthorId)
                    {
                        authorName = author.Name;
                        break;
                    }
                }

                BookCardViewModel card = new BookCardViewModel();

                card.Id = book.Id;
                card.Title = book.Title;
                card.Price = book.Price;
                card.AuthorName = authorName;

                model.Books.Add(card);
            }

            model.Authors = authors;

            return View(model);
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
        [ValidateAntiForgeryToken]
        public IActionResult AddAuthor(string name, string country)
        {
            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(country))
            {
                TempData["Message"] = "Please enter the author's name and country.";
                return RedirectToAction("Index");
            }

            int newId = 1;

            if (authors.Count > 0)
            {
                newId = authors[authors.Count - 1].Id + 1;
            }

            Author author = new Author();

            author.Id = newId;
            author.Name = name.Trim();
            author.Country = country.Trim();

            authors.Add(author);

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBook(string title, decimal price, int authorId)
        {
            if (!ModelState.IsValid ||
                string.IsNullOrWhiteSpace(title) ||
                price < 0)
            {
                TempData["Message"] = "Enter a title and a valid non-negative price.";
                return RedirectToAction("Index");
            }

            bool authorExists = false;

            foreach (Author author in authors)
            {
                if (author.Id == authorId)
                {
                    authorExists = true;
                    break;
                }
            }

            if (!authorExists)
            {
                TempData["Message"] = "Please select an existing author.";
                return RedirectToAction("Index");
            }

            int newId = 1;

            if (books.Count > 0)
            {
                newId = books[books.Count - 1].Id + 1;
            }

            Book book = new Book();

            book.Id = newId;
            book.Title = title.Trim();
            book.Price = price;
            book.AuthorId = authorId;

            books.Add(book);

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveBook(int id)
        {
            for (int i = 0; i < books.Count; i++)
            {
                if (books[i].Id == id)
                {
                    books.RemoveAt(i);
                    break;
                }
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveAuthor(int id)
        {
            foreach (Book book in books)
            {
                if (book.AuthorId == id)
                {
                    TempData["Message"] =
                        "Remove this author's books before removing the author.";

                    return RedirectToAction("Index");
                }
            }

            for (int i = 0; i < authors.Count; i++)
            {
                if (authors[i].Id == id)
                {
                    authors.RemoveAt(i);
                    break;
                }
            }

            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
