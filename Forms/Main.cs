using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using WarrantyTracker.Models;
using WarrantyTracker.Data;
using Microsoft.EntityFrameworkCore;

namespace WarrantyTracker.Forms
{
    public partial class MainForm : Form
    {
        #region Control Declarations
        private readonly TableLayoutPanel mainLayout = new();
        private readonly Panel headerPanel = new();
        private readonly Label headerLabel = new();
        private readonly TextBox txtItemName = new();
        private readonly TextBox txtSerialNumber = new();
        private readonly DateTimePicker dtpPurchaseDate = new();
        private readonly DateTimePicker dtpWarrantyExpiration = new();
        private readonly ComboBox cboWarrantyPlan = new();
        private readonly Label lblWarrantyPrice = new();
        private readonly TextBox txtPurchasePrice = new();
        private readonly TextBox txtMaintenanceNotes = new();
        private readonly Button btnSubmit = new();
        private readonly Button btnTestConnection = new();
        private readonly Label lblValidation = new();
        private readonly ErrorProvider errorProvider = new();
        #endregion

        #region Constructor and Initialization
        public MainForm()
        {
            InitializeComponent();
            SetupValidation();
            InitializeWarrantyPlans();
        }

        private void InitializeComponent()
        {
            ConfigureFormSettings();
            ConfigureMainLayout();
            ConfigureHeaderPanel();
            InitializeControls();
            AddControlsToLayout();
            ConfigureTestConnectionButton();
        }

        private void ConfigureFormSettings()
        {
            Text = "Warranty Tracking System";
            Size = new Size(800, 600);
            MinimumSize = new Size(600, 500);
            BackColor = Color.FromArgb(240, 248, 255);
            Font = new Font("Segoe UI", 10F);
            Padding = new Padding(10);
        }

        private void ConfigureMainLayout()
        {
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.ColumnCount = 2;
            mainLayout.RowCount = 10;
            mainLayout.Padding = new Padding(20);
            mainLayout.BackColor = Color.White;
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
        }

        private void ConfigureHeaderPanel()
        {
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 60;
            headerPanel.BackColor = Color.FromArgb(51, 122, 183);

            headerLabel.Text = "Warranty Item Registration";
            headerLabel.ForeColor = Color.White;
            headerLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            headerLabel.AutoSize = true;
            headerLabel.Location = new Point(20, 15);

            headerPanel.Controls.Add(headerLabel);
        }

        private void InitializeControls()
        {
            ConfigureTextBox(txtItemName, "Enter item name");
            ConfigureTextBox(txtSerialNumber, "Enter serial number");
            ConfigureDatePicker(dtpPurchaseDate);
            ConfigureDatePicker(dtpWarrantyExpiration);
            ConfigureWarrantyPlanDropdown();
            ConfigureWarrantyPriceLabel();
            ConfigureTextBox(txtPurchasePrice, "Enter price");
            ConfigureMaintenanceNotesBox();
            ConfigureSubmitButton();
        }

