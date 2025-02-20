CREATE DATABASE Academy;

USE Academy;

CREATE TABLE Curators (
    CuratorsId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(MAX) NOT NULL CHECK (LEN(Name) > 0),
    Surname NVARCHAR(MAX) NOT NULL CHECK (LEN(SurName) > 0)
);

CREATE TABLE Faculties (
    FacultiesId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL UNIQUE CHECK (LEN(Name) > 0)
);

CREATE TABLE Departments (
    DepartmentsId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Building INT NOT NULL CHECK (Building BETWEEN 1 AND 5),
    Financing MONEY NOT NULL CHECK (Financing >= 0) DEFAULT 0,
    Name NVARCHAR(100) NOT NULL UNIQUE CHECK (LEN(Name) > 0),
    FacultyId INT NOT NULL FOREIGN KEY REFERENCES Faculties(FacultiesId)
);

CREATE TABLE Groups (
    GroupsId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(10) NOT NULL UNIQUE CHECK (LEN(Name) > 0),
    Year INT NOT NULL CHECK (Year BETWEEN 1 AND 5),
    DepartmentId INT NOT NULL FOREIGN KEY REFERENCES Departments(DepartmentsId)
);

CREATE TABLE GroupsCurators (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CuratorId INT NOT NULL FOREIGN KEY REFERENCES Curators(CuratorsId),
    GroupId INT NOT NULL FOREIGN KEY REFERENCES Groups(GroupsId)
);

CREATE TABLE GroupsLectures (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    GroupId INT NOT NULL FOREIGN KEY REFERENCES Groups(GroupsId),
    LectureId INT NOT NULL FOREIGN KEY REFERENCES Lectures(LecturesId)
);

CREATE TABLE GroupsStudents (
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    GroupId INT NOT NULL FOREIGN KEY  REFERENCES Groups(GroupsId),
    StudentId INT NOT NULL FOREIGN KEY REFERENCES Students(StudentsId)
);

CREATE TABLE Subjects (
    SubjectsId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL UNIQUE CHECK (LEN(Name) > 0)
);

CREATE TABLE Teachers (
    TeachersId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    IsProfessor BIT NOT NULL DEFAULT 0,
    Name NVARCHAR(MAX) NOT NULL CHECK (LEN(Name) > 0),
    Surname NVARCHAR(MAX) NOT NULL CHECK (LEN(SurName) > 0),
    Salary MONEY NOT NULL CHECK (Salary > 0)
);

CREATE TABLE Lectures (
    LecturesId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Date DATE NOT NULL CHECK (Date <= GETDATE()),
    SubjectId INT NOT NULL FOREIGN KEY REFERENCES Subjects(SubjectsId),
    TeacherId INT NOT NULL FOREIGN KEY REFERENCES Teachers(TeachersId)
);

CREATE TABLE Students (
    StudentsId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name NVARCHAR(MAX) NOT NULL CHECK (LEN(Name) > 0),
    Surname NVARCHAR(MAX) NOT NULL CHECK (LEN(SurName) > 0),
    Rating INT NOT NULL CHECK (Rating BETWEEN 0 AND 5)
);


INSERT INTO Faculties (Name) VALUES
('Computer Science'),
('Engineering'),
('Mathematics');

INSERT INTO Departments (Building, Financing, Name, FacultyId) VALUES
(1, 200000, 'Software Development', 1),
(2, 50000, 'Mechanical Engineering', 2),
(3, 120000, 'Applied Mathematics', 3);

INSERT INTO Groups (Name, Year, DepartmentId) VALUES
('CS101', 5, 1),
('CS102', 5, 1),
('В221', 4, 2),
('MATH301', 5, 3);

INSERT INTO Curators (Name, Surname) VALUES
('Alice', 'Johnson'),
('Bob', 'Smith');

INSERT INTO GroupsCurators (CuratorId, GroupId) VALUES
(1, 1),
(1, 2),
(2, 1);

INSERT INTO Students (Name, Surname, Rating) VALUES
('John', 'Doe', 4),
('Jane', 'Smith', 5),
('Robert', 'Brown', 3),
('Alice', 'Williams', 2);

INSERT INTO GroupsStudents (GroupId, StudentId) VALUES
(1, 1),
(1, 2),
(2, 3),
(2, 4);

INSERT INTO Subjects (Name) VALUES
('Databases'),
('Algorithms'),
('Machine Learning');

INSERT INTO Teachers (IsProfessor, Name, Surname, Salary) VALUES
(1, 'Dr. Michael', 'Clark', 70000),
(0, 'Anna', 'Taylor', 50000),
(1, 'Dr. Steve', 'Adams', 80000);

