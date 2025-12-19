using Scalar.AspNetCore; 
using CliniCore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.IdentityModel.Tokens;                
using System.Text;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Configura la licencia Community
QuestPDF.Settings.License = LicenseType.Community;


// Configuración de Base de Datos
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// CONFIGURACIÓN DE SEGURIDAD JWT 
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings.GetValue<string>("Key")!);

builder.Services.AddAuthentication(options =>
{
    
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
        ValidateAudience = true,
        ValidAudience = jwtSettings.GetValue<string>("Audience"),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});


//-------------------------------------------------------------------


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<CliniCore.API.Services.DataSeederService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins(
                  "http://localhost:5086",  // <--- El puerto HTTP de tu Frontend
                  "https://localhost:5086"  // <--- El puerto HTTPS 
              )
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{    
    
     // app.UseSwagger();
     // app.UseSwaggerUI();

    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

// 2. ACTIVAR EL MIDDLEWARE (Justo aquí)
app.UseCors("AllowBlazorClient");

//ACTIVAR SEGURIDAD
app.UseAuthentication(); //  Verifica el Token
app.UseAuthorization();  //  Verifica permisos
app.MapControllers();
//app.UseHttpsRedirection();

app.Run();

