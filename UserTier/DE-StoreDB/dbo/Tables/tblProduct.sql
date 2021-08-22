CREATE TABLE [dbo].[tblProduct]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ProductId] INT NOT NULL, 
    [Name] VARCHAR(50) NOT NULL, 
    [Price] INT NOT NULL, 
    [Stock] INT NOT NULL, 
    [Category] INT NOT NULL, 
    [Offer] INT NOT NULL, 
    [Delivery] TINYINT NOT NULL
)
