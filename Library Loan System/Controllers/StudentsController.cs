using Library_Loan_System.Dtos;
using Library_Loan_System.Dtos.Student;
using Library_Loan_System.Services.Student;
using Microsoft.AspNetCore.Mvc;

namespace Library_Loan_System.Controllers;

[ApiController]
[Route("api/v1/student")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<StudentResponseDto>>> Create([FromBody] CreateStudentDto request)
    {
        try
        {
            var result = await _studentService.CreateAsync(request);

            var response = new ApiResponse<StudentResponseDto>(
                Code: 201,
                Status: "Created",
                Message: "Student registered successfully.",
                Data: result
            );

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, response);
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>(
                Code: 400,
                Status: "Bad Request",
                Message: ex.Message,
                Data: null
            ));
        }
    }

    [HttpGet("/api/v1/students")]
    public async Task<ActionResult<ApiResponse<IEnumerable<StudentResponseDto>>>> GetAll()
    {
        var result = await _studentService.GetAllAsync();

        if (result == null || !result.Any())
        {
            return Ok(new ApiResponse<IEnumerable<StudentResponseDto>>(
                Code: 200,
                Status: "OK",
                Message: "No students found.",
                Data: Enumerable.Empty<StudentResponseDto>()
            ));
        }

        return Ok(new ApiResponse<IEnumerable<StudentResponseDto>>(
            Code: 200,
            Status: "OK",
            Message: "Students retrieved successfully.",
            Data: result
        ));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<StudentResponseDto>>> GetById([FromRoute] Guid id)
    {
        var result = await _studentService.GetByIdAsync(id);

        if (result == null)
        {
            return NotFound(new ApiResponse<StudentResponseDto>(
                Code: 404,
                Status: "Not Found",
                Message: "Student not found.",
                Data: null
            ));
        }

        return Ok(new ApiResponse<StudentResponseDto>(
            Code: 200,
            Status: "OK",
            Message: "Student details retrieved successfully.",
            Data: result
        ));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<StudentResponseDto>>> Update([FromRoute] Guid id, [FromBody] UpdateStudentDto request)
    {
        try
        {
            var result = await _studentService.UpdateAsync(id, request);

            return Ok(new ApiResponse<StudentResponseDto>(
                Code: 200,
                Status: "OK",
                Message: "Student updated successfully.",
                Data: result
            ));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>(
                Code: 400,
                Status: "Bad Request",
                Message: ex.Message,
                Data: null
            ));
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete([FromRoute] Guid id)
    {
        try
        {
            var success = await _studentService.DeleteAsync(id);
            if (!success)
            {
                return NotFound(new ApiResponse<object>(
                    Code: 404,
                    Status: "Not Found",
                    Message: "Student not found.",
                    Data: null
                ));
            }

            return Ok(new ApiResponse<object>(
                Code: 200,
                Status: "OK",
                Message: "Student deleted successfully.",
                Data: null
            ));
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>(
                Code: 400,
                Status: "Bad Request",
                Message: ex.Message,
                Data: null
            ));
        }
    }
}