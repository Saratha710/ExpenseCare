namespace ExpenseCareApi.Infrastructure.Data;

using System;
using Microsoft.EntityFrameworkCore;
using ExpenseCareApi.Core.Models;

public class ExpenseCareDbContext : DbContext
{
    public ExpenseCareDbContext(DbContextOptions<ExpenseCareDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<DonationDetails> Donations { get; set; }
    public DbSet<ExpenseDetails> Expenses { get; set; }
    public DbSet<UpiSettings> UpiSettings { get; set; }

}