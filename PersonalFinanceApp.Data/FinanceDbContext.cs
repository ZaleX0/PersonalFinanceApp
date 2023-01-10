using Microsoft.EntityFrameworkCore;
using PersonalFinanceApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceApp.Data;
public class FinanceDbContext : DbContext
{
	public FinanceDbContext(DbContextOptions options) : base(options)
	{
	}

	public DbSet<User> Users { get; set; }
	public DbSet<IncomeCategory> IncomeCategories { get; set; }
	public DbSet<ExpenseCategory> ExpenseCategories { get; set; }
	public DbSet<Income> Incomes { get; set; }
	public DbSet<Expense> Expenses { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
        modelBuilder.Entity<Income>().Property(i => i.Price).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Expense>().Property(e => e.Price).HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Income>().HasOne(i => i.Category).WithMany().HasForeignKey(i => i.CategoryId);
        modelBuilder.Entity<Expense>().HasOne(e => e.Category).WithMany().HasForeignKey(e => e.CategoryId);

		//modelBuilder.Entity<User>()
		//	.HasMany(u => u.Incomes)
		//	.WithOne(i => i.User)
		//	.HasForeignKey(i => i.UserId);
    }
}
