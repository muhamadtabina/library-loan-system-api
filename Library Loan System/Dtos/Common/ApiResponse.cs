namespace Library_Loan_System.Dtos;

public record ApiResponse<T>(
    int Code,
    string Status,
    string Message,
    T? Data
);