        private void ConfigureWarrantyPlanDropdown()
        {
            cboWarrantyPlan.Width = 300;
            cboWarrantyPlan.Font = new Font("Segoe UI", 10F);
            cboWarrantyPlan.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void ConfigureWarrantyPriceLabel()
        {
            lblWarrantyPrice.AutoSize = true;
            lblWarrantyPrice.Font = new Font("Segoe UI", 10F);
            lblWarrantyPrice.ForeColor = Color.FromArgb(51, 122, 183);
        }

        private void ConfigureTextBox(TextBox textBox, string placeholder)
        {
            textBox.Width = 200;
            textBox.Font = new Font("Segoe UI", 10F);
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Padding = new Padding(5);
            textBox.PlaceholderText = placeholder;
        }

        private void ConfigureDatePicker(DateTimePicker picker)
        {
            picker.Format = DateTimePickerFormat.Short;
            picker.Width = 200;
            picker.Font = new Font("Segoe UI", 10F);
        }

        private void ConfigureMaintenanceNotesBox()
        {
            ConfigureTextBox(txtMaintenanceNotes, "Enter maintenance notes");
            txtMaintenanceNotes.Multiline = true;
            txtMaintenanceNotes.Height = 100;
        }

        private void ConfigureSubmitButton()
        {
            btnSubmit.Text = "Submit";
            btnSubmit.Width = 120;
            btnSubmit.Height = 40;
            btnSubmit.BackColor = Color.FromArgb(51, 122, 183);
            btnSubmit.ForeColor = Color.White;
            btnSubmit.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnSubmit.FlatStyle = FlatStyle.Flat;
            btnSubmit.Click += btnSubmit_Click;
        }

        private void ConfigureTestConnectionButton()
        {
            btnTestConnection.Text = "Test Database Connection";
            btnTestConnection.Width = 150;
            btnTestConnection.Height = 40;
            btnTestConnection.BackColor = Color.LightBlue;
            btnTestConnection.FlatStyle = FlatStyle.Flat;
            btnTestConnection.Click += btnTestConnection_Click;
            btnTestConnection.Location = new Point(10, 10);
        }

        private void AddControlsToLayout()
        {
            Controls.Add(mainLayout);
            Controls.Add(headerPanel);
            Controls.Add(btnTestConnection);
            mainLayout.Top = headerPanel.Height;

            AddFormRow("Item Name:", txtItemName, 0);
            AddFormRow("Serial Number:", txtSerialNumber, 1);
            AddFormRow("Purchase Date:", dtpPurchaseDate, 2);
            AddFormRow("Warranty Expiration:", dtpWarrantyExpiration, 3);
            AddFormRow("Warranty Plan:", cboWarrantyPlan, 4);
            
            mainLayout.Controls.Add(lblWarrantyPrice, 1, 5);
            AddFormRow("Purchase Price:", txtPurchasePrice, 6);
            AddFormRow("Maintenance Notes:", txtMaintenanceNotes, 7);

            var buttonPanel = new Panel { Dock = DockStyle.Fill };
            buttonPanel.Controls.Add(btnSubmit);
            mainLayout.Controls.Add(buttonPanel, 1, 8);
            mainLayout.Controls.Add(lblValidation, 1, 9);
        }

        private void AddFormRow(string labelText, Control control, int row)
        {
            var label = new Label
            {
                Text = labelText,
                AutoSize = true,
                Anchor = AnchorStyles.Left | AnchorStyles.Top,
                Padding = new Padding(5)
            };

            mainLayout.Controls.Add(label, 0, row);
            mainLayout.Controls.Add(control, 1, row);
        }
        #endregion

        #region Warranty Plans
        public record WarrantyPlan(string Duration, decimal Price, int Days)
        {
            public override string ToString() => $"{Duration} (${Price:F2})";
        }

        private void InitializeWarrantyPlans()
        {
            var plans = new[]
            {
                new WarrantyPlan("30 Day Warranty", 7.99M, 30),
                new WarrantyPlan("60 Day Warranty", 14.99M, 60),
                new WarrantyPlan("90 Day Warranty", 24.99M, 90)
            };

            cboWarrantyPlan.DataSource = plans;
            cboWarrantyPlan.SelectedIndexChanged += WarrantyPlan_SelectedIndexChanged;
        }

        private void WarrantyPlan_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cboWarrantyPlan.SelectedItem is WarrantyPlan plan)
            {
                dtpWarrantyExpiration.Value = dtpPurchaseDate.Value.AddDays(plan.Days);
                lblWarrantyPrice.Text = $"Warranty Price: ${plan.Price:F2}";
            }
        }
        #endregion

