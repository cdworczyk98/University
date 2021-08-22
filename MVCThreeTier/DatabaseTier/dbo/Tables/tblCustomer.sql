CREATE TABLE [dbo].[tblCustomer]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CustomerId] INT NOT NULL, 
    [FirstName] VARCHAR(50) NOT NULL, 
    [LastName] VARCHAR(50) NOT NULL, 
    [Email] VARCHAR(50) NOT NULL, 
    [PhoneNo] VARCHAR(50) NOT NULL, 
    [Address] VARCHAR(50) NOT NULL, 
    [LoyaltyCard] INT NOT NULL
)
