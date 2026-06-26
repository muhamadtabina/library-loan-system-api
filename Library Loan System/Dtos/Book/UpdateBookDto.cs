namespace Library_Loan_System.Dtos.Book;

public record UpdateBookDto(
    string Title,
    string Author,
    int Stock
    );
