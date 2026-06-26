namespace Library_Loan_System.Dtos.Loan;

public record LoanResponseDto(
    Guid LoanId,
    Guid StudentId,
    string StudentNim,
    string StudentName,
    Guid BookId,
    string BookTitle,
    string BookAuthor,
    string LoanDate,
    string DueDate,
    string? ReturnDate,
    int Fine,
    string Status
);