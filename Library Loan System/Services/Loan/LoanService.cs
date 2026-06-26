using Microsoft.EntityFrameworkCore;
using Library_Loan_System.Models;
using Library_Loan_System.Dtos.Loan;

namespace Library_Loan_System.Services.Loan;

public class LoanService : ILoanService
{
    private readonly DataContext _context;

    private const string IndonesianDateFormat = "dd-MM-yyyy HH:mm:ss";

    public LoanService(DataContext context)
    {
        _context = context;
    }

    public async Task<LoanResponseDto> BorrowBookAsync(LoanRequestDto request)
    {
        var student = await _context.Students.FindAsync(request.StudentId);
        var book = await _context.Books.FindAsync(request.BookId);

        if (student == null || book == null)
            throw new Exception("Error: Student or Book not found!");

        if (book.Stock <= 0)
            throw new Exception("Error: Book stock is empty!");

        var activeLoanCount = await _context.Loans
            .CountAsync(l => l.StudentId == request.StudentId && l.Status == "Borrowed");

        if (activeLoanCount >= 3)
            throw new Exception("Batas peminjaman maksimal 3 buku!");

        var today = DateTime.UtcNow;
        var loan = new Models.Loan
        {
            Id = Guid.NewGuid(),
            StudentId = request.StudentId,
            BookId = request.BookId,
            LoanDate = today,
            DueDate = today.AddDays(7),
            Fine = 0,
            Status = "Borrowed"
        };

        book.Stock -= 1;

        _context.Loans.Add(loan);
        await _context.SaveChangesAsync();

        return new LoanResponseDto(
            LoanId: loan.Id,
            StudentId: loan.StudentId,
            StudentNim: student.Nim,
            StudentName: student.Name,
            BookId: loan.BookId,
            BookTitle: book.Title,
            BookAuthor: loan.Book.Author,
            LoanDate: loan.LoanDate.ToString(IndonesianDateFormat),
            DueDate: loan.DueDate.ToString(IndonesianDateFormat),
            ReturnDate: loan.ReturnDate?.ToString(IndonesianDateFormat),
            Fine: loan.Fine,
            Status: loan.Status
        );
    }

    public async Task<LoanResponseDto> ReturnBookAsync(Guid loanId)
    {
        var loan = await _context.Loans
            .Include(l => l.Student)
            .Include(l => l.Book)
            .FirstOrDefaultAsync(l => l.Id == loanId);

        if (loan == null)
            throw new Exception("Error: Kode peminjaman tidak valid!");

        if (loan.Status == "Returned")
            throw new Exception("Error: Buku ini sudah pernah dikembalikan!");

        var today = DateTime.UtcNow;
        int fine = 0;

        if (today.Date > loan.DueDate.Date)
        {
            var daysLate = (today.Date - loan.DueDate.Date).Days;
            fine = daysLate * 1000;
        }

        loan.ReturnDate = today;
        loan.Fine = fine;
        loan.Status = "Returned";

        loan.Book.Stock += 1;

        await _context.SaveChangesAsync();

        return new LoanResponseDto(
            LoanId: loan.Id,
            StudentId: loan.StudentId,
            StudentNim: loan.Student.Nim,
            StudentName: loan.Student.Name,
            BookId: loan.BookId,
            BookTitle: loan.Book.Title,
            BookAuthor: loan.Book.Author,
            LoanDate: loan.LoanDate.ToString(IndonesianDateFormat),
            DueDate: loan.DueDate.ToString(IndonesianDateFormat),
            ReturnDate: loan.ReturnDate?.ToString(IndonesianDateFormat),
            Fine: loan.Fine,
            Status: loan.Status
        );
    }

    public async Task<IEnumerable<LoanResponseDto>> GetAllLoansAsync()
    {
        return await _context.Loans
            .Select(l => new LoanResponseDto(
                LoanId: l.Id,
                StudentId: l.StudentId,
                StudentNim: l.Student.Nim,
                StudentName: l.Student.Name,
                BookId: l.BookId,
                BookTitle: l.Book.Title,
                BookAuthor: l.Book.Author,
                LoanDate: l.LoanDate.ToString(IndonesianDateFormat),
                DueDate: l.DueDate.ToString(IndonesianDateFormat),
                ReturnDate: l.ReturnDate != null ? l.ReturnDate.Value.ToString(IndonesianDateFormat) : null,
                Fine: l.Fine,
                Status: l.Status
            ))
            .ToListAsync();
    }
}