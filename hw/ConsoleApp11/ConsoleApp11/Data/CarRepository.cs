namespace ConsoleApp11.Data;

using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using ConsoleApp11.Models;

public class CarRepository
{
    private readonly string _connectionString;

    public CarRepository(string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ArgumentNullException(nameof(connectionString), "Error: Connection string must not be empty.");
        }

        _connectionString = connectionString;
    }
    
    public void AddCarWithOwner(Car car, string ownerName)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var transaction = connection.BeginTransaction();
        try
        {
            string insertCar = @"
                INSERT INTO Cars (Brand, Model, Year, Price) 
                VALUES (@Brand, @Model, @Year, @Price); 
                SELECT CAST(SCOPE_IDENTITY() as int)";
                
            int carId = connection.ExecuteScalar<int>(insertCar, car, transaction);

            string insertOwner = "INSERT INTO Owners (Name, CarId) VALUES (@Name, @CarId)";
            connection.Execute(insertOwner, new { Name = ownerName, CarId = carId }, transaction);

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.WriteLine($"Error adding car: {ex.Message}");
            throw;
        }
    }
    
    public bool UpdateOwner(int carId, string newOwner)
    {
        using var connection = new SqlConnection(_connectionString);
        string sql = "UPDATE Owners SET Name = @NewOwner WHERE CarId = @CarId";
        return connection.Execute(sql, new { CarId = carId, NewOwner = newOwner }) > 0;
    }
    
    public bool DeleteCar(int carId)
    {
        using var connection = new SqlConnection(_connectionString);
        string sql = @"
            DELETE FROM Owners WHERE CarId = @CarId;
            DELETE FROM Cars WHERE Id = @CarId;";
        return connection.Execute(sql, new { CarId = carId }) > 0;
    }

    public List<CarWithOwner> GetAllCarsWithOwners()
    {
        using var connection = new SqlConnection(_connectionString);
        string sql = @"
            SELECT c.Id, c.Brand, c.Model, c.Year, c.Price, o.Name AS OwnerName 
            FROM Cars c 
            INNER JOIN Owners o ON c.Id = o.CarId";
        return connection.Query<CarWithOwner>(sql).AsList();
    }
    
    public List<CarWithOwner> GetCarsByOwner(string ownerName)
    {
        using var connection = new SqlConnection(_connectionString);
        string sql = @"
            SELECT c.Id, c.Brand, c.Model, c.Year, c.Price, o.Name AS OwnerName 
            FROM Cars c 
            INNER JOIN Owners o ON c.Id = o.CarId 
            WHERE o.Name = @OwnerName";
        return connection.Query<CarWithOwner>(sql, new { OwnerName = ownerName }).AsList();
    }
}