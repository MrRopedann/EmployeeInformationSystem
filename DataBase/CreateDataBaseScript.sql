CREATE DATABASE EmployeeDB;

USE EmployeeDB;

CREATE TABLE Employees (
    EmployeeID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50),
    LastName NVARCHAR(50),
    Email NVARCHAR(100),
    DateOfBirth DATE,
    Salary DECIMAL(18, 2)
);
