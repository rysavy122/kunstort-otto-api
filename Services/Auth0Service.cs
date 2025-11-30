using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using App.Interfaces;

public class Auth0Service : IAuth0Service
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<Auth0Service> _logger;

    public Auth0Service(HttpClient httpClient, IConfiguration configuration, ILogger<Auth0Service> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> GetManagementApiTokenAsync()
    {
        var domain = _configuration["Auth0:ManagementApi:Domain"];
        var clientId = _configuration["Auth0:ManagementApi:ClientId"];
        var clientSecret = _configuration["Auth0:ManagementApi:ClientSecret"];
        var audience = _configuration["Auth0:ManagementApi:Audience"];

        _logger.LogInformation("Requesting Auth0 Management API token");

        var url = $"https://{domain}/oauth/token";

        var body = new
        {
            client_id = clientId,
            client_secret = clientSecret,
            audience = audience,
            grant_type = "client_credentials"
        };

        var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to get Auth0 Management API token. Status Code: {StatusCode}, Response: {Response}", response.StatusCode, await response.Content.ReadAsStringAsync());
            return null;
        }

        var result = await response.Content.ReadAsStringAsync();
        dynamic json = JsonConvert.DeserializeObject(result);
        _logger.LogInformation("Successfully obtained Auth0 Management API token");
        return json.access_token;
    }
}
