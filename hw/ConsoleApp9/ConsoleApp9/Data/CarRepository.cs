using ConsoleApp9.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using  Microsoft.Data.SqlClient;

namespace ConsoleApp9.Data;

public class CarRepository
{
    private readonly string _connectionString;

    public CarRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public int AddCar(Car car)
    {
        using var connection = new SqlConnection(_connectionString);
        string sql = "INSERT INTO Cars (Brand, Model, Year, Price) VALUES (@Brand, @Model, @Year, @Price); SELECT CAST(SCOPE_IDENTITY() as int)";
        return connection.ExecuteScalar<int>(sql, car);
    }
    
    public bool UpdateCarPrice(int carId, decimal newPrice)
    {
        using var connection = new SqlConnection(_connectionString);
        string sql = "UPDATE Cars SET Price = @NewPrice WHERE Id = @CarId";
        return connection.Execute(sql, new { CarId = carId, NewPrice = newPrice }) > 0;
    }
    
    public bool DeleteCar(int carId)
    {
        using var connection = new SqlConnection(_connectionString);
        string sql = "DELETE FROM Cars WHERE Id = @CarId";
        return connection.Execute(sql, new { CarId = carId }) > 0;
    }
    
    public List<Car> GetAllCars()
    {
        using var connection = new SqlConnection(_connectionString);
        string sql = "SELECT * FROM Cars";
        return connection.Query<Car>(sql).ToList();
    }
    
    public List<Car> GetCarsByBrand(string brand)
    {
        using var connection = new SqlConnection(_connectionString);
        string sql = "SELECT * FROM Cars WHERE Brand = @BrandName";
        return connection.Query<Car>(sql, new { BrandName = brand }).ToList();
    }
}