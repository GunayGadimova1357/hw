using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using ef1.Models;
using System;

namespace ef1.Data;

    public class AppDbContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<ServiceHistory> ServiceHistories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Добавляем сотрудников
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, Name = "Олег Сидоров", Position = "Менеджер" },
                new Employee { Id = 2, Name = "Мария Алексеева", Position = "Менеджер" }
            );

            // Добавляем 10 клиентов
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, Name = "Иван Иванов", Email = "ivan@test.com", Phone = "111-111" },
                new Customer { Id = 2, Name = "Ольга Петрова", Email = "olga@test.com", Phone = "222-222" },
                new Customer { Id = 3, Name = "Алексей Смирнов", Email = "alexey@test.com", Phone = "333-333" },
                new Customer { Id = 4, Name = "Елена Козлова", Email = "elena@test.com", Phone = "444-444" },
                new Customer { Id = 5, Name = "Дмитрий Васильев", Email = "dmitry@test.com", Phone = "555-555" },
                new Customer { Id = 6, Name = "Анна Михайлова", Email = "anna@test.com", Phone = "666-666" },
                new Customer { Id = 7, Name = "Сергей Кузнецов", Email = "sergey@test.com", Phone = "777-777" },
                new Customer { Id = 8, Name = "Марина Попова", Email = "marina@test.com", Phone = "888-888" },
                new Customer { Id = 9, Name = "Артем Соколов", Email = "artem@test.com", Phone = "999-999" },
                new Customer { Id = 10, Name = "Юлия Николаева", Email = "yulia@test.com", Phone = "101-101" }
            );

            // Добавляем 5 автомобилей
            modelBuilder.Entity<Car>().HasData(
                new Car { Id = 1, Brand = "Toyota", Model = "Camry", Year = 2022, Price = 2500000 },
                new Car { Id = 2, Brand = "BMW", Model = "X5", Year = 2021, Price = 4500000 },
                new Car { Id = 3, Brand = "Mercedes", Model = "C-Class", Year = 2023, Price = 3500000 },
                new Car { Id = 4, Brand = "Audi", Model = "A6", Year = 2022, Price = 3700000 },
                new Car { Id = 5, Brand = "Ford", Model = "Focus", Year = 2020, Price = 1800000 }
            );

            // Добавляем 5 продаж (связи клиент-авто-сотрудник)
            modelBuilder.Entity<Sale>().HasData(
                new Sale { Id = 1, CarId = 1, CustomerId = 1, EmployeeId = 1, SaleDate = new DateTime(2024, 3, 1) },
                new Sale { Id = 2, CarId = 2, CustomerId = 2, EmployeeId = 2, SaleDate = new DateTime(2024, 3, 5) },
                new Sale { Id = 3, CarId = 3, CustomerId = 3, EmployeeId = 1, SaleDate = new DateTime(2024, 3, 10) },
                new Sale { Id = 4, CarId = 4, CustomerId = 4, EmployeeId = 2, SaleDate = new DateTime(2024, 3, 15) },
                new Sale { Id = 5, CarId = 5, CustomerId = 5, EmployeeId = 1, SaleDate = new DateTime(2024, 3, 20) }
            );
        }
    }
