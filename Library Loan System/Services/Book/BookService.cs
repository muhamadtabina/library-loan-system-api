using Library_Loan_System.Dtos.Book;
using Microsoft.EntityFrameworkCore;

namespace Library_Loan_System.Services.Book;

public class BookService : IBookService
{
    private readonly DataContext _context;

    public BookService(DataContext context)
    {
        _context = context;
    }

    public async Task<BookResponseDto> CreateAsync(CreateBookDto request)
    {
        var book = new Models.Book
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Author = request.Author,
            Stock = request.Stock
        };

        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return new BookResponseDto(
            Id: book.Id,
            Title: book.Title,
            Author: book.Author,
            Stock: book.Stock
        );
    }

    public async Task<IEnumerable<BookResponseDto>> GetAllAsync()
    {
        return await _context.Books
            .Select(b => new BookResponseDto(
                Id: b.Id,
                Title: b.Title,
                Author: b.Author,
                Stock: b.Stock
            ))
            .ToListAsync();
    }

    public async Task<BookResponseDto?> GetByIdAsync(Guid id)
    {
        var b = await _context.Books.FindAsync(id);
        if (b == null) return null;

        return new BookResponseDto(
            Id: b.Id,
            Title: b.Title,
            Author: b.Author,
            Stock: b.Stock
        );
    }

    public async Task<BookResponseDto> UpdateAsync(Guid id, UpdateBookDto request)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) throw new Exception("Book not found!");

        book.Title = request.Title;
        book.Author = request.Author;
        book.Stock = request.Stock;

        await _context.SaveChangesAsync();

        return new BookResponseDto(
            Id: book.Id,
            Title: book.Title,
            Author: book.Author,
            Stock: book.Stock
        );
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return false;

        var isBorrowed = await _context.Loans.AnyAsync(l => l.BookId == id && l.Status == "Borrowed");
        if (isBorrowed) throw new Exception("Cannot delete book that is currently borrowed!");

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return true;
    }
}