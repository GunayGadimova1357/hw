CREATE DATABASE academydb;

CREATE TABLE GroupTable (
    GroupId INT PRIMARY KEY IDENTITY(1,1),
    GroupName NVARCHAR(100) NOT NULL,
    StudentsCount INT DEFAULT 0
);

CREATE TABLE Student (
    StudentId INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,
    AverageGrade FLOAT DEFAULT 0,
    GroupId INT NOT NULL FOREIGN KEY REFERENCES GroupTable(GroupId)
);

CREATE TABLE Course (
    CourseId INT PRIMARY KEY IDENTITY(1,1),
    CourseName NVARCHAR(100) NOT NULL
);

CREATE TABLE Teacher (
    TeacherId INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL
);

CREATE TABLE StudentCourse (
    StudentId INT NOT NULL FOREIGN KEY REFERENCES Student(StudentId),
    CourseId INT NOT NULL FOREIGN KEY REFERENCES Course(CourseId),
    PRIMARY KEY (StudentId, CourseId)
);

CREATE TABLE Grade (
    GradeId INT PRIMARY KEY IDENTITY(1,1),
    StudentId INT NOT NULL,
    CourseId INT NOT NULL,
    GradeValue INT CHECK (GradeValue BETWEEN 1 AND 5),
    FOREIGN KEY (StudentId) REFERENCES Student(StudentId),
    FOREIGN KEY (CourseId) REFERENCES Course(CourseId)
);

CREATE TABLE GradeHistory (
    HistoryId INT PRIMARY KEY IDENTITY(1,1),
    StudentId INT NOT NULL,
    CourseId INT NOT NULL,
    OldGrade INT,
    NewGrade INT,
    ChangeDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE Attendance (
    AttendanceId INT PRIMARY KEY IDENTITY(1,1),
    AttendanceDate DATE NOT NULL,
    IsAbsent BIT NOT NULL,
    StudentId INT NOT NULL FOREIGN KEY REFERENCES Student(StudentId)
);

CREATE TABLE RetakeList (
    RetakeId INT PRIMARY KEY IDENTITY(1,1),
    StudentId INT NOT NULL FOREIGN KEY REFERENCES Student(StudentId)
);

CREATE TABLE Payments (
    PaymentId INT PRIMARY KEY IDENTITY(1,1),
    Amount DECIMAL(10,2) NOT NULL,
    IsPaid BIT NOT NULL,
    StudentId INT NOT NULL FOREIGN KEY REFERENCES Student(StudentId)
);

CREATE TABLE Warnings (
    WarningId INT PRIMARY KEY IDENTITY(1,1),
    Reason NVARCHAR(255),
    WarningDate DATETIME DEFAULT GETDATE(),
    StudentId INT NOT NULL FOREIGN KEY REFERENCES Student(StudentId)
);

INSERT INTO GroupTable (GroupName) VALUES
    ('Group A'),
    ('Group B');

INSERT INTO Student (FullName, GroupId) VALUES
    ('John Smith', 1),
    ('Peter Johnson', 1),
    ('Mary Brown', 2);

INSERT INTO Course (CourseName) VALUES
    ('Introduction to Programming'),
    ('Mathematics'),
    ('Physics');

INSERT INTO Teacher (FullName) VALUES
    ('Andrew Roberts'),
    ('Elena Carter');

INSERT INTO StudentCourse (StudentId, CourseId) VALUES
    (1, 1),
    (2, 1),
    (3, 2);

INSERT INTO Grade (StudentId, CourseId, GradeValue) VALUES
    (1, 1, 5),
    (2, 1, 4),
    (3, 2, 2);

INSERT INTO Attendance (StudentId, AttendanceDate, IsAbsent) VALUES
    (1, '2025-02-10', 1),
    (1, '2025-02-11', 1);

INSERT INTO Payments (StudentId, Amount, IsPaid) VALUES
    (1, 50000, 1),
    (2, 50000, 0),
    (3, 30000, 1);

CREATE TRIGGER trg_LimitStudentsInGroup
ON Student
AFTER INSERT
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM GroupTable
        INNER JOIN inserted ON GroupTable.GroupId = inserted.GroupId
        WHERE GroupTable.StudentsCount >= 30
    )
    BEGIN
        RAISERROR ('Cannot add more than 30 students to the group.', 16, 1);
        ROLLBACK TRANSACTION;
    END
