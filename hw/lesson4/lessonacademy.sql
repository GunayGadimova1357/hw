CREATE DATABASE AcademyDB;

CREATE TABLE Curators (
    CuratorsId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Name NVARCHAR(MAX) NOT NULL CHECK (LEN(Name) > 0),
    Surname NVARCHAR(MAX) NOT NULL CHECK (LEN(Surname) > 0)
);

CREATE TABLE Departments (
    DepartmentsId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Financing MONEY NOT NULL CHECK (Financing > 0) DEFAULT 0,
    Name NVARCHAR(100) NOT NULL UNIQUE CHECK (LEN(Name) > 0),
    FacultyId INT NOT NULL FOREIGN KEY REFERENCES Faculties(FacultiesId)
);

CREATE TABLE Faculties (
    FacultiesId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Financing MONEY NOT NULL CHECK (Financing >= 0) DEFAULT 0,
    Name NVARCHAR(100) NOT NULL UNIQUE CHECK (LEN(Name) > 0)
);

CREATE TABLE Groups (
    GroupsId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Name NVARCHAR(10) NOT NULL UNIQUE CHECK (LEN(Name) > 0),
    Year INT NOT NULL CHECK (Year BETWEEN 1 AND 5),
    DepartmentId INT NOT NULL FOREIGN KEY REFERENCES Departments(DepartmentsId)
);

CREATE TABLE GroupsCurators (
    GroupsCuratorsId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    CuratorId INT NOT NULL FOREIGN KEY REFERENCES Curators(CuratorsId),
    GroupId INT NOT NULL FOREIGN KEY  REFERENCES Groups(GroupsId)
);

CREATE TABLE Lectures (
    LecturesId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    LectureRoom NVARCHAR(MAX) NOT NULL CHECK (LEN(LectureRoom) > 0),
    SubjectId INT NOT NULL FOREIGN KEY  REFERENCES Subjects(SubjectsId),
    TeacherId INT NOT NULL FOREIGN KEY  REFERENCES Teachers(TeachersId)
);

CREATE TABLE GroupsLectures (
    GroupsLecturesId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    GroupId INT NOT NULL FOREIGN KEY REFERENCES Groups(GroupsId),
    LectureId INT NOT NULL FOREIGN KEY REFERENCES Lectures(LecturesId)
);

CREATE TABLE Subjects (
    SubjectsId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Name NVARCHAR(100) NOT NULL UNIQUE CHECK (LEN(Name) > 0)
);

CREATE TABLE Teachers (
    TeachersId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Name NVARCHAR(MAX) NOT NULL CHECK (LEN(Name) > 0),
    Surname NVARCHAR(MAX) NOT NULL CHECK (LEN(Surname) > 0),
    Salary MONEY NOT NULL CHECK (Salary > 0)
);

INSERT INTO Faculties (Financing, Name) VALUES
(500000, 'Computer Science'),
(400000, 'Mathematics'),
(300000, 'Physics');

INSERT INTO Departments (Financing, Name, FacultyId) VALUES
(200000, 'Software Engineering', 1),
(250000, 'Cybersecurity', 1),
(150000, 'Applied Math', 2),
(120000, 'Theoretical Physics', 3);

INSERT INTO Curators (Name, Surname) VALUES
('John', 'Doe'),
('Alice', 'Smith'),
('Robert', 'Brown');

INSERT INTO Groups (Name, Year, DepartmentId) VALUES
('P101', 1, 1),
('P102', 2, 1),
('P107', 3, 2),
('M201', 2, 3),
('CS501', 5, 1);

INSERT INTO GroupsCurators (CuratorId, GroupId) VALUES
(1, 1),
(2, 2),
(3, 3),
(1, 4),
(2, 5);

INSERT INTO Teachers (Name, Surname, Salary) VALUES
('Michael', 'Johnson', 60000),
('Samantha', 'Adams', 55000),
('David', 'Clark', 70000);

INSERT INTO Subjects (Name) VALUES
('Database Theory'),
('Algorithms'),
('Linear Algebra');

INSERT INTO Lectures (LectureRoom, SubjectId, TeacherId) VALUES
('A101', 1, 1),
('B103', 2, 2),
('C205', 3, 3),
('B103', 1, 2);

INSERT INTO GroupsLectures (GroupId, LectureId) VALUES
(1, 1),
(2, 2),
(3, 3),
(4, 1),
(5, 4);

SELECT Teachers.Name, Teachers.Surname, Groups.Name
FROM Teachers, Groups;

SELECT Faculties.Name
FROM Faculties
JOIN Departments ON Faculties.FacultiesId = Departments.FacultyId
GROUP BY Faculties.Name, Faculties.Financing
HAVING SUM(Departments.Financing) > Faculties.Financing;

SELECT Faculties.Name
FROM Faculties
JOIN Departments ON Faculties.FacultiesId = Departments.FacultyId
GROUP BY Faculties.Name, Faculties.Financing
HAVING SUM(Departments.Financing) > Faculties.Financing;

SELECT Teachers.Name, Teachers.Surname
FROM Teachers
JOIN Lectures ON Teachers.TeachersId = Lectures.TeacherId
JOIN GroupsLectures ON Lectures.LecturesId = GroupsLectures.LectureId
JOIN Groups ON GroupsLectures.GroupId = Groups.GroupsId
WHERE Groups.Name = 'P107';

SELECT DISTINCT Teachers.Surname, Faculties.Name
FROM Teachers
JOIN Lectures ON Teachers.TeachersId = Lectures.TeacherId
JOIN GroupsLectures ON Lectures.LecturesId = GroupsLectures.LectureId
JOIN Groups ON GroupsLectures.GroupId = Groups.GroupsId
JOIN Departments ON Groups.DepartmentId = Departments.DepartmentsId
JOIN Faculties ON Departments.FacultyId = Faculties.FacultiesId;

SELECT Departments.Name, Groups.Name
FROM Departments
JOIN Groups ON Departments.DepartmentsId = Groups.DepartmentId;

SELECT Subjects.Name
FROM Subjects
JOIN Lectures ON Subjects.SubjectsId = Lectures.SubjectId
JOIN Teachers ON Lectures.TeacherId = Teachers.TeachersId
WHERE Teachers.Name = 'Samantha' AND Teachers.Surname = 'Adams';

SELECT DISTINCT Departments.Name
FROM Departments
JOIN Groups ON Departments.DepartmentsId = Groups.DepartmentId
JOIN GroupsLectures ON Groups.GroupsId = GroupsLectures.GroupId
JOIN Lectures ON GroupsLectures.LectureId = Lectures.LecturesId
JOIN Subjects ON Lectures.SubjectId = Subjects.SubjectsId
WHERE Subjects.Name = 'Database Theory';

SELECT Groups.Name
FROM Groups
JOIN Departments ON Groups.DepartmentId = Departments.DepartmentsId
JOIN Faculties ON Departments.FacultyId = Faculties.FacultiesId
WHERE Faculties.Name = 'Computer Science';

SELECT Groups.Name, Faculties.Name
FROM Groups
JOIN Departments ON Groups.DepartmentId = Departments.DepartmentsId
JOIN Faculties ON Departments.FacultyId = Faculties.FacultiesId
WHERE Groups.Year = 5;

