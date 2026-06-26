using Library_Loan_System.Dtos.Student;
using Microsoft.EntityFrameworkCore;

namespace Library_Loan_System.Services.Student;

public class StudentService : IStudentService
{
    private readonly DataContext _context;

    public StudentService(DataContext context)
    {
        _context = context;
    }

    public async Task<StudentResponseDto> CreateAsync(CreateStudentDto request)
    {
        var exists = await _context.Students.AnyAsync(s => s.Nim == request.Nim);
        if (exists) throw new Exception("NIM is already registered!");

        var student = new Models.Student
        {
            Id = Guid.NewGuid(),
            Nim = request.Nim,
            Name = request.Name,
            Major = request.Major
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return new StudentResponseDto(
            Id: student.Id,
            Nim: student.Nim,
            Name: student.Name,
            Major: student.Major,
            CurrentLoanCount: 0
        );
    }

    public async Task<IEnumerable<StudentResponseDto>> GetAllAsync()
    {
        return await _context.Students
            .Select(s => new StudentResponseDto(
                Id: s.Id,
                Nim: s.Nim,
                Name: s.Name,
                Major: s.Major,
                CurrentLoanCount: _context.Loans.Count(l => l.StudentId == s.Id && l.Status == "Borrowed")
            ))
            .ToListAsync();
    }

    public async Task<StudentResponseDto?> GetByIdAsync(Guid id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null) return null;

        var loanCount = await _context.Loans
            .CountAsync(l => l.StudentId == id && l.Status == "Borrowed");

        return new StudentResponseDto(
            Id: student.Id,
            Nim: student.Nim,
            Name: student.Name,
            Major: student.Major,
            CurrentLoanCount: loanCount
        );
    }

    public async Task<StudentResponseDto> UpdateAsync(Guid id, UpdateStudentDto request)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null) throw new Exception("Student not found!");

        if (student.Nim != request.Nim)
        {
            var exists = await _context.Students.AnyAsync(s => s.Nim == request.Nim);
            if (exists) throw new Exception("NIM is already registered by another student!");
        }

        student.Nim = request.Nim;
        student.Name = request.Name;
        student.Major = request.Major;

        await _context.SaveChangesAsync();

        var loanCount = await _context.Loans
            .CountAsync(l => l.StudentId == id && l.Status == "Borrowed");

        return new StudentResponseDto(
            Id: student.Id,
            Nim: student.Nim,
            Name: student.Name,
            Major: student.Major,
            CurrentLoanCount: loanCount
        );
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student == null) return false;

        var hasActiveLoan = await _context.Loans.AnyAsync(l => l.StudentId == id && l.Status == "Borrowed");
        if (hasActiveLoan) throw new Exception("Cannot delete student with active loans!");

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();
        return true;
    }
}