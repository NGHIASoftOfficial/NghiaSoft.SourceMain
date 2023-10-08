using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using NghiaSoft.Core.IdentityServer;
using NghiaSoft.FileServerAPI.Data;
using NghiaSoft.FileServerAPI.Middlewares;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);
var idSvrConnector = builder.Configuration.GetSection("IdentityServer").Get<ApiClientConnector>();

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services
    .AddDbContext<ApplicationDbContext>(options => { options.UseSqlServer(connectionString); });

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsBuilder =>
    {
        corsBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// https://www.camiloterevinto.com/post/oauth-pkce-flow-for-asp-net-core-with-swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "File Server API", Version = "v1" });

    // Add OAuth2/OpenID Connect authentication to Swagger
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = HeaderNames.Authorization,
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri(idSvrConnector.AbsoluteAuthorizationUrl),
                TokenUrl = new Uri(idSvrConnector.AbsoluteTokenUrl),
                // RefreshUrl = new Uri(idSvrConnector.AbsoluteRefreshUrl),
                Scopes = idSvrConnector.Scopes.ToDictionary(x => x.Name, x => x.Description)
            }
        }
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                },
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.Authority = idSvrConnector.Authority;
        options.TokenValidationParameters.ValidateAudience = !string.IsNullOrEmpty(idSvrConnector.Audience);
        options.Audience = idSvrConnector.Audience;
        options.RequireHttpsMetadata = idSvrConnector.Authority.StartsWith("https://");
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // Configure Swagger UI to use JWT for authentication
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "File Server API V1");
        c.OAuthClientId(idSvrConnector.ClientId);
        c.OAuthClientSecret("P@ssw0rd@FileServerAPI");
        c.OAuthScopes(idSvrConnector.Scopes.Where(x => x.IsDefault).Select(x => x.Name).ToArray());
        c.OAuthUsePkce();
        c.DocExpansion(DocExpansion.None);
        // c.RoutePrefix = string.Empty; // Serve the Swagger UI at the root URL
    });
}


app.UseCors();
// app.UseHttpsRedirection();

app.UseAllowUploadValidatorMiddleware();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();