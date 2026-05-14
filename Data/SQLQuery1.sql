-- 1. IdentityDB
CREATE DATABASE IdentityDB;
GO
USE IdentityDB;
CREATE TABLE Users (
    Id INT PRIMARY KEY,
    Username NVARCHAR(50),
    FullName NVARCHAR(100),
    [Rank] NVARCHAR(20)
);
INSERT INTO Users VALUES 
(1, 'sv01', 'Nguyen Van A', 'Gold'),
(2, 'sv02', 'Tran Thi B', 'Silver');

-- 2. BookDB
CREATE DATABASE BookDB;
GO
USE BookDB;
CREATE TABLE Books (
    Id INT PRIMARY KEY,
    Title NVARCHAR(200),
    Author NVARCHAR(100),
    Stock INT
);
INSERT INTO Books VALUES 
(101, 'Clean Code', 'Robert C. Martin', 5),
(102, 'Design Patterns', 'Gang of Four', 2);

-- 3. BorrowingDB
CREATE DATABASE BorrowingDB;
GO
USE BorrowingDB;
CREATE TABLE BorrowRecords (
    Id INT PRIMARY KEY IDENTITY,
    UserId INT,
    BookId INT,
    BorrowDate DATETIME DEFAULT GETDATE(),
    ReturnDate DATETIME NULL
);

-- 4. NotificationDB
CREATE DATABASE NotificationDB;
GO
USE NotificationDB;
CREATE TABLE Notifications (
    Id INT PRIMARY KEY IDENTITY,
    UserId INT,
    Message NVARCHAR(500),
    CreatedAt DATETIME DEFAULT GETDATE()
);