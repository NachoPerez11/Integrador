using ECommerce.Application.Common.Behaviors;
using ECommerce.Application.Contracts.Persistence;
using ECommerce.Application.Contracts.Security;
using ECommerce.Application.Features.Auth.Commands;
using ECommerce.Application.Features.Products.Commands;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Persistence.Repositories;
using ECommerce.Infrastructure.Security;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. CAPA DE PRESENTACIÓN
// ==========================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ==========================================
// 2. CAPA DE APLICACIÓN
// ==========================================
// Registramos MediatR buscando los Handlers en el proyecto de Aplicación
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly));

// Registramos todas las reglas de FluentValidation
builder.Services.AddTransient<IValidator<RegisterUserCommand>, RegisterUserCommandValidator>();
builder.Services.AddTransient<IValidator<CreateProductCommand>, CreateProductCommandValidator>();

// Agregamos el Behavior para que las validaciones corran automáticamente
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// ==========================================
// 3. CAPA DE INFRAESTRUCTURA
// ==========================================
// Configuración de Entity Framework Core con SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Mapeo de Contratos a Implementaciones
builder.Services.AddScoped<IUserRepository, UserRepository>();  
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();
builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

// ==========================================
// 4. CONFIGURACIÓN DE SEGURIDAD (JWT)
// ==========================================
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Secret"]!))
        };
    });

var app = builder.Build();

// ==========================================
// 5. MIDDLEWARES DEL PIPELINE HTTP
// ==========================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();