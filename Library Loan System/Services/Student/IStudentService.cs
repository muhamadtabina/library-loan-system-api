using Library_Loan_System.Dtos.Student;

namespace Library_Loan_System.Services.Student;

public interface IStudentService
{
    Task<StudentResponseDto> CreateAsync(CreateStudentDto request);
    Task<IEnumerable<StudentResponseDto>> GetAllAsync();
    Task<StudentResponseDto?> GetByIdAsync(Guid id);
    Task<StudentResponseDto> UpdateAsync(Guid id, UpdateStudentDto request);
    Task<bool> DeleteAsync(Guid id);
}