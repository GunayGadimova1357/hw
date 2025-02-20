CREATE DATABASE CarDealership;
GO
USE CarDealership;
GO

CREATE TABLE Customers (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Phone NVARCHAR(20) NOT NULL
);

CREATE TABLE Cars (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Brand NVARCHAR(50) NOT NULL,
    Model NVARCHAR(50) NOT NULL,
    Year INT CHECK (Year >= 2000),
    Price DECIMAL(10,2) CHECK (Price > 0)
);

CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY(1,1),
    CustomerId INT FOREIGN KEY REFERENCES Customers(Id) ON DELETE CASCADE,
    CarId INT FOREIGN KEY REFERENCES Cars(Id) ON DELETE CASCADE,
    OrderDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE CarPriceHistory (
    Id INT PRIMARY KEY IDENTITY(1,1),
    CarId INT FOREIGN KEY REFERENCES Cars(Id) ON DELETE CASCADE,
    OldPrice DECIMAL(10,2),
    NewPrice DECIMAL(10,2),
    ChangeDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE DeletedOrdersLog (
    Id INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT,
    CustomerId INT,
    CarId INT,
    OrderDate DATETIME,
    DeletedAt DATETIME DEFAULT GETDATE()
);

INSERT INTO Customers (Name, Email, Phone) VALUES
('Иван Петров', 'ivan.petrov@email.com', '123-456-789'),
('Мария Сидорова', 'maria.sidorova@email.com', '987-654-321'),
('Алексей Смирнов', 'alex.smirnov@email.com', '555-666-777');

INSERT INTO Cars (Brand, Model, Year, Price) VALUES
('Toyota', 'Camry', 2022, 30000),
('BMW', 'X5', 2023, 60000),
('Mercedes', 'C-Class', 2021, 50000);

INSERT INTO Orders (CustomerId, CarId) VALUES
(1, 1),
(2, 2),
(3, 3);

CREATE TRIGGER Trigger_CarPriceUpdate
ON Cars
AFTER UPDATE
AS
BEGIN
    IF UPDATE(Price)
    BEGIN
        INSERT INTO CarPriceHistory (CarId, OldPrice, NewPrice, ChangeDate)
        SELECT InsertedTable.Id, DeletedTable.Price, InsertedTable.Price, GETDATE()
        FROM Inserted InsertedTable
        JOIN Deleted DeletedTable ON InsertedTable.Id = DeletedTable.Id
        WHERE InsertedTable.Price <> DeletedTable.Price;
    END;
END;


CREATE TRIGGER Trigger_PreventCustomerDeletion
ON Customers
INSTEAD OF DELETE
AS
BEGIN
    IF EXISTS (
        SELECT 1 FROM Orders
        WHERE CustomerId IN (SELECT Id FROM Deleted)
    )
    BEGIN
        PRINT('Cannot delete a customer with active orders.');
        RETURN;
    END;
    DELETE FROM Customers WHERE Id IN (SELECT Id FROM Deleted);
END;

CREATE TRIGGER Trigger_LogDeletedOrders
ON Orders
AFTER DELETE
AS
BEGIN
    INSERT INTO DeletedOrdersLog (OrderId, CustomerId, CarId, OrderDate, DeletedAt)
    SELECT DeletedTable.Id, DeletedTable.CustomerId, DeletedTable.CarId, DeletedTable.OrderDate, GETDATE()
    FROM Deleted DeletedTable;
END;

CREATE TRIGGER Trigger_AutoReducePriceOnYearUpdate
ON Cars
AFTER UPDATE
AS
BEGIN
    IF UPDATE(Year)
    BEGIN
        UPDATE Cars
        SET Price = Cars.Price * 0.95
        FROM Cars
        JOIN Inserted InsertedTable ON Cars.Id = InsertedTable.Id
        JOIN Deleted DeletedTable ON Cars.Id = DeletedTable.Id
        WHERE InsertedTable.Year <> DeletedTable.Year;
    END;
END;


CREATE TRIGGER Trigger_PreventDuplicateOrders
ON Orders
AFTER INSERT
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM Orders ExistingOrders
        JOIN Inserted NewOrders
        ON ExistingOrders.CustomerId = NewOrders.CustomerId
        AND ExistingOrders.CarId = NewOrders.CarId
    )
    BEGIN
        PRINT('This customer has already ordered this car.');
        ROLLBACK TRANSACTION;
    END;
END;
