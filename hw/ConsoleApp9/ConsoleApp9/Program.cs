using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using ConsoleApp9.Data;
using ConsoleApp9.Models;


class Program
{
    static void Main()
    {
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        string connectionString = configuration.GetConnectionString("DefaultConnection");

        var carRepo = new CarRepository(connectionString);

        var newCar = new Car { Brand = "Toyota", Model = "Corolla", Year = 2022, Price = 20000m };
        int carId = carRepo.AddCar(newCar);
        Console.WriteLine($"Добавлен автомобиль с ID: {carId}");
        
        bool isUpdated = carRepo.UpdateCarPrice(carId, 18000m);
        Console.WriteLine(isUpdated ? "Цена обновлена" : "Обновление не удалось");
        
        bool isDeleted = carRepo.DeleteCar(carId);
        Console.WriteLine(isDeleted ? "Автомобиль удален" : "Удаление не удалось");
        
        var cars = carRepo.GetAllCars();
        Console.WriteLine("Список автомобилей:");
        foreach (var car in cars)
        {
            Console.WriteLine($"{car.Id}: {car.Brand} {car.Model}, {car.Year}, ${car.Price}");
        }
        
        string brandFilter = "Toyota";
        var filteredCars = carRepo.GetCarsByBrand(brandFilter);
        Console.WriteLine($"Автомобили бренда {brandFilter}:");
        foreach (var car in filteredCars)
        {
            Console.WriteLine($"{car.Id}: {car.Brand} {car.Model}, {car.Year}, ${car.Price}");
        }
    }
}


