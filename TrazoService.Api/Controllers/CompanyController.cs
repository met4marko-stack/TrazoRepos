using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrazoService.Application.Features.Companies.Commands;
using TrazoService.Application.Features.Companies.DTOs;
using TrazoService.Application.Features.Companies.Queries;

namespace TrazoService.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CompanyController : ControllerBase
{
    private readonly IMediator _mediator;

    public CompanyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CompanyDto>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllCompaniesQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CompanyDto>> GetById(int id)
    {
        var result = await _mediator.Send(new GetCompanyByIdQuery(id));
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("razonsocial/{razonSocial}")]
    public async Task<ActionResult<CompanyDto>> GetByRazonSocial(string razonSocial)
    {
        var result = await _mediator.Send(new GetCompanyByRazonSocialQuery(razonSocial));
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateCompanyCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCompanyCommand command)
    {
        if (id != command.Id) return BadRequest();
        
        var result = await _mediator.Send(command);
        if (!result) return NotFound();
        
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteCompanyCommand(id));
        if (!result) return NotFound();
        
        return NoContent();
    }
}
