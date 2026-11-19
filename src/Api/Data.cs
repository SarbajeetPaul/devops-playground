using Microsoft.EntityFrameworkCore;

public class Note
{
    public int Id { get; set; }
    public string Text { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public record NoteInput(string? Text);

public class AppDb(DbContextOptions<AppDb> options) : DbContext(options)
{
    public DbSet<Note> Notes => Set<Note>();
}