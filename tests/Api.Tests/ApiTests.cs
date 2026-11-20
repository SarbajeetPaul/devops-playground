using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

public class ApiTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Health_returns_200()
    {
        var res = await _client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task Posted_note_can_be_read_back()
    {
        var post = await _client.PostAsJsonAsync("/notes", new { text = "ci note" });
        Assert.Equal(HttpStatusCode.Created, post.StatusCode);

        var notes = await _client.GetFromJsonAsync<List<NoteDto>>("/notes");
        Assert.Contains(notes!, n => n.Text == "ci note");
    }

    [Fact]
    public async Task Empty_note_is_rejected()
    {
        var res = await _client.PostAsJsonAsync("/notes", new { text = "" });
        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    private record NoteDto(int Id, string Text);
}