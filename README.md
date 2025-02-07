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

Testing Front and Back End

# Warranty Tracking System - GUI Test Cases

## Test Case 1: Valid Purchase Item Entry
### Test Scenario: 
Submit a new purchase item with valid data for all required fields

### Test Case:
Verify that the system successfully processes and validates a complete purchase item entry

### Test Steps:
1. Enter "Samsung TV QN65Q80B" in Item Name field
2. Enter "SN2024XYZ789" in Serial Number field
3. Set Purchase Date to current date
4. Set Warranty Expiration to current date + 1 year
5. Enter "999.99" in Purchase Price field
6. Enter "Initial setup completed" in Maintenance Notes
7. Click Submit button

### Test Data:
- Item Name: Samsung TV QN65Q80B
- Serial Number: SN2024XYZ789
- Purchase Date: 02/07/2024
- Warranty Expiration: 02/07/2025
- Purchase Price: 999.99
- Maintenance Notes: Initial setup completed

### Expected Result:
- Success message displayed: "Item successfully saved!"
- Form fields cleared after submission
- No validation errors shown

### Actual Result:
- Success message displayed as expected
- All form fields cleared
- No validation errors shown

### Pass/Fail:
PASS

## Test Case 2: Boundary Value Testing - Purchase Price
### Test Scenario:
Test system handling of boundary values for purchase price field

### Test Case:
Verify that the system properly validates minimum and maximum purchase price values

### Test Steps:
1. Enter "Test Item" in Item Name field
2. Enter "TEST123" in Serial Number field
3. Set Purchase Date to current date
4. Set Warranty Expiration to current date + 30 days
5. Enter "0.00" in Purchase Price field
6. Click Submit button
7. Document error message
8. Enter "999999999.99" in Purchase Price field
9. Click Submit button

### Test Data:
Test 1:
- Purchase Price: 0.00
Test 2:
- Purchase Price: 999999999.99

### Expected Result:
Test 1:
- Error message: "Please enter a valid purchase price greater than 0."
- Form submission prevented
Test 2:
- Form submission successful (testing upper boundary)

### Actual Result:
Test 1:
- Error message displayed as expected
- Form submission prevented
Test 2:
- Large value accepted and processed correctly

### Pass/Fail:
PASS

## Test Case 3: Date Validation Testing
### Test Scenario:
Test system validation of purchase and warranty expiration dates

### Test Case:
Verify that the system properly validates date relationships and future dates

### Test Steps:
1. Enter "Test Product" in Item Name field
2. Enter "DATE123" in Serial Number field
3. Set Purchase Date to future date (tomorrow)
4. Set Warranty Expiration to current date
5. Click Submit button
6. Document error message
7. Set Purchase Date to current date
8. Set Warranty Expiration to date before Purchase Date
9. Click Submit button

### Test Data:
Test 1:
- Purchase Date: 02/08/2025 (future date)
- Warranty Expiration: 02/07/2025
Test 2:
- Purchase Date: 02/07/2025
- Warranty Expiration: 02/06/2025

### Expected Result:
Test 1:
- Error message: "Purchase date cannot be in the future."
- Form submission prevented
Test 2:
- Error message: "Warranty expiration date cannot be earlier than purchase date."
- Form submission prevented

### Actual Result:
Test 1:
- Error displayed as expected
- Submission prevented
Test 2:
- Error displayed as expected
- Submission prevented

### Pass/Fail:
PASS

---

## Test Documentation Summary

The test cases above verify the core functionality of the Warranty Tracking System GUI front-end. Each test case focuses on specific aspects of the application:

1. **Basic Functionality Test**: Verifies the happy path scenario where all inputs are valid and the form processes correctly.

2. **Boundary Value Test**: Focuses on edge cases in the purchase price field, ensuring proper validation of minimum and maximum values.

3. **Date Validation Test**: Ensures proper handling of date relationships and future date restrictions.

These test cases validate:
- Input field validation
- Error message display
- Form submission logic
- Data validation rules
- User interface feedback
- Form reset functionality

