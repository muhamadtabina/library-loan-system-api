using Library_Loan_System.Dtos;
using Library_Loan_System.Dtos.Loan;
using Library_Loan_System.Services.Loan;
using Microsoft.AspNetCore.Mvc;

namespace Library_Loan_System.Controllers;

[ApiController]
[Route("api/v1/loans")]
public class LoansController : ControllerBase
{
    private readonly ILoanService _loanService;

    public LoansController(ILoanService loanService)
    {
        _loanService = loanService;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<LoanResponseDto>>> BorrowBook([FromBody] LoanRequestDto request)
    {
        try
        {
            var result = await _loanService.BorrowBookAsync(request);

            return Ok(new ApiResponse<LoanResponseDto>(
                Code: 200,
                Status: "OK",
                Message: "Book borrowed successfully.",
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

    [HttpPost("{loanId:guid}/return")]
    public async Task<ActionResult<ApiResponse<LoanResponseDto>>> ReturnBook([FromRoute] Guid loanId)
    {
        try
        {
            var result = await _loanService.ReturnBookAsync(loanId);

            return Ok(new ApiResponse<LoanResponseDto>(
                Code: 200,
                Status: "OK",
                Message: "Book returned successfully.",
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

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<LoanResponseDto>>>> GetAllLoans()
    {
        var result = await _loanService.GetAllLoansAsync();

        if (result == null || !result.Any())
        {
            return Ok(new ApiResponse<IEnumerable<LoanResponseDto>>(
                Code: 200,
                Status: "OK",
                Message: "No loan transactions found.",
                Data: Enumerable.Empty<LoanResponseDto>()
            ));
        }

        return Ok(new ApiResponse<IEnumerable<LoanResponseDto>>(
            Code: 200,
            Status: "OK",
            Message: "Loan transactions retrieved successfully.",
            Data: result
        ));
    }
}