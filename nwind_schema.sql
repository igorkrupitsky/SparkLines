IF NOT EXISTS(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Categories')
CREATE TABLE Categories (
	CategoryID int NOT NULL PRIMARY KEY,
	CategoryName nvarchar(15) NOT NULL,
	Description nvarchar(max) NULL,
	Picture image NULL)
go

IF NOT EXISTS(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Customers')
CREATE TABLE Customers (
	CustomerID nvarchar(5) NOT NULL PRIMARY KEY,
	CompanyName nvarchar(40) NOT NULL,
	ContactName nvarchar(30) NULL,
	ContactTitle nvarchar(30) NULL,
	Address nvarchar(60) NULL,
	City nvarchar(15) NULL,
	Region nvarchar(15) NULL,
	PostalCode nvarchar(10) NULL,
	Country nvarchar(15) NULL,
	Phone nvarchar(24) NULL,
	Fax nvarchar(24) NULL,
	lat float NULL, 
	lon float NULL)
go

IF NOT EXISTS(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Employees')
CREATE TABLE Employees (
	EmployeeID int NOT NULL PRIMARY KEY,
	LastName nvarchar(20) NOT NULL,
	FirstName nvarchar(10) NOT NULL,
	Title nvarchar(30) NULL,
	TitleOfCourtesy nvarchar(25) NULL,
	BirthDate datetime NULL,
	HireDate datetime NULL,
	Address nvarchar(60) NULL,
	City nvarchar(15) NULL,
	Region nvarchar(15) NULL,
	PostalCode nvarchar(10) NULL,
	Country nvarchar(15) NULL,
	HomePhone nvarchar(24) NULL,
	Extension nvarchar(4) NULL,
	Photo image NULL,
	Notes nvarchar(max) NULL,
	ReportsTo int NULL)
go

IF NOT EXISTS(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Order_Details')
CREATE TABLE Order_Details (
	OrderID int NOT NULL,
	ProductID int NOT NULL,
	UnitPrice money NOT NULL,
	Quantity smallint NOT NULL,
	Discount real NOT NULL)
go

IF OBJECT_ID('PK_nwind_Order_Details') IS NULL
ALTER TABLE Order_Details ADD CONSTRAINT PK_nwind_Order_Details PRIMARY KEY (ProductID, OrderID)
go

IF NOT EXISTS(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Orders')
CREATE TABLE Orders (
	OrderID int NOT NULL PRIMARY KEY,
	CustomerID nvarchar(5) NULL,
	EmployeeID int NULL,
	OrderDate datetime NULL,
	RequiredDate datetime NULL,
	ShippedDate datetime NULL,
	ShipVia int NULL,
	Freight money NULL,
	ShipName nvarchar(40) NULL,
	ShipAddress nvarchar(60) NULL,
	ShipCity nvarchar(15) NULL,
	ShipRegion nvarchar(15) NULL,
	ShipPostalCode nvarchar(10) NULL,
	ShipCountry nvarchar(15) NULL)
go

IF NOT EXISTS(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Products')
CREATE TABLE Products (
	ProductID int NOT NULL PRIMARY KEY,
	ProductName nvarchar(40) NOT NULL,
	SupplierID int NULL,
	CategoryID int NULL,
	QuantityPerUnit nvarchar(20) NULL,
	UnitPrice money NULL,
	UnitsInStock smallint NULL,
	UnitsOnOrder smallint NULL,
	ReorderLevel smallint NULL,
	Discontinued bit NOT NULL)
go

IF NOT EXISTS(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Shippers')
CREATE TABLE Shippers (
	ShipperID int NOT NULL PRIMARY KEY,
	CompanyName nvarchar(40) NOT NULL,
	Phone nvarchar(24) NULL)
go

IF NOT EXISTS(select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Suppliers')
CREATE TABLE Suppliers (
	SupplierID int NOT NULL PRIMARY KEY,
	CompanyName nvarchar(40) NOT NULL,
	ContactName nvarchar(30) NULL,
	ContactTitle nvarchar(30) NULL,
	Address nvarchar(60) NULL,
	City nvarchar(15) NULL,
	Region nvarchar(15) NULL,
	PostalCode nvarchar(10) NULL,
	Country nvarchar(15) NULL,
	Phone nvarchar(24) NULL,
	Fax nvarchar(24) NULL,
	HomePage nvarchar(max) NULL)
go
