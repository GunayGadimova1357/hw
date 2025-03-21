using System;
using System.Linq;
using ef1.Data;
using ef1.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

class Program
{
    static void Main()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        string connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Error: Connection string not loaded!");
        }

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        using var context = new AppDbContext(optionsBuilder.Options);
        
        context.Database.Migrate();

        var dbService = new DatabaseService(context);

        // Демонстрация CRUD-операций
        Console.WriteLine("\nAll cars in stock:");
        foreach (var car in dbService.GetAllCars())
        {
            Console.WriteLine($"{car.Id}: {car.Brand} {car.Model} ({car.Year}) - {car.Price} $.");
        }

        //  Добавление автомобиля
        dbService.AddCar(new ef1.Models.Car { Brand = "Honda", Model = "Civic", Year = 2022, Price = 2200000 });

        // Обновление автомобиля
        dbService.UpdateCar(1, 2800000);

        //  Удаление автомобиля
        dbService.DeleteCar(5);

        //  Добавление продажи
        dbService.AddSale(2, 1, 1);

        //  Вывод списка продаж
        Console.WriteLine("\n Sales list:");
        foreach (var sale in dbService.GetAllSales())
        {
            Console.WriteLine($"Sale ID {sale.Id}: {sale.Car.Brand} {sale.Car.Model} → {sale.Customer.Name}, manager: {sale.Employee.Name}");
        }

        //  LINQ-запросы
        Console.WriteLine("\n Executing LINQ queries:");

        // Найти все автомобили, купленные конкретным клиентом (например, ID 1)
        var customerId = 1;
        var customerCars = context.Sales
            .Where(s => s.CustomerId == customerId)
            .Select(s => s.Car)
            .ToList();

        Console.WriteLine($" Cars purchased by client ID {customerId}:");
        foreach (var car in customerCars)
        {
            Console.WriteLine($"{car.Brand} {car.Model}");
        }

        // Вывести список продаж за определенный период
        var startDate = new DateTime(2024, 3, 1);
        var endDate = new DateTime(2024, 3, 31);
        var salesInPeriod = context.Sales
            .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
            .ToList();

        Console.WriteLine($"\n Sales for the period{startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}:");
        foreach (var sale in salesInPeriod)
        {
            Console.WriteLine($"Sale ID {sale.Id}: {sale.Car.Brand} {sale.Car.Model} → {sale.Customer.Name}");
        }

        // Подсчитать количество продаж каждого менеджера
        var salesCount = context.Sales
            .GroupBy(s => s.Employee.Name)
            .Select(g => new { Manager = g.Key, SalesCount = g.Count() })
            .ToList();

        Console.WriteLine("\n Number of sales by managers:");
        foreach (var sale in salesCount)
        {
            Console.WriteLine($"{sale.Manager}: {sale.SalesCount} sale(s)");
        }
    }
}