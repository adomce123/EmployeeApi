using EmployeeApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeApi.Infrastructure
{
    public class EmployeesDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public EmployeesDbContext(DbContextOptions<EmployeesDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasOne(e => e.Boss) // Employee can have one boss
                      .WithMany() // Boss can have many Employees
                      .HasForeignKey(e => e.BossId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Role)
                .IsUnique()
                .HasFilter("[Role] = 'Ceo'");

            modelBuilder.Entity<Employee>().HasData(
            new Employee
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                HomeAddress = "123 Main St",
                Role = "Ceo",
                Birthdate = new DateTime(1980, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                EmploymentDate = new DateTime(2010, 1, 1, 1, 1, 1, DateTimeKind.Utc),
                CurrentSalary = 200000,
                BossId = null
            },
            new Employee
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Smith",
                HomeAddress = "456 Maple Ave",
                Role = "Manager",
                Birthdate = new DateTime(1985, 5, 15, 1, 0, 0, 0, DateTimeKind.Utc),
                EmploymentDate = new DateTime(2012, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                CurrentSalary = 150000,
                BossId = 1
            },
            new Employee
            {
                Id = 3,
                FirstName = "Bob",
                LastName = "Johnson",
                HomeAddress = "789 Elm St",
                Role = "Developer",
                Birthdate = new DateTime(1990, 8, 20, 0, 0, 0, DateTimeKind.Utc),
                EmploymentDate = new DateTime(2015, 7, 1, 0, 0, 0, DateTimeKind.Utc),
                CurrentSalary = 100000,
                BossId = 2
            }
        );
        }
    }
}
