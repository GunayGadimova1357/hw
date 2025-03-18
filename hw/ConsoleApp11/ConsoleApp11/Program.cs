using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using ConsoleApp11.Data;
using ConsoleApp11.Models;

namespace ConsoleApp11;

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
            throw new InvalidOperationException("Error: The connection string was not loaded.");
        }
        
        var carRepo = new CarRepository(connectionString);
        
        var newCar = new Car { Brand = "Toyota", Model = "Corolla", Year = 2022, Price = 20000m };
        carRepo.AddCarWithOwner(newCar, "John Smith");
        
        bool updated = carRepo.UpdateOwner(1, "Michael Williams");
        Console.WriteLine(updated ? "Owner updated." : "Owner update failed.");
        
        var cars = carRepo.GetAllCarsWithOwners();
        foreach (var car in cars)
        {
            Console.WriteLine($"{car.Brand} {car.Model}, {car.Year}, ${car.Price} - Owner: {car.OwnerName}");
        }
    }
}