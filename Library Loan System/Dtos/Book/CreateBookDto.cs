namespace Library_Loan_System.Dtos.Book;

public record CreateBookDto(
    string Title,
    string Author,
    int Stock
    );
