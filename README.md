# Warranty Tracking System

A Windows Forms application for tracking product warranties and maintenance information.

## Project Structure

### Configuration Files
- `appsettings.json` - Contains database connection string and logging configuration
- `WarrantyTracker.csproj` - Project configuration including NuGet package references

### Data Layer
- `Data/WarrantyContext.cs` - Entity Framework database context, handles database connection and model configuration
- `Data/DataVerification.cs` - Provides methods to verify data was saved correctly

### Models
- `Models/PurchaseItem.cs` - Main entity model representing a product warranty record
- `Models/MaintenanceRequest.cs` - Entity model for maintenance requests
- `Models/Product.cs` - Base product information model
- `Models/ProductModel.cs` - Product model/category information
- `Models/Warranty.cs` - Detailed warranty information

### Forms
- `Forms/Main.cs` - Main Windows Form containing the UI and business logic
  - Handles user input validation
  - Manages warranty plan selection
  - Processes database operations
  - Provides real-time feedback

### Program Entry
- `Program.cs` - Application entry point, initializes the Windows Form and database connection

### Database Scripts
- `Scripts/Schema.sql` - Contains database creation script and table definitions

## Database Structure

### Main Table: PRODUCT
- `ProductID` (Primary Key)
- `ModelID`
- `ItemName`
- `SerialNumber`
- `PurchaseDate`
- `WarrantyExpiration`
- `PurchasePrice`
- `WarrantyPrice`
- `MaintenanceNotes`
- `CreatedDate`
- `LastModifiedDate`

### View: warranty_status
Provides an easy way to check warranty status of products

## Key Features
- Product warranty registration
- Warranty plan selection
- Data validation
- Database persistence
- Error handling and logging
- Real-time feedback

## Technology Stack
- .NET 8.0
- Windows Forms
- Entity Framework Core
- MySQL Database
- Entity Framework Core MySQL Provider (Pomelo)