CREATE DATABASE Academy;

CREATE TABLE Departments (
    DepartmentsId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Financing MONEY NOT NULL CHECK (Financing >= 0) DEFAULT 0,
    DepartmentsName NVARCHAR(100) NOT NULL UNIQUE CHECK (LEN(DepartmentsName) > 0)
);

CREATE TABLE Faculties (
    FacultiesId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Dean NVARCHAR(MAX) NOT NULL CHECK (LEN(Dean) > 0),
    FacultiesName NVARCHAR(100) NOT NULL UNIQUE CHECK (LEN(FacultiesName) > 0)
);

CREATE TABLE Groups (
    GroupsId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    GroupsName NVARCHAR(10) NOT NULL UNIQUE CHECK (LEN(GroupsName) > 0),
    Rating INT NOT NULL CHECK (Rating BETWEEN 0 AND 5),
    Year INT NOT NULL CHECK (Year BETWEEN 1 AND 5)
);

CREATE TABLE Teachers (
    TeachersId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    EmploymentDate DATE NOT NULL CHECK (EmploymentDate >= '1990-01-01'),
    IsAssistant BIT NOT NULL DEFAULT 0,
    IsProfessor BIT NOT NULL DEFAULT 0,
    TeachersName NVARCHAR(MAX) NOT NULL CHECK (LEN(TeachersName) > 0) ,
    Position NVARCHAR(MAX) NOT NULL CHECK (LEN(Position ) > 0),
    Premium MONEY NOT NULL CHECK (Premium >= 0) DEFAULT 0,
    Salary MONEY NOT NULL CHECK (Salary > 0),
    TeachersSurname NVARCHAR(MAX) NOT NULL CHECK (LEN(TeachersSurname) > 0)
);

INSERT INTO Departments (Financing, DepartmentsName) VALUES
(10000, 'Mathematics'),
(12000, 'Physics'),
(9000, 'Software Development'),
(30000, 'Engineering');

INSERT INTO Faculties (Dean, FacultiesName) VALUES
('Dr. Smith', 'Science'),
('Dr. Brown', 'Engineering'),
('Dr. Johnson', 'Computer Science');

INSERT INTO Groups (GroupsName, Rating, Year) VALUES
('MATH101', 4, 2),
('PHYS201', 5, 3),
('ENGR301', 3, 5),
('COMP401', 2, 5);

INSERT INTO Teachers (EmploymentDate, IsAssistant, IsProfessor, TeachersName, Position, Premium, Salary, TeachersSurname) VALUES
('1995-06-15', 0, 1, 'John', 'Professor', 500, 2000, 'Doe'),
('2005-09-10', 1, 0, 'Alice', 'Assistant', 300, 1000, 'Smith'),
('1998-04-22', 0, 1, 'Michael', 'Professor', 700, 2200, 'Brown'),
('2010-07-01', 1, 0, 'Emily', 'Assistant', 400, 900, 'Johnson'),
('1993-11-30', 0, 1, 'David', 'Professor', 600, 1800, 'Williams');

SELECT DepartmentsName, Financing, DepartmentsId FROM Departments;

SELECT GroupsName AS "Group Name", Rating AS "Group Rating" FROM Groups;

SELECT
    TeachersSurname,
    (Premium / Salary) * 100 AS "Premium Percentage",
    (Premium / (Salary + Premium)) * 100 AS "Premium Salary Percentage"
FROM Teachers;

SELECT
    'The dean of faculty ' + FacultiesName + ' is ' + Dean AS "Faculty Info"
FROM Faculties;

SELECT TeachersSurname FROM Teachers WHERE IsProfessor = 1 AND Salary > 1050;

SELECT DepartmentsName FROM Departments WHERE Financing < 11000 OR Financing > 25000;

SELECT FacultiesName FROM Faculties WHERE FacultiesName <> 'Computer Science';

SELECT TeachersSurname, Position FROM Teachers WHERE IsProfessor = 0;

SELECT TeachersSurname, Position, Salary, Premium
FROM Teachers
WHERE IsAssistant = 1 AND Premium BETWEEN 160 AND 550;

SELECT TeachersSurname, Salary FROM Teachers WHERE IsAssistant = 1;

SELECT TeachersSurname, Position FROM Teachers WHERE EmploymentDate < '2000-01-01';

SELECT DepartmentsName AS "Name of Department" FROM Departments
WHERE DepartmentsName < 'Software Development';

SELECT TeachersSurname FROM Teachers WHERE IsAssistant = 1 AND (Salary + Premium) <= 1200;

SELECT GroupsName FROM Groups WHERE Year = 5 AND Rating BETWEEN 2 AND 4;

SELECT TeachersSurname FROM Teachers WHERE IsAssistant = 1 AND (Salary < 550 OR Premium < 200);
