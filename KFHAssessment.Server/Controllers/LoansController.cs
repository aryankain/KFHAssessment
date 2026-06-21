using KFHAssessment.Server.Services;
using KFHAssessment.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KFHAssessment.Server.Controllers;

[ApiController]
[Route("api/loans")]
[Authorize]
public class LoansController : ControllerBase
{
    private readonly LoanService _loans;
    public LoansController(LoanService loans) => _loans = loans;

    [HttpGet]
    public async Task<ActionResult<List<LoanDto>>> GetAll() =>
        Ok(await _loans.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LoanDto>> GetById(int id)
    {
        var loan = await _loans.GetByIdAsync(id);
        return loan is null ? NotFound() : Ok(loan);
    }

    [HttpPost]
    public async Task<ActionResult<LoanDto>> Create([FromBody] CreateLoanDto dto)
    {
        var created = await _loans.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPost("{id:int}/evaluate")]
    public async Task<ActionResult<LoanEvaluationResultDto>> Evaluate(int id)
    {
        var result = await _loans.EvaluateAsync(id);
        return result is null ? NotFound() : Ok(result);
    }
}