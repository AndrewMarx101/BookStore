using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BookStore.Models;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Text;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private const string CartFilePath = @"C:\Users\Andrew\source\repos\BookStore\CartItems.txt";

        public IActionResult Index()
        {
            List<Book> books = GetBooks(); // Replace this with your actual data retrieval logic

            int cartCount = GetCartItemCount();
            ViewBag.CartCount = cartCount;

            return View(books);
        }

        [HttpPost]
        public IActionResult AddToCart(Book book)
        {
            // Add the book to the cart
            AddToCartFile(book);

            int cartCount = GetCartItemCount();
            ViewBag.CartCount = cartCount;

            return RedirectToAction("Index");
        }

        public IActionResult Cart()
        {
            List<Book> cartItems = GetCartItems();
            return View(cartItems);
        }

        private List<Book> GetBooks()
        {
            List<Book> books = new List<Book>();

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Books.txt");

            string[] lines = System.IO.File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] bookData = line.Split(',');

                if (bookData.Length == 5)
                {
                    Book book = new Book
                    {
                        Name = bookData[0],
                        Author = bookData[1],
                        Type = bookData[2],
                        Price = decimal.Parse(bookData[3]),
                        ImageUrl = bookData[4]
                    };

                    books.Add(book);
                }
            }

            return books;
        }

        private List<Book> GetCartItems()
        {
            List<Book> cartItems = new List<Book>();

            if (System.IO.File.Exists(CartFilePath))
            {
                string[] lines = System.IO.File.ReadAllLines(CartFilePath);
                foreach (string line in lines)
                {
                    string[] bookData = line.Split(',');

                    if (bookData.Length == 5)
                    {
                        Book book = new Book
                        {
                            Name = bookData[0],
                            Author = bookData[1],
                            Type = bookData[2],
                            Price = decimal.Parse(bookData[3]),
                            ImageUrl = bookData[4]
                        };

                        cartItems.Add(book);
                    }
                }
            }

            return cartItems;
        }

        private void AddToCartFile(Book book)
        {
            string line = $"{book.Name},{book.Author},{book.Type},{book.Price},{book.ImageUrl}";
            System.IO.File.AppendAllText(CartFilePath, line + "\n");
        }

        private int GetCartItemCount()
        {
            int count = 0;

            if (System.IO.File.Exists("CartItems.txt"))
            {
                string[] lines = System.IO.File.ReadAllLines(CartFilePath);
                count = lines.Length;
            }

            return count;
        }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Search(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                // If searchQuery is null or empty, return an empty search result
                List<Book> emptyResult = new List<Book>();
                return View(emptyResult);
            }

            List<Book> searchResults = GetBooks().Where(b => b.Name.Contains(searchQuery) || b.Author.Contains(searchQuery)).ToList();
            return View(searchResults);
        }

        [HttpGet]
        public IActionResult AddBooks()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBooks(Book book)
        {
            if (ModelState.IsValid)
            {
                string bookEntry = $"{book.Name},{book.Author},{book.Type},{book.Price},{book.ImageUrl}";
                string filePath = "Books.txt";

                try
                {
                    System.IO.File.AppendAllText(filePath, bookEntry + Environment.NewLine);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Log the exception or display an error message
                    ViewBag.ErrorMessage = $"An error occurred while adding the book: {ex.Message}";
                    return View(book);
                }
            }

            return View(book);
        }


    }
}
