namespace Library_Loan_System.Dtos.Student;

public record StudentResponseDto(
    Guid Id,
    string Nim,
    string Name,
    string Major,
    int CurrentLoanCount
    );