The testing framework uses both positive and negative testing approaches to ensure robust validation and error handling. All test cases execute against the Windows Forms application developed in Module 4, utilizing the existing validation logic and UI components.

# Warranty Tracking System - Back-End Test Cases

## Test Case 1: Database Connection and Context Initialization
### Test Scenario:
Verify that WarrantyContext successfully establishes database connection and configures entity relationships

### Test Case:
Test database connection initialization and configuration loading

### Test Steps:
1. Initialize new WarrantyContext instance
2. Attempt database connection
3. Verify configuration loading
4. Check entity configuration
5. Validate logging setup

### Test Data:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=WarrantyDB;User=test;Password=test;"
  }
}
```

### Expected Result:
- Context successfully initialized
- Database connection established
- Entity configurations properly mapped
- Logging system activated

### Actual Result:
- Context initialization successful
- Connection established to test database
- Entity relationships configured
- Logging operational

### Pass/Fail:
PASS

## Test Case 2: PurchaseItem Data Persistence
### Test Scenario:
Test the complete cycle of saving and retrieving a PurchaseItem through the data layer

### Test Case:
Verify that PurchaseItem entity can be saved to database and retrieved with proper data validation

### Test Steps:
1. Create new PurchaseItem instance with test data
2. Save to database using WarrantyContext
3. Retrieve saved item using serial number
4. Verify all properties match
5. Test data validation rules

### Test Data:
```csharp
var testItem = new PurchaseItem
{
    ModelID = 1,
    ItemName = "Test Product",
    SerialNumber = "TEST-2024-001",
    PurchaseDate = DateTime.UtcNow,
    WarrantyExpiration = DateTime.UtcNow.AddYears(1),
    PurchasePrice = 499.99M,
    WarrantyPrice = 49.99M,
    MaintenanceNotes = "Initial test entry"
};
```

### Expected Result:
- Item successfully persisted to database
- Retrieved item matches all input properties
- Created/Modified dates automatically set
- Foreign key constraints maintained

### Actual Result:
- Data persistence successful
- Retrieved data matches input
- Timestamps correctly set
- Relationships maintained

### Pass/Fail:
PASS

## Test Case 3: DataVerification Service Boundary Testing
### Test Scenario:
Test the DataVerification service with edge cases and invalid data

### Test Case:
Verify that the DataVerification service properly handles validation edge cases and error conditions

### Test Steps:
1. Test verification with null values
2. Test with duplicate serial numbers
3. Test with invalid date ranges
4. Verify error logging
5. Check exception handling

### Test Data:
```csharp
// Test Case 1: Null Values
var nullItem = new PurchaseItem { SerialNumber = null };

// Test Case 2: Duplicate Serial
var duplicate1 = new PurchaseItem { SerialNumber = "DUPLICATE-001" };
var duplicate2 = new PurchaseItem { SerialNumber = "DUPLICATE-001" };

// Test Case 3: Invalid Dates
var invalidDates = new PurchaseItem 
{
    PurchaseDate = DateTime.UtcNow,
    WarrantyExpiration = DateTime.UtcNow.AddDays(-1)
};
```

### Expected Result:
- Null values properly rejected with validation messages
- Duplicate serial numbers detected and prevented
- Invalid dates trigger appropriate validation errors
- All errors properly logged

### Actual Result:
- Null validation functioning correctly
- Duplicate detection working as expected
- Date validation properly enforced
- Error logging comprehensive

### Pass/Fail:
PASS

---

## Test Documentation Summary

These back-end test cases verify the core functionality of the data layer and service components:

### Data Layer Testing
1. **Database Context**:
   - Connection management
   - Configuration loading
   - Entity mapping
   - Relationship configuration

2. **Entity Framework Features**:
   - Automatic change tracking
   - Transaction management
   - Constraint enforcement
   - Lazy loading behavior

### Service Layer Testing
1. **Data Verification Service**:
   - Input validation
   - Business rule enforcement
   - Error handling
   - Logging functionality

### Key Validation Points:
- Data persistence accuracy
- Referential integrity
- Validation rule enforcement
- Error handling robustness
- Transaction consistency
- Service layer integration

