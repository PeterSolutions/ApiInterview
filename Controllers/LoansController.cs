using DataBase;
using Microsoft.AspNetCore.Mvc;

namespace ApiInterview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly BookContext _context;

        public LoansController(BookContext context)
        {
            _context = context;
        }

        // Método para realizar un préstamo
        [HttpPost("loan")]
        public IActionResult LoanBook([FromBody] LoanRequest request)
        {
            var loan = new LoansJournal
            {
                UserID = request.UserId,
                BookID = request.BookId,
                LoanDate = DateTime.Now,
                ReturnDate = null // Indica que el libro aún no ha sido devuelto
            };

            _context.Loans.Add(loan);
            _context.SaveChanges();

            return Ok("Book loaned successfully to user.");
        }
    }

    public class LoanRequest
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
    }
}