END;

CREATE TRIGGER trg_UpdateStudentsCount
ON Student
AFTER INSERT, DELETE
AS
BEGIN
    UPDATE GroupTable
    SET StudentsCount = (SELECT COUNT(*) FROM Student WHERE Student.GroupId = GroupTable.GroupId);
END;

CREATE TRIGGER trg_AutoEnrollToCourse
ON Student
AFTER INSERT
AS
BEGIN
    DECLARE @CourseId INT;

    SELECT @CourseId = CourseId FROM Course WHERE CourseName = 'Introduction to Programming';

    IF @CourseId IS NOT NULL
    BEGIN
        INSERT INTO StudentCourse (StudentId, CourseId)
        SELECT StudentId, @CourseId FROM inserted;
    END
END;

CREATE TRIGGER trg_LowGradeWarning
ON Grade
AFTER INSERT, UPDATE
AS
BEGIN
    INSERT INTO Warnings (StudentId, Reason, WarningDate)
    SELECT inserted.StudentId, 'Low grade', GETDATE()
    FROM inserted
    WHERE inserted.GradeValue < 3;
END;

CREATE TRIGGER trg_PreventTeacherDeletion
ON Teacher
INSTEAD OF DELETE
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM Course
        INNER JOIN deleted ON Course.CourseId = deleted.TeacherId
    )
    BEGIN
        RAISERROR ('Cannot delete a teacher with active courses.', 16, 1);
        RETURN;
    END

    DELETE FROM Teacher WHERE TeacherId IN (SELECT TeacherId FROM deleted);
END;

CREATE TRIGGER trg_GradeHistory
ON Grade
AFTER UPDATE
AS
BEGIN
    INSERT INTO GradeHistory (StudentId, CourseId, OldGrade, NewGrade, ChangeDate)
    SELECT deleted.StudentId, deleted.CourseId, deleted.GradeValue, inserted.GradeValue, GETDATE()
    FROM deleted
    INNER JOIN inserted ON deleted.StudentId = inserted.StudentId AND deleted.CourseId = inserted.CourseId;
END;


CREATE TRIGGER trg_AttendanceControl
ON Attendance
AFTER INSERT
AS
BEGIN
    INSERT INTO RetakeList (StudentId)
    SELECT inserted.StudentId
    FROM inserted
    WHERE (
        SELECT COUNT(*)
        FROM Attendance
        WHERE Attendance.StudentId = inserted.StudentId AND Attendance.IsAbsent = 1) > 5;
END;

CREATE TRIGGER trg_PreventStudentDeletion
ON Student
INSTEAD OF DELETE
AS
BEGIN
    IF EXISTS (
        SELECT 1 FROM Payments
        INNER JOIN deleted ON Payments.StudentId = deleted.StudentId
        WHERE Payments.IsPaid = 0
    ) OR EXISTS (
        SELECT 1 FROM Grade
        INNER JOIN deleted ON Grade.StudentId = deleted.StudentId
        WHERE Grade.GradeValue < 3
    )
    BEGIN
        RAISERROR ('Cannot delete a student with outstanding payments or failing grades.', 16, 1);
        RETURN;
    END

    DELETE FROM Student WHERE StudentId IN (SELECT StudentId FROM deleted);
END;

CREATE TRIGGER trg_UpdateAverageGrade
ON Grade
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    UPDATE Student
    SET AverageGrade = (
        SELECT AVG(CAST(GradeValue AS FLOAT))
        FROM Grade
        WHERE Grade.StudentId = Student.StudentId
    )
    WHERE Student.StudentId IN (
        SELECT DISTINCT StudentId FROM inserted
        UNION
        SELECT DISTINCT StudentId FROM deleted
    );
END;

