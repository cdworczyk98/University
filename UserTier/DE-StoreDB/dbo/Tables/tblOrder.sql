CREATE TABLE [dbo].[tblOrder]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [OrderId] INT NOT NULL, 
    [ProductId] INT NOT NULL, 
    [CustomerId] INT NOT NULL, 
    [Quantity] INT NOT NULL, 
    [Total] FLOAT NOT NULL, 
    [OrderDate] DATETIME NOT NULL
)
