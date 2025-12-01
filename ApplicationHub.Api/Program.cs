using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using ApplicationHub.Api;
using ApplicationHub.Api.Middlewares;
using ApplicationHub.Data;
using ApplicationHub.Data.Mappings;
using ApplicationHub.Modules.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddScoped<IDbOption, DbOption>();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<IUserMeta, UserMeta>();
builder.Services.AddControllers();
builder.Services.ConfigureServices();

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    x.IncludeXmlComments(xmlPath);
    // --- Add JWT Auth to Swagger ---

    x.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter your JWT token here:  \n**Example:** `Bearer eyJhbGciOi...`"
    });

    x.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configure Cookie Authentication
var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(builder.Configuration["JwtKey"]!));
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.SaveToken = true;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = false,
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = key,
            ClockSkew = TimeSpan.Zero
        };
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromHours(10);
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = ctx =>
            {
                ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = ctx =>
            {
                ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            }
        };
    });
// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("ApplicationHubPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

MapsterMappings.RegisterMappings();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Ensure database and tables are created
    if (context.Database.EnsureCreated())
    {
        // Only runs if the database was just created
        context.Users.Add(new User()
        {
            Id = Guid.NewGuid(),
            Email = "test@gmail.com",
            Password = "cscdU/eM5elfDLf2UmKTUr+ka5TSJZJuYu6yL021MGA=",
            FirstName = "Test",
            LastName = "Test",
            CurrentCompany = "Test"
        });
        await context.SaveChangesAsync();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        const string swaggerUrl = "https://cdnjs.cloudflare.com/ajax/libs/swagger-ui/5.17.14/swagger-ui-bundle.js";
        x.InjectJavascript(swaggerUrl,
            "text/javascript");
        x.SwaggerEndpoint("/swagger/v1/swagger.json", "Application Hub API");
    });
}

app.UseCors("ApplicationHubPolicy");
app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseAuthentication();
app.UseMiddleware<UserMetaMiddleware>();
app.UseAuthorization();
app.MapControllers();
app.Run();