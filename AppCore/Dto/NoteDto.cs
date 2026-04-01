using AppCore.Entities;
using AppCore.Enums;

public record NoteDto
{
    public Guid Id { get; init; }
    public string Content { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;

    public static NoteDto FromEntity(Note note)
    {
        return new NoteDto
        {
            Id = note.Id,
            Content = note.Content,
            CreatedAt = note.CreatedAt,
            CreatedBy = note.CreatedBy
        };
    }
}

public record CreateNoteDto(string Content);