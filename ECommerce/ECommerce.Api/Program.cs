using ECommerce.Application.Common.Behaviors;
using ECommerce.Application.Contracts.Persistence;
using ECommerce.Application.Contracts.Security;
using ECommerce.Application.Features.Auth.Commands;
using ECommerce.Application.Features.Products.Commands;
using ECommerce.Application.Integrations.PaymentService;

using ECommerce.Domain.Entities;

using ECommerce.Infrastructure.Security;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Persistence.Repositories;
using ECommerce.Infrastructure.Integrations.PaymentService;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using FluentValidation;
using System.Text;
using MediatR;


var builder = WebApplication.CreateBuilder(args);



// ==========================================
// 1. CAPA DE PRESENTACIÓN
// ==========================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando el esquema Bearer. Escribí 'Bearer' [espacio] y pegá tu token. Ejemplo: 'Bearer eyJhbGci...'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});



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
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

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

builder.Services.AddHttpClient<IPaymentClient, PaymentClient>(client =>
{
    var paymentServiceUrl = builder.Configuration["Services:Payment"] ?? "https://localhost:5123";
    
    client.BaseAddress = new Uri(paymentServiceUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
});

var app = builder.Build();



// ==========================================
// 5. SEED DE USUARIO ADMINISTRADOR
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var passwordHasher = services.GetRequiredService<IPasswordHasher>();

        context.Database.Migrate();

        var adminExists = context.Set<User>().Any(u => u.Role == "Admin");

        if (!adminExists)
        {
            var adminUser = User.Create(
                email: "admin@gmail.com",
                name: "Administrador",
                passwordHash: passwordHasher.Hash("Admin123"),
                role: "Admin"
            );

            context.Set<User>().Add(adminUser);
            context.SaveChanges();
            
            Console.WriteLine("Usuario Administrador creado exitosamente.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ocurrió un error al inicializar la base de datos: {ex.Message}");
    }
}



// ==========================================
// 6. MIDDLEWARES DEL PIPELINE HTTP
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