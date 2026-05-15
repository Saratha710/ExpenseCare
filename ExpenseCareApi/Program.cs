using System;
using Microsoft.EntityFrameworkCore;
using ExpenseCareApi.Infrastructure.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using System.Text.Json;
using ExpenseCareApi.Application.Services;
using ExpenseCareApi.Infrastructure.Repositories;
using ExpenseCareApi.Core.Interfaces;
using ExpenseCareApi.Core.Validators;
using ExpenseCareApi.Core.Mapping;
using AutoMapper;
using ExpenseCareApi.Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins(
            "http://localhost:4200",
            "https://wonderful-glacier-0420dae00.7.azurestaticapps.net",
            "https://wonderful-glacier-0420dae00-preview.eastasia.7.azurestaticapps.net"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    { options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// builder.Services.AddDbContext<ExpenseCareDbContext>(options =>
//       options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<ExpenseCareDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null
        )
    ));

builder.Services.AddHttpClient<SmsService>();

builder.Services.AddAutoMapper(typeof(Program).Assembly); 
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateDonationValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateDonationValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateExpenseValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateExpenseValidator>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IDonationRepository, DonationRepository>();
builder.Services.AddScoped<IDonationService, DonationService>();

builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IExpenseService,    ExpenseService>();

builder.Services.AddAutoMapper(typeof(DonationDetailsMapping).Assembly);
builder.Services.AddAutoMapper(typeof(ExpenseDetailsMapping).Assembly);

builder.Services.AddScoped<IUpiSettingsRepository, UpiSettingsRepository>();

builder.Services.AddScoped<IUpiSettingsService, UpiSettingsService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,  // ← checks expiry automatically
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
 app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAngular");
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ExpenseCareDbContext>();
    context.Database.Migrate();
    
    var existingUser = context.Users.FirstOrDefault(u => u.UserName == "testuser");
    if (existingUser == null)
    {
        context.Users.Add(new User
        {
            Name         = "Test Donor",
            MobileNumber = "9999999999",
            UserName     = "testuser",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test@123"),
            Role         = "User"
        });
        context.SaveChanges();
        Console.WriteLine("Test user seeded!");
    }
}

app.Run();
