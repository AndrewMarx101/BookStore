using Microsoft.AspNetCore.Mvc;

namespace BookStore.Models
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        

    }
    public class Book
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        
    }
}
