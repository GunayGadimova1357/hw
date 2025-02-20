CREATE DATABASE fortydb;

CREATE TABLE Customers (
    CustomerID INT IDENTITY (1,1) PRIMARY KEY,
    FirstName VARCHAR(50),
    LastName VARCHAR(50),
    Email VARCHAR(100)
);
INSERT INTO Customers(FirstName, LastName, Email)
VALUES ( 'Anna', 'Brown', 'qwerr@eee.xxx'),
       ( 'John', 'Smith', 'johns@ddd.sd'),
       ( 'Emily', 'Johnson', 'qfffr@eee.xxx'),
       ( 'William', 'Wilson', 'wwwww@ddd.sd'),
       ( 'Arya', 'Martinez', 'aryamartinez@ddd.sd');

UPDATE Customers
SET Email= 'annabrown@gmail.com'
WHERE CustomerID=1;

DELETE FROM Customers WHERE CustomerID=5;

SELECT * FROM Customers
ORDER BY LastName;

INSERT INTO Customers (FirstName, LastName, Email)
VALUES ( 'Sophia', 'Davis', 'qwerr@eee.xxx'),
       ( 'James', 'Anderson', 'jjjs@ddd.sd');

CREATE TABLE Orders (
    OrderID INT IDENTITY (1,1)  PRIMARY KEY,
    CustomerID INT,
    OrderDate DATE,
    TotalAmount DECIMAL(10, 2),
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

INSERT INTO Orders (CustomerID, OrderDate, TotalAmount)
VALUES (1, '2025-02-08', 15.45),
       (2, '2025-01-02', 150),
       (3, '2024-12-31', 200.50);

UPDATE Orders
SET TotalAmount=200
WHERE CustomerID=2;

DELETE FROM Orders WHERE CustomerID=3;

SELECT * FROM Orders
WHERE CustomerID=1;

SELECT * FROM Orders
WHERE YEAR(OrderDate)=2023;

CREATE TABLE Products (
    ProductID INT IDENTITY (1,1)  PRIMARY KEY,
    ProductName VARCHAR(100),
    Price DECIMAL(10, 2)
);

INSERT INTO Products (ProductName, Price)
VALUES ('book', 7),
       ('guitar', 250),
       ('cd', 5),
       ('jewellery', 10);

UPDATE Products
SET Price=255
WHERE ProductID=2;

DELETE FROM Products WHERE ProductID=4;

SELECT * FROM Products
WHERE Price>100;

SELECT * FROM Products
WHERE Price<=50;

CREATE TABLE OrderDetails (
    OrderDetailID INT IDENTITY (1,1)  PRIMARY KEY,
    OrderID INT,
    ProductID INT,
    Quantity INT,
    Price DECIMAL(10, 2),
    FOREIGN KEY (OrderID) REFERENCES Orders(OrderID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);

INSERT INTO OrderDetails (OrderID, ProductID, Quantity, Price)
VALUES  (1, 1, 2, 19.99),
        (1, 2, 1, 9.50),
        (2, 3, 5, 15.75),
        (2, 4, 3, 25.00),
        (3, 1, 10, 5.99),
        (3, 2, 7, 12.49);

UPDATE OrderDetails
SET Quantity=3
WHERE OrderDetailID=1;

DELETE FROM OrderDetails WHERE OrderDetailID=2;

SELECT * FROM OrderDetails
WHERE OrderID=1;

SELECT * FROM OrderDetails
WHERE ProductID=2;

SELECT Orders.OrderID, CONCAT(Customers.FirstName, ' ', Customers.LastName) AS FullName
FROM Orders
INNER JOIN Customers ON Orders.CustomerID = Customers.CustomerID;

SELECT OrderDetails.ProductID,  Customers.FirstName, OrderDetails.Quantity
FROM OrderDetails
INNER JOIN Orders ON OrderDetails.OrderID = Orders.OrderID
INNER JOIN Customers ON Orders.CustomerID = Customers.CustomerID;

SELECT Orders.OrderID, CONCAT(Customers.FirstName, ' ', Customers.LastName) AS FullName
FROM Orders
LEFT JOIN Customers ON Orders.CustomerID = Customers.CustomerID;

SELECT Orders.OrderID, Products.ProductName
FROM Orders
INNER JOIN OrderDetails ON Orders.OrderID = OrderDetails.OrderID
INNER JOIN Products ON OrderDetails.ProductID=Products.ProductID;

SELECT Orders.OrderID, CONCAT(Customers.FirstName, ' ', Customers.LastName) AS FullName
FROM Customers
LEFT JOIN Orders  on Customers.CustomerID = Orders.CustomerID;

SELECT Products.ProductName, OrderDetails.OrderID, OrderDetails.Quantity, OrderDetails.Price
FROM Products
RIGHT JOIN OrderDetails  on Products.ProductID = OrderDetails.ProductID;

SELECT Orders.OrderID, Products.ProductName
FROM Orders
INNER JOIN OrderDetails ON Orders.OrderID = OrderDetails.OrderID
INNER JOIN Products ON OrderDetails.ProductID=Products.ProductID;

SELECT Customers.FirstName, Orders.OrderID, OrderDetails.Quantity, OrderDetails.Price
FROM OrderDetails
INNER JOIN Orders  ON Orders.OrderID = OrderDetails.OrderID
INNER JOIN Customers  ON Customers.CustomerID = Orders.CustomerID;

SELECT  Customers.FirstName
FROM Customers
INNER JOIN Orders ON Customers.CustomerID = Orders.CustomerID
INNER JOIN OrderDetails ON Orders.OrderID = OrderDetails.OrderID
GROUP BY Customers.FirstName
HAVING SUM(OrderDetails.Quantity * OrderDetails.Price) > 500;

SELECT Products.ProductName
FROM OrderDetails
INNER JOIN Products ON OrderDetails.ProductID = Products.ProductID
GROUP BY Products.ProductName
HAVING SUM(OrderDetails.Quantity) > 10;

SELECT Customers.FirstName, SUM(OrderDetails.Quantity * OrderDetails.Price) AS TotalSpent
FROM Customers
INNER JOIN Orders ON Customers.CustomerID = Orders.CustomerID
INNER JOIN OrderDetails ON Orders.OrderID = OrderDetails.OrderID
GROUP BY Customers.FirstName;

SELECT ProductName, Price
FROM Products
WHERE Price > (SELECT AVG(Price) FROM Products);

SELECT Orders.OrderID, Customers.FirstName, Products.ProductName, OrderDetails.Quantity, OrderDetails.Price
FROM Orders
INNER JOIN Customers ON Orders.CustomerID = Customers.CustomerID
INNER JOIN OrderDetails ON Orders.OrderID = OrderDetails.OrderID
INNER JOIN Products ON OrderDetails.ProductID = Products.ProductID;

SELECT Customers.FirstName, Customers.LastName, Products.ProductName, Orders.OrderID,
       SUM(OrderDetails.Quantity * OrderDetails.Price) AS TotalOrderCost
FROM Customers
INNER JOIN Orders ON Customers.CustomerID = Orders.CustomerID
INNER JOIN OrderDetails ON Orders.OrderID = OrderDetails.OrderID
INNER JOIN Products ON OrderDetails.ProductID = Products.ProductID
GROUP BY Customers.FirstName, Customers.LastName, Products.ProductName, Orders.OrderID;

SELECT Orders.OrderID, SUM(OrderDetails.Quantity * OrderDetails.Price) AS TotalCost
FROM Orders
INNER JOIN OrderDetails ON Orders.OrderID = OrderDetails.OrderID
GROUP BY Orders.OrderID
HAVING SUM(OrderDetails.Quantity * OrderDetails.Price) > 1000;

SELECT Customers.FirstName, Customers.LastName,
       SUM(OrderDetails.Quantity * OrderDetails.Price) AS TotalSpent
FROM Customers
INNER JOIN Orders ON Customers.CustomerID = Orders.CustomerID
INNER JOIN OrderDetails ON Orders.OrderID = OrderDetails.OrderID
GROUP BY Customers.FirstName, Customers.LastName
HAVING SUM(OrderDetails.Quantity * OrderDetails.Price) > (SELECT AVG(TotalOrderSum)
        FROM (SELECT SUM(OrderDetails.Quantity * OrderDetails.Price) AS TotalOrderSum
              FROM Orders
              INNER JOIN OrderDetails ON Orders.OrderID = OrderDetails.OrderID
              GROUP BY Orders.OrderID) AS OrderAvg);

SELECT Customers.FirstName, Customers.LastName, COUNT(Orders.OrderID) AS OrderCount
FROM Customers
LEFT JOIN Orders ON Customers.CustomerID = Orders.CustomerID
GROUP BY Customers.FirstName, Customers.LastName;

SELECT Products.ProductName, SUM(OrderDetails.Quantity) AS TotalOrdered
FROM OrderDetails
INNER JOIN Products ON OrderDetails.ProductID = Products.ProductID
GROUP BY Products.ProductName
HAVING SUM(OrderDetails.Quantity) > 3;

SELECT Customers.FirstName, Customers.LastName, Orders.OrderID, SUM(OrderDetails.Quantity) AS TotalItemsOrdered
FROM Customers
INNER JOIN Orders ON Customers.CustomerID = Orders.CustomerID
INNER JOIN OrderDetails ON Orders.OrderID = OrderDetails.OrderID
GROUP BY Customers.FirstName, Customers.LastName, Orders.OrderID;
