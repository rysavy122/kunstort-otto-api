using App.Data;
using App.Interfaces;
using App.Hubs;
using App.Middlewares;
using App.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "6060";


builder.WebHost.UseUrls($"http://localhost:{port}");


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
var azureBlobConfig = builder.Configuration.GetSection("AzureStorage");
builder.Services.AddSingleton(_ => new BlobServiceClient(azureBlobConfig["ConnectionString"]));

builder.Services.AddSingleton<IAzureBlobStorageService>(provider => new AzureBlobStorageService(
    provider.GetRequiredService<ILogger<AzureBlobStorageService>>(),
    provider.GetRequiredService<IConfiguration>()
));


// Database configuration
builder.Services.AddDbContext<OttoDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("OttoDatabase"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("OttoDatabase"))));

// CORS configuration
var clientOriginUrl = builder.Configuration.GetValue<string>("CLIENT_ORIGIN_URL") ?? "http://localhost:4040";
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(clientOriginUrl)
              .AllowAnyHeader()
              .AllowAnyMethod()
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

// Log the application startup environment
app.Logger.LogInformation($"Application starting in environment: {app.Environment.EnvironmentName}");

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

app.MapHub<NotificationHub>("/hubs/notification");

app.MapControllers();
app.Run();
