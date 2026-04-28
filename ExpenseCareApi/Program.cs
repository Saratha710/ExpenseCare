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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")  // your Angular dev URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    { options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

builder.Services.AddDbContext<ExpenseCareDbContext>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAngular");
//app.UseHttpsRedirection();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ExpenseCareDbContext>();
    
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
