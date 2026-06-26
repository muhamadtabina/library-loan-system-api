using Library_Loan_System.Dtos.Loan;

namespace Library_Loan_System.Services.Loan;

public interface ILoanService
{
    Task<LoanResponseDto> BorrowBookAsync(LoanRequestDto request);
    Task<LoanResponseDto> ReturnBookAsync(Guid loanId);
    Task<IEnumerable<LoanResponseDto>> GetAllLoansAsync();
}