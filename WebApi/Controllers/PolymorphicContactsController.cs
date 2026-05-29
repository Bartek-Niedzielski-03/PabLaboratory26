using System.Security.Claims;
using AppCore.Authorization;
using AppCore.Dto;
using AppCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/contacts/poly")]
public class PolymorphicContactsController : ControllerBase
{
    private readonly IContactService _service;

    public PolymorphicContactsController(IContactService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Policy = nameof(CrmPolicies.ReadOnlyAccess))]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int size = 20)
    {
        var result = await _service.GetAllContactsPagedAsync(page, size);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize(Policy = nameof(CrmPolicies.ReadOnlyAccess))]
    public async Task<IActionResult> GetById(Guid id)
    {
        var contact = await _service.GetContactByIdAsync(id);
        if (contact is null)
            return NotFound();
        return Ok(contact);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateContactDto? dto)
    {
        if (dto is null)
            return BadRequest("Nieprawidłowe dane kontaktu.");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _service.AddContactAsync(dto, userId);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateContactDto dto)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _service.UpdateContactAsync(id, dto, userId);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _service.DeleteContactAsync(id, userId);
            return NoContent();
        }
        catch (UnauthorizedAccessException)
        {
            return Forbid();
        }
    }
}