using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using App.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IAuth0Service _auth0Service;
    private readonly ILogger<RolesController> _logger;

    public RolesController(IConfiguration configuration, IAuth0Service auth0Service, ILogger<RolesController> logger)
    {
        _configuration = configuration;
        _auth0Service = auth0Service;
        _logger = logger;
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] RoleAssignmentRequest request)
    {
        _logger.LogInformation("AssignRole called with UserId: {UserId} and RoleId: {RoleId}", request.UserId, request.RoleId);

        var token = await _auth0Service.GetManagementApiTokenAsync();

        if (token == null)
        {
            _logger.LogError("Unable to get Auth0 Management API token");
            return Unauthorized("Unable to get Auth0 Management API token");
        }

        var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var url = $"https://{_configuration["Auth0:Domain"]}/api/v2/users/{request.UserId}/roles";
        _logger.LogInformation("Sending request to Auth0 API: {Url}", url);

        var content = new StringContent(JsonConvert.SerializeObject(new { roles = new[] { request.RoleId } }), Encoding.UTF8, "application/json");
        var response = await client.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to assign role. Status Code: {StatusCode}, Response: {Response}", response.StatusCode, await response.Content.ReadAsStringAsync());
            return BadRequest("Failed to assign role");
        }

        _logger.LogInformation("Role assigned successfully");
        return Ok(new { message = "Role assigned successfully" });
    }

    public class RoleAssignmentRequest
    {
        public string RoleId { get; set; }
        public string UserId { get; set; }
    }
}
