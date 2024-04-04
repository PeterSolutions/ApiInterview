using DataBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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


        //Metodo para retornar un book
        [HttpPost("return/{bookId}")]
        public IActionResult ReturnBook(int bookId)
        {
            var loan = _context.Loans
                               .Where(l => l.BookID == bookId && l.ReturnDate == null)
                               .OrderByDescending(l => l.LoanDate)
                               .FirstOrDefault();

            if (loan != null)
            {
                loan.ReturnDate = DateTime.Now;
                _context.SaveChanges();
                return Ok("Book returned successfully.");
            }

            return NotFound("Loan record not found.");
        }
    }

    public class LoanRequest
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
    }

    
}
