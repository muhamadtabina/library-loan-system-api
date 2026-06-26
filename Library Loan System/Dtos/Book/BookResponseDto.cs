namespace Library_Loan_System.Dtos.Book;

public record BookResponseDto(
    Guid Id,
    string Title,
    string Author,
    int Stock
    );
