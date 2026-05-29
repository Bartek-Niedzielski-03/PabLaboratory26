using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace PabLaboratory26Tests;

public class PolymorphicContactsIntegrationTests : IClassFixture<WebApplicationFactory<WebApi.Program>>
{
    private readonly HttpClient _client;

    public PolymorphicContactsIntegrationTests(WebApplicationFactory<WebApi.Program> factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<string> LoginAndGetTokenAsync()
    {
        var loginDto = new { email = "sales@crm.local", password = "Sales@123!" };
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
        if (!response.IsSuccessStatusCode) return string.Empty;
        var content = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(content);
        return doc.RootElement.GetProperty("accessToken").GetString() ?? string.Empty;
    }

    private void SetAuthHeader(string token)
    {
        _client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }

    [Fact]
    public async Task GetAllContacts_WithoutToken_Returns401()
    {
        _client.DefaultRequestHeaders.Authorization = null;
        var response = await _client.GetAsync("/api/contacts/poly");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreatePerson_WithValidData_Returns201()
    {
        var token = await LoginAndGetTokenAsync();
        if (string.IsNullOrEmpty(token)) return;
        SetAuthHeader(token);

        var dto = new
        {
            contactType = "Person",
            firstName = "Jan",
            lastName = "Testowy",
            email = $"jan.testowy.{Guid.NewGuid()}@test.pl",
            phone = "48111222333",
            gender = "Male"
        };

        var response = await _client.PostAsJsonAsync("/api/contacts/poly", dto);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(content);
        Assert.Equal("Jan", doc.RootElement.GetProperty("firstName").GetString());
    }

    [Fact]
    public async Task CreateCompany_WithValidData_Returns201()
    {
        var token = await LoginAndGetTokenAsync();
        if (string.IsNullOrEmpty(token)) return;
        SetAuthHeader(token);

        var dto = new
        {
            contactType = "Company",
            name = "Test Corp",
            email = $"test.{Guid.NewGuid()}@corp.pl",
            phone = "48999888777",
            nip = "9876543210",
            industry = "IT"
        };

        var response = await _client.PostAsJsonAsync("/api/contacts/poly", dto);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task CreateOrganization_WithValidData_Returns201()
    {
        var token = await LoginAndGetTokenAsync();
        if (string.IsNullOrEmpty(token)) return;
        SetAuthHeader(token);

        var dto = new
        {
            contactType = "Organization",
            name = "Fundacja Test",
            email = $"test.{Guid.NewGuid()}@fundacja.pl",
            phone = "48777666555",
            type = "Foundation",
            mission = "Testowanie aplikacji."
        };

        var response = await _client.PostAsJsonAsync("/api/contacts/poly", dto);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetContactById_NonExisting_Returns404()
    {
        var token = await LoginAndGetTokenAsync();
        if (string.IsNullOrEmpty(token)) return;
        SetAuthHeader(token);

        var response = await _client.GetAsync($"/api/contacts/poly/{Guid.NewGuid()}");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}