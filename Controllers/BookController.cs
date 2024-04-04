using DataBase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiInterview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private BookContext _Context;

        public BookController(BookContext context)
        {
            _Context = context; 

        }

        public IEnumerable<Book> GetBooks() => _Context.Books.ToList();



        [HttpGet("filter/title")]
        public ActionResult<IEnumerable<Book>> GetBooksByTitle(string title)
        {
            var books = _Context.Books
                .Where(b => b.Title.Contains(title))
                .Select(b => new
                {
                    b.BoockID,
                    b.Title,
                    b.Genre,
                    Status = b.Loans.Any(l => l.ReturnDate == null) ? "En préstamo" : "Disponible",
                    LoansInLastYear = b.Loans.Count(l => l.LoanDate >= DateTime.Now.AddYears(-1))
                })
                .ToList();

            return Ok(books);
        }

        [HttpGet("filter/genre")]
        public ActionResult<IEnumerable<Book>> GetBooksByGenre(string genre)
        {
            var books = _Context.Books
                .Where(b => b.Genre == genre)
                .Select(b => new
                {
                    b.BoockID,
                    b.Title,
                    b.Genre,
                    Status = b.Loans.Any(l => l.ReturnDate == null) ? "En préstamo" : "Disponible",
                    LoansInLastYear = b.Loans.Count(l => l.LoanDate >= DateTime.Now.AddYears(-1))
                })
                .ToList();

            return Ok(books);
        }

        // add book
        [HttpPost("add")]
        public IActionResult AddBook([FromBody] Book newBook)
        {
            _Context.Books.Add(newBook);
            _Context.SaveChanges();

            return Ok("Book added successfully.");
        }

        // SoftDelete Books
        [HttpPost("delete/{bookId}")]
        public IActionResult DeleteBook(int bookId)
        {
            var book = _Context.Books.Find(bookId);

            if (book == null)
            {
                return NotFound("Book not found.");
            }

            book.IsDeleted = true;
            _Context.SaveChanges();

            return Ok("Book deleted successfully.");
        }


    }
}
