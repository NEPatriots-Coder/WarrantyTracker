-- Create PRODUCT_MODEL table
CREATE TABLE PRODUCT_MODEL (
    ModelID INT IDENTITY(1,1) PRIMARY KEY,
    ModelName NVARCHAR(100) NOT NULL,
    Manufacturer NVARCHAR(100) NOT NULL,
    Category NVARCHAR(50) NOT NULL,
    Specifications NVARCHAR(MAX),
    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),
    LastModifiedDate DATETIME2 DEFAULT GETUTCDATE()
);

-- Create PRODUCT table
CREATE TABLE PRODUCT (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    ModelID INT NOT NULL,
    SerialNumber NVARCHAR(50) NOT NULL,
    PurchaseDate DATETIME2 NOT NULL,
    PurchasePrice DECIMAL(18,2) NOT NULL,
    RetailerName NVARCHAR(100),
    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),
    LastModifiedDate DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Product_Model FOREIGN KEY (ModelID) 
        REFERENCES PRODUCT_MODEL(ModelID),
    CONSTRAINT UQ_SerialNumber UNIQUE (SerialNumber)
);

-- Create WARRANTY table
CREATE TABLE WARRANTY (
    WarrantyID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT NOT NULL,
    WarrantyType NVARCHAR(50) NOT NULL,
    StartDate DATETIME2 NOT NULL,
    EndDate DATETIME2 NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    Terms NVARCHAR(MAX),
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),
    LastModifiedDate DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Warranty_Product FOREIGN KEY (ProductID) 
        REFERENCES PRODUCT(ProductID)
);

-- Create MAINTENANCE_REQUEST table
CREATE TABLE MAINTENANCE_REQUEST (
    RequestID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT NOT NULL,
    RequestDate DATETIME2 NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Status NVARCHAR(20) NOT NULL 
        CONSTRAINT CK_MaintenanceStatus 
        CHECK (Status IN ('Pending', 'InProgress', 'Completed', 'Cancelled')),
    Resolution NVARCHAR(MAX),
    CompletionDate DATETIME2,
    Cost DECIMAL(18,2),
    TechnicianNotes NVARCHAR(MAX),
    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),
    LastModifiedDate DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Maintenance_Product FOREIGN KEY (ProductID) 
        REFERENCES PRODUCT(ProductID)
);