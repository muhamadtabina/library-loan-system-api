namespace Library_Loan_System.Dtos.Loan;

public record LoanRequestDto(
   Guid StudentId,
   Guid BookId
);
