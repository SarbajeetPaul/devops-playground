using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("ConnectionStrings__Default is not set");
builder.Services.AddDbContext<AppDb>(o => o.UseNpgsql(connectionString));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AppDb>().Database.EnsureCreated();
}

app.MapGet("/", () => "Hello from DevOps Playground");
app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));
app.MapGet("/version", () => new { version = "0.1.0" });

app.MapGet("/notes", async (AppDb db) => await db.Notes.ToListAsync());

app.MapPost("/notes", async (NoteInput input, AppDb db) =>
{
    if (string.IsNullOrWhiteSpace(input.Text))
        return Results.BadRequest(new { error = "text is required" });

    var note = new Note { Text = input.Text };
    db.Notes.Add(note);
    await db.SaveChangesAsync();
    return Results.Created($"/notes/{note.Id}", note);
});

app.Run();