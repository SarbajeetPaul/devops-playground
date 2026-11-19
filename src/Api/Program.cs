var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello from DevOps Playground!");
app.MapGet("/health", () => Results.Ok(new { status = "Looks healthy brother!" }));

app.MapGet("/version", () => new { version = "0.1.0" });

app.Run();