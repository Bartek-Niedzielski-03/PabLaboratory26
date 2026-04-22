using AppCore.Dto;
using AppCore.Services;
using AppCore.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("/api/contacts")]
public class ContactsController : ControllerBase
{
    private readonly IPersonService _service;

    public ContactsController(IPersonService service)
    {
        _service = service;
    }

    // GET /api/contacts
    [HttpGet]
    [Authorize(Policy = nameof(CrmPolicies.ReadOnlyAccess))]
    public async Task<IActionResult> GetAllPersons([FromQuery] int page = 1, [FromQuery] int size = 20)
    {
        var result = await _service.FindAllPeoplePaged(page, size);
        return Ok(result);
    }

    // GET /api/contacts/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPerson(Guid id)
    {
        var person = await _service.GetById(id);

        if (person is null)
            return NotFound();

        return Ok(person);
    }

    // POST /api/contacts
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePersonDto dto)
    {
        var result = await _service.AddPerson(dto);

        return CreatedAtAction(
            nameof(GetPerson),
            new { id = result.Id },
            result
        );
    }

    // PUT /api/contacts/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePersonDto dto)
    {
        var existing = await _service.GetById(id);

        if (existing is null)
            return NotFound();

        await _service.UpdatePerson(id, dto);

        var updated = await _service.GetById(id);

        return Ok(updated);
    }
    
    [HttpPost("{contactId:guid}/notes")]
    [ProducesResponseType(typeof(NoteDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddNote(
        [FromRoute] Guid contactId,
        [FromBody] CreateNoteDto dto)
    {
        var note = await _service.AddNoteToPerson(contactId, dto);

        return CreatedAtAction(
            nameof(GetNotes),
            new { contactId },
            NoteDto.FromEntity(note));
    }

    [HttpGet("{contactId:guid}/notes")]
    [ProducesResponseType(typeof(IEnumerable<NoteDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetNotes([FromRoute] Guid contactId)
    {
        var person = await _service.GetPerson(contactId);
        return Ok(person.Notes);
    }

    [HttpDelete("{contactId:guid}/notes/{noteId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteNote(
        [FromRoute] Guid contactId,
        [FromRoute] Guid noteId)
    {
        await _service.DeleteNoteFromPerson(contactId, noteId);
        return NoContent();
    }
}