INSERT INTO Lectures (Date, SubjectId, TeacherId) VALUES
('2024-02-10', 1, 1),
('2024-02-12', 1, 2),
('2024-02-14', 2, 3),
('2024-02-15', 3, 1);

INSERT INTO GroupsLectures (GroupId, LectureId) VALUES
(1, 1),
(1, 2),
(2, 3),
(2, 4);

SELECT Building
FROM Departments
GROUP BY Building
HAVING SUM(Financing) > 100000;

SELECT Name
FROM Groups
WHERE Year = 5
AND DepartmentId = (SELECT DepartmentsId FROM Departments WHERE Name = 'Software Development')
AND GroupsId IN (
    SELECT GroupId
    FROM GroupsLectures
    WHERE LectureId IN (
        SELECT Id
        FROM Lectures
        WHERE DATEPART(WEEK, Date) = 1
    )
    GROUP BY GroupId
    HAVING COUNT(LectureId) > 10
);

SELECT Name
FROM Groups
WHERE GroupsId IN (
    SELECT GroupId
    FROM GroupsStudents
    GROUP BY GroupId
    HAVING AVG((SELECT Rating FROM Students WHERE Id = StudentId)) >
           (SELECT AVG(Rating) FROM Students WHERE Id IN (SELECT StudentId FROM GroupsStudents WHERE GroupId = (SELECT GroupsId FROM Groups WHERE Name = 'D221')))
);

SELECT Name, Surname
FROM Teachers
WHERE Salary > (SELECT AVG(Salary) FROM Teachers WHERE IsProfessor = 1);

SELECT Groups.Name
FROM Groups
JOIN GroupsCurators ON Groups.GroupsId = GroupsCurators.GroupId
GROUP BY Groups.Name
HAVING COUNT(GroupsCurators.CuratorId) > 1;

SELECT Name
FROM Groups
WHERE GroupsId IN (
    SELECT GroupId
    FROM GroupsStudents
    GROUP BY GroupId
    HAVING AVG((SELECT Rating FROM Students WHERE Id = StudentId)) <
           (SELECT MIN(AVG_Rating) FROM (SELECT GroupId, AVG((SELECT Rating FROM Students WHERE Id = StudentId)) AS AVG_Rating FROM GroupsStudents WHERE GroupId IN (SELECT GroupsId FROM Groups WHERE Year = 5) GROUP BY GroupId) AS SubQuery)
);

SELECT Name
FROM Faculties
WHERE FacultiesId IN (
    SELECT FacultyId
    FROM Departments
    GROUP BY FacultyId
    HAVING SUM(Financing) > (
        SELECT SUM(Financing)
        FROM Departments
        WHERE FacultyId = (SELECT FacultiesId FROM Faculties WHERE Name = 'Computer Science')
    )
);


SELECT Subjects.Name, Teachers.Name, Teachers.Surname
FROM Lectures
JOIN Subjects ON Lectures.SubjectId = Subjects.SubjectsId
JOIN Teachers ON Lectures.TeacherId = Teachers.TeachersId
GROUP BY Subjects.Name, Teachers.Name, Teachers.Surname
HAVING COUNT(Lectures.LecturesId) = (
    SELECT MAX(LectureCount)
    FROM (
        SELECT COUNT(Lectures.LecturesId) AS LectureCount
        FROM Lectures
        GROUP BY Lectures.SubjectId
    ) AS MaxLectureCounts
);

SELECT Name
FROM Subjects
WHERE SubjectsId = (
    SELECT SubjectId
    FROM Lectures
    GROUP BY SubjectId
    HAVING COUNT(LecturesId) = (SELECT MIN(LectureCount) FROM (SELECT SubjectId, COUNT(LecturesId) AS LectureCount FROM Lectures GROUP BY SubjectId) AS SubQuery)
);

SELECT
    (SELECT COUNT(*) FROM Students WHERE StudentsId IN (SELECT StudentId FROM GroupsStudents WHERE GroupId IN (SELECT GroupsId FROM Groups WHERE DepartmentId = (SELECT DepartmentsId FROM Departments WHERE Name = 'Software Development')))) AS StudentCount,
    (SELECT COUNT(DISTINCT SubjectId) FROM Lectures WHERE LecturesId IN (SELECT LectureId FROM GroupsLectures WHERE GroupId IN (SELECT GroupsId FROM Groups WHERE DepartmentId = (SELECT DepartmentsId FROM Departments WHERE Name = 'Software Development')))) AS SubjectCount;




