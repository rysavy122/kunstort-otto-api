using App.Data;
using App.Interfaces;
using App.Hubs;
using App.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//var port = Environment.GetEnvironmentVariable("PORT") ?? "6060";
//builder.WebHost.UseUrls($"http://localhost:{port}");

// SignalR 
builder.Services.AddSignalR();

// Register the Azure Blob Storage service with the DI container.
builder.Services.AddSingleton<IAzureBlobStorageService, AzureBlobStorageService>();


// Register other services with the DI container.
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IForschungsfrageService, ForschungsfrageService>();
builder.Services.AddScoped<IPlakatService, PlakatService>();
builder.Services.AddScoped<IKommentarService, KommentarService>();
builder.Services.AddScoped<ICommentPositionService, CommentPositionService>();
builder.Services.AddScoped<IMediaPositionService, MediaPositionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddHttpClient<IAuth0Service, Auth0Service>();


// Configuration for Azure Blob Storage
builder.Services.AddSingleton<IAzureBlobStorageService>(provider => new AzureBlobStorageService(
    provider.GetRequiredService<ILogger<AzureBlobStorageService>>(),
    provider.GetRequiredService<IConfiguration>()
));


// Database configuration
var cs =
    builder.Configuration.GetConnectionString("OttoDatabaseTidb")
    ?? builder.Configuration.GetConnectionString("OttoDatabase");

if (string.IsNullOrWhiteSpace(cs))
    throw new Exception("No connection string configured (OttoDatabaseTidb / OttoDatabase).");

builder.Services.AddDbContext<OttoDbContext>(options =>
    options.UseMySql(cs, new MySqlServerVersion(new Version(8, 0, 0))));

// CORS configuration
var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

var clientOriginUrl = builder.Configuration.GetValue<string>("CLIENT_ORIGIN_URL") ?? "https://localhost:4040";

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(clientOriginUrl)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .SetPreflightMaxAge(TimeSpan.FromSeconds(86400));
    });
});


// Authentication configuration using Auth0 settings
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://{builder.Configuration["Auth0:CustomApi:Domain"]}/";
        options.Audience = builder.Configuration["Auth0:CustomApi:Audience"];
    });

builder.Services.AddControllers();

var app = builder.Build();

// Initialize Azure Blob Service (if necessary) and potentially list container blobs
var azureBlobService = app.Services.GetRequiredService<IAzureBlobStorageService>();
await azureBlobService.ListContainerBlobsAsync();

// --- NEW: log which DB we are connected to ---
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OttoDbContext>();

    var conn = db.Database.GetDbConnection();
    app.Logger.LogInformation(
        "Database connection: DataSource={DataSource}, Database={Database}, ConnectionString={ConnectionString}",
        conn.DataSource,
        conn.Database,
        conn.ConnectionString
    );

    try
    {
        var ffCount = await db.Forschungsfragen!.CountAsync();
        app.Logger.LogInformation("Forschungsfragen row count: {Count}", ffCount);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "Error when querying Forschungsfragen for startup test.");
    }
}
// --- END NEW ---


// Log the application startup environment
app.Logger.LogInformation($"Application starting in environment: {app.Environment.EnvironmentName}");

// CLIENT ORIGIN URL 
app.Logger.LogInformation($"CLIENT ORIGIN URL: {clientOriginUrl}");

// Middleware and application configuration
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

// Ensure SignalR explicitly uses the default policy
app.MapHub<NotificationHub>("/hubs/notification").RequireCors(cors => cors
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins(clientOriginUrl)
);
app.MapControllers();
app.Run();
