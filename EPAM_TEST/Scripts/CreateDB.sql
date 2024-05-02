IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = 'EPAM')
BEGIN
    CREATE DATABASE EPAM;
END
GO

USE EPAM;
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Student')
BEGIN
    CREATE TABLE Student
    (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Name NVARCHAR(100) NOT NULL
    );
END
ELSE
BEGIN
    PRINT 'Table Student already exists';
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'StudyGroup')
BEGIN
    CREATE TABLE StudyGroup
    (
        Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        Name NVARCHAR(30) NOT NULL,
        Subject INT NOT NULL,
        CreatedDate DATETIME NOT NULL
    );
END
ELSE
BEGIN
    PRINT 'Table StudyGroup already exists';
END
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'StudentStudyGroup')
BEGIN
    CREATE TABLE StudentStudyGroup
    (
        StudentId UNIQUEIDENTIFIER,
        StudyGroupId UNIQUEIDENTIFIER,
        CONSTRAINT PK_StudentStudyGroup PRIMARY KEY (StudentId, StudyGroupId),
        CONSTRAINT FK_StudentStudyGroup_Student FOREIGN KEY (StudentId) REFERENCES Student(Id),
        CONSTRAINT FK_StudentStudyGroup_StudyGroup FOREIGN KEY (StudyGroupId) REFERENCES StudyGroup(Id)
    );
END
ELSE
BEGIN
    PRINT 'Table StudentStudyGroup already exists';
END
GO