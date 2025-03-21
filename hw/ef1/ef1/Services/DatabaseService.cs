namespace ef1.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using ef1.Data;
using ef1.Models;
using Microsoft.EntityFrameworkCore;


    public class DatabaseService
    {
        private readonly AppDbContext _context;

        public DatabaseService(AppDbContext context)
        {
            _context = context;
        }

        // Получение списка всех автомобилей
        public List<Car> GetAllCars()
        {
            return _context.Cars.ToList();
        }

        // Добавление нового автомобиля
        public void AddCar(Car car)
        {
            _context.Cars.Add(car);
            _context.SaveChanges();
            Console.WriteLine($" Car {car.Brand} {car.Model} added.");
        }

        // Обновление автомобиля
        public void UpdateCar(int carId, decimal newPrice)
        {
            var car = _context.Cars.Find(carId);
            if (car != null)
            {
                car.Price = newPrice;
                _context.SaveChanges();
                Console.WriteLine($" Car price {car.Brand} {car.Model} updated.");
            }
        }

        // Удаление автомобиля
        public void DeleteCar(int carId)
        {
            var car = _context.Cars.Find(carId);
            if (car != null)
            {
                _context.Cars.Remove(car);
                _context.SaveChanges();
                Console.WriteLine($" Car {car.Brand} {car.Model} deleted.");
            }
        }

        // Получение списка клиентов
        public List<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }

        // Получение списка всех продаж
        public List<Sale> GetAllSales()
        {
            return _context.Sales.Include(s => s.Car).Include(s => s.Customer).Include(s => s.Employee).ToList();
        }

        //  Добавление продажи
        public void AddSale(int carId, int customerId, int employeeId)
        {
            var sale = new Sale
            {
                CarId = carId,
                CustomerId = customerId,
                EmployeeId = employeeId,
                SaleDate = DateTime.Now
            };

            _context.Sales.Add(sale);
            _context.SaveChanges();
            Console.WriteLine($" Selling a car ID {carId} customer ID {customerId} registered.");
        }
    }
