using Library_Loan_System.Dtos;
using Library_Loan_System.Dtos.Book;
using Library_Loan_System.Services.Book;
using Microsoft.AspNetCore.Mvc;

namespace Library_Loan_System.Controllers;

[ApiController]
[Route("api/v1/book")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<BookResponseDto>>> Create([FromBody] CreateBookDto request)
    {
        try
        {
            var result = await _bookService.CreateAsync(request);

            var response = new ApiResponse<BookResponseDto>(
                Code: 201,
                Status: "Created",
                Message: "Book created successfully.",
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

    [HttpGet("/api/v1/books")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BookResponseDto>>>> GetAll()
    {
        var result = await _bookService.GetAllAsync();

        if (result == null || !result.Any())
        {
            return Ok(new ApiResponse<IEnumerable<BookResponseDto>>(
                Code: 200,
                Status: "OK",
                Message: "No books found.",
                Data: Enumerable.Empty<BookResponseDto>()
            ));
        }

        return Ok(new ApiResponse<IEnumerable<BookResponseDto>>(
            Code: 200,
            Status: "OK",
            Message: "Books retrieved successfully.",
            Data: result
        ));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<BookResponseDto>>> GetById([FromRoute] Guid id)
    {
        var result = await _bookService.GetByIdAsync(id);

        if (result == null)
        {
            return NotFound(new ApiResponse<BookResponseDto>(
                Code: 404,
                Status: "Not Found",
                Message: "Book not found.",
                Data: null
            ));
        }

        return Ok(new ApiResponse<BookResponseDto>(
            Code: 200,
            Status: "OK",
            Message: "Book details retrieved successfully.",
            Data: result
        ));
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ApiResponse<BookResponseDto>>> Update([FromRoute] Guid id, [FromBody] UpdateBookDto request)
    {
        try
        {
            var result = await _bookService.UpdateAsync(id, request);

            return Ok(new ApiResponse<BookResponseDto>(
                Code: 200,
                Status: "OK",
                Message: "Book updated successfully.",
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
            var success = await _bookService.DeleteAsync(id);
            if (!success)
            {
                return NotFound(new ApiResponse<object>(
                    Code: 404,
                    Status: "Not Found",
                    Message: "Book not found.",
                    Data: null
                ));
            }

            return Ok(new ApiResponse<object>(
                Code: 200,
                Status: "OK",
                Message: "Book deleted successfully.",
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