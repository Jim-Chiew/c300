-- THE DATABASE WAS CREATED AS A COLLECTIVE TEAM EFFORT.
-- Create Shape Table
-- Created Temporal tables Ref link below:
-- https://docs.microsoft.com/en-us/sql/relational-databases/tables/temporal-tables?view=sql-server-ver15
CREATE TABLE dbo.Shape 
	(	 
		[ShapeID] int NOT NULL IDENTITY(1,1) PRIMARY KEY, 
		[Name] nvarchar(30) NOT NULL, 
		ValidFrom DATETIME2 GENERATED ALWAYS AS ROW START NOT NULL, 
		ValidTo DATETIME2 GENERATED ALWAYS AS ROW END NOT NULL, 
		PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo)
	)
	WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.Shape_History));


--CREATE Product Table
CREATE TABLE dbo.Product 
	( 
		[ProductID] int NOT NULL IDENTITY(1,1) PRIMARY KEY, 
		[Name] nvarchar(30) NOT NULL, 
		[Price] FLOAT(2) NOT NULL, 
		ValidFrom DATETIME2 GENERATED ALWAYS AS ROW START NOT NULL, 
		ValidTo DATETIME2 GENERATED ALWAYS AS ROW END NOT NULL, 
		PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo)
	)
	WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.Product_History));


-- Create Inventory Table
CREATE TABLE dbo.Inventory 
	( 
		[InventoryID] int NOT NULL IDENTITY(1,1) PRIMARY KEY, 
		[Location] nvarchar(30) NOT NULL,
		ValidFrom DATETIME2 GENERATED ALWAYS AS ROW START NOT NULL, 
		ValidTo DATETIME2 GENERATED ALWAYS AS ROW END NOT NULL, 
		PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo)
	)
	WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.Inventory_History));


-- Create User Table
CREATE TABLE dbo.Users 
	( 
		[UserID] int NOT NULL IDENTITY(1,1) PRIMARY KEY, 
		[Username] nvarchar(30) NOT NULL, 
		[Name] nvarchar(30) NOT NULL, [Role] nvarchar(30) NOT NULL, 
		[Password] varbinary(50) NOT NULL, 
		[Email] nvarchar(30) NOT NULL, 
		[ContactNo] nvarchar(30) NOT NULL, 
		[ApiKey] nvarchar(30) NOT NULL, 
		[LastLogin] DATETIME NULL
	);


-- CREATE InventoryItems Table
CREATE TABLE dbo.InventoryItems 
	( 
		[ItemID] int NOT NULL IDENTITY(1,1) PRIMARY KEY, 
		[ShapeID] int NOT NULL FOREIGN KEY REFERENCES Shape(ShapeID), 
		[ProductID] int NOT NULL FOREIGN KEY REFERENCES Product(ProductID), 
		[InventoryID] int NOT NULL FOREIGN KEY REFERENCES Inventory(InventoryID), 
		[Quantity] int NOT NULL, 
		[MinQuantity] int NOT NULL,
		ValidFrom DATETIME2 GENERATED ALWAYS AS ROW START NOT NULL, 
		ValidTo DATETIME2 GENERATED ALWAYS AS ROW END NOT NULL, 
		PERIOD FOR SYSTEM_TIME (ValidFrom, ValidTo)
	)
	WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.InventoryItems_History));


-- CREATE AssignedInventory Table
CREATE TABLE dbo.AssignedInventory 
	( 
		[InventoryID] int NOT NULL FOREIGN KEY REFERENCES Inventory(InventoryID), 
		[UserID] int NOT NULL FOREIGN KEY REFERENCES Users(UserID),	
		CONSTRAINT PK_AssignedInventory PRIMARY KEY (InventoryID,UserID)
	);