        #region Database Operations
        private async void btnTestConnection_Click(object? sender, EventArgs e)
        {
            try
            {
                using var context = new WarrantyContext();
                Console.WriteLine("Testing database connection...");
                
                var connectionString = context.Database.GetConnectionString();
                Console.WriteLine($"Using connection string: {connectionString}");
                
                var canConnect = await context.Database.CanConnectAsync();
                Console.WriteLine($"Can connect to database: {canConnect}");
                
                if (canConnect)
                {
                    Console.WriteLine("Database exists and is accessible");
                    MessageBox.Show("Successfully connected to database!", "Success", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Console.WriteLine("Cannot connect to database");
                    MessageBox.Show("Could not connect to database.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database test error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                MessageBox.Show($"Connection error: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSubmit_Click(object? sender, EventArgs e)
{
    if (!ValidateInput()) return;

    try
    {
        using var context = new WarrantyContext();
        Console.WriteLine("Starting save process...");

        var selectedPlan = cboWarrantyPlan.SelectedItem as WarrantyPlan;
        var item = new PurchaseItem
        {
            ModelID = 1,  // Default ModelID
            ItemName = txtItemName.Text,
            SerialNumber = txtSerialNumber.Text,
            PurchaseDate = dtpPurchaseDate.Value,
            WarrantyExpiration = dtpWarrantyExpiration.Value,
            PurchasePrice = decimal.Parse(txtPurchasePrice.Text),
            WarrantyPrice = selectedPlan?.Price ?? 0M,
            MaintenanceNotes = txtMaintenanceNotes.Text
        };

        Console.WriteLine($"Attempting to save item: {item.ItemName}");
        await context.PurchaseItems.AddAsync(item);
        var result = await context.SaveChangesAsync();
        Console.WriteLine($"Save result: {result}");

        MessageBox.Show("Item saved successfully!", "Success");
        ClearForm();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
        MessageBox.Show($"Error saving item: {ex.Message}", "Error");
    }
}
        #endregion

        #region Validation
        private void SetupValidation()
        {
            txtItemName.Validating += (s, e) => ValidateControl(txtItemName, e, "Item name is required");
            txtSerialNumber.Validating += (s, e) => ValidateControl(txtSerialNumber, e, "Serial number is required");
            txtPurchasePrice.Validating += ValidatePurchasePrice;
            dtpWarrantyExpiration.ValueChanged += ValidateWarrantyDate;
        }

        private void ValidateControl(Control control, CancelEventArgs e, string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(control.Text))
            {
                e.Cancel = true;
                errorProvider.SetError(control, errorMessage);
                ShowError(errorMessage);
            }
            else
            {
                errorProvider.SetError(control, "");
                lblValidation.Text = "";
            }
        }

        private void ValidatePurchasePrice(object? sender, CancelEventArgs e)
        {
            if (!decimal.TryParse(txtPurchasePrice.Text, out decimal price) || price <= 0)
            {
                e.Cancel = true;
                errorProvider.SetError(txtPurchasePrice, "Please enter a valid price greater than 0");
                ShowError("Please enter a valid price greater than 0");
            }
            else
            {
                errorProvider.SetError(txtPurchasePrice, "");
                lblValidation.Text = "";
            }
        }

        private void ValidateWarrantyDate(object? sender, EventArgs e)
        {
            if (dtpWarrantyExpiration.Value < dtpPurchaseDate.Value)
            {
                errorProvider.SetError(dtpWarrantyExpiration, "Warranty date must be after purchase date");
                ShowError("Warranty date must be after purchase date");
            }
            else
            {
                errorProvider.SetError(dtpWarrantyExpiration, "");
                lblValidation.Text = "";
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtItemName.Text) ||
                string.IsNullOrWhiteSpace(txtSerialNumber.Text) ||
                !decimal.TryParse(txtPurchasePrice.Text, out decimal price) ||
                price <= 0 ||
                dtpWarrantyExpiration.Value < dtpPurchaseDate.Value ||
                dtpPurchaseDate.Value > DateTime.Now)
            {
                ShowError("Please check all fields are filled correctly.");
                return false;
            }
            return true;
        }
        #endregion

        #region Utility Methods
        private void ShowError(string message)
        {
            lblValidation.Text = message;
            lblValidation.ForeColor = Color.Red;
        }

        private void ClearForm()
        {
            txtItemName.Clear();
            txtSerialNumber.Clear();
            dtpPurchaseDate.Value = DateTime.Now;
            dtpWarrantyExpiration.Value = DateTime.Now;
            txtPurchasePrice.Clear();
            txtMaintenanceNotes.Clear();
            lblValidation.Text = "";
            errorProvider.Clear();
        }
        #endregion
    }
}