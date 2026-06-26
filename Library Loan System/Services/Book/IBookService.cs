using Library_Loan_System.Dtos.Book;

namespace Library_Loan_System.Services.Book;

public interface IBookService
{
    Task<BookResponseDto> CreateAsync(CreateBookDto request);
    Task<IEnumerable<BookResponseDto>> GetAllAsync();
    Task<BookResponseDto?> GetByIdAsync(Guid id);
    Task<BookResponseDto> UpdateAsync(Guid id, UpdateBookDto request);
    Task<bool> DeleteAsync(Guid id);
}