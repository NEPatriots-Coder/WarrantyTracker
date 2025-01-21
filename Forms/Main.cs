using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using WarrantyTracker.Models;

namespace WarrantyTracker.Forms
{
    public partial class MainForm : Form
    {
        private TableLayoutPanel? mainLayout;
        private Panel? headerPanel;
        private Label? headerLabel;
        private TextBox? txtItemName;
        private TextBox? txtSerialNumber;
        private DateTimePicker? dtpPurchaseDate;
        private DateTimePicker? dtpWarrantyExpiration;
        private TextBox? txtPurchasePrice;
        private TextBox? txtMaintenanceNotes;
        private Button? btnSubmit;
        private Label? lblValidation;
        private ErrorProvider? errorProvider;

        public MainForm()
        {
            InitializeComponent();
            SetupValidation();
        }

        private void InitializeComponent()
        {
            // Form settings
            this.Text = "Warranty Tracking System";
            this.Size = new Size(800, 600);
            this.MinimumSize = new Size(600, 500);
            this.BackColor = Color.FromArgb(240, 248, 255); // AliceBlue
            this.Font = new Font("Segoe UI", 10F);
            this.Padding = new Padding(10);

            // Initialize ErrorProvider
            errorProvider = new ErrorProvider();
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            // Create main layout
            mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 8,
                Padding = new Padding(20),
                BackColor = Color.White,
            };

            // Header Panel
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(51, 122, 183) // Bootstrap primary blue
            };

            headerLabel = new Label
            {
                Text = "Warranty Item Registration",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(20, 15)
            };
            headerPanel.Controls.Add(headerLabel);

            // Initialize controls with modern styling
            InitializeControls();

            // Add controls to layout
            AddControlsToLayout();

            // Add layout to form
            this.Controls.Add(mainLayout);
            this.Controls.Add(headerPanel);
            mainLayout.Top = headerPanel.Height;
        }

        private void InitializeControls()
        {
            // Create all input controls
            txtItemName = CreateTextBox("Enter item name");
            txtSerialNumber = CreateTextBox("Enter serial number");
            
            dtpPurchaseDate = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Width = 200,
                Font = new Font("Segoe UI", 10F),
                Value = DateTime.Now
            };

            dtpWarrantyExpiration = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                Width = 200,
                Font = new Font("Segoe UI", 10F),
                Value = DateTime.Now.AddYears(1)
            };

            txtPurchasePrice = CreateTextBox("Enter price");

            txtMaintenanceNotes = CreateTextBox("Enter maintenance notes");
            txtMaintenanceNotes.Multiline = true;
            txtMaintenanceNotes.Height = 100;

            btnSubmit = new Button
            {
                Text = "Submit",
                Width = 120,
                Height = 40,
                BackColor = Color.FromArgb(51, 122, 183),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSubmit.Click += btnSubmit_Click;

            lblValidation = new Label
            {
                AutoSize = true,
                ForeColor = Color.Red,
                Font = new Font("Segoe UI", 9F)
            };
        }

        private TextBox CreateTextBox(string placeholder)
        {
            var textBox = new TextBox
            {
                Width = 200,
                Font = new Font("Segoe UI", 10F),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(5),
            };
            
            return textBox;
        }

        private void AddControlsToLayout()
        {
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

            AddFormRow("Item Name:", txtItemName, 0);
            AddFormRow("Serial Number:", txtSerialNumber, 1);
            AddFormRow("Purchase Date:", dtpPurchaseDate, 2);
            AddFormRow("Warranty Expiration:", dtpWarrantyExpiration, 3);
            AddFormRow("Purchase Price:", txtPurchasePrice, 4);
            AddFormRow("Maintenance Notes:", txtMaintenanceNotes, 5);

            // Add submit button in its own row
            var buttonPanel = new Panel { Dock = DockStyle.Fill };
            buttonPanel.Controls.Add(btnSubmit);
            mainLayout.Controls.Add(buttonPanel, 1, 6);

            // Add validation label in the last row
            mainLayout.Controls.Add(lblValidation, 1, 7);
        }

        private void AddFormRow(string labelText, Control? control, int row)
{
    if (control == null) return;
    
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

        private void SetupValidation()
        {
            txtItemName.Validating += (s, e) => ValidateControl(txtItemName, e, "Item name is required");
            txtSerialNumber.Validating += (s, e) => ValidateControl(txtSerialNumber, e, "Serial number is required");
            txtPurchasePrice.Validating += ValidatePurchasePrice;
            dtpWarrantyExpiration.ValueChanged += ValidateWarrantyDate;
        }

        private void ValidateControl(Control? control, CancelEventArgs e, string errorMessage)
{
    if (control == null) return;
    
    if (string.IsNullOrWhiteSpace(control.Text))
    {
        e.Cancel = true;
        errorProvider?.SetError(control, errorMessage);
        lblValidation.Text = errorMessage;
    }
    else
    {
        errorProvider?.SetError(control, "");
        lblValidation.Text = "";
    }
}

     
        private void ValidatePurchasePrice(object sender, CancelEventArgs e)
        {
            if (!decimal.TryParse(txtPurchasePrice.Text, out decimal price) || price <= 0)
            {
                e.Cancel = true;
                errorProvider.SetError(txtPurchasePrice, "Please enter a valid price greater than 0");
                lblValidation.Text = "Please enter a valid price greater than 0";
            }
            else
            {
                errorProvider.SetError(txtPurchasePrice, "");
                lblValidation.Text = "";
            }
        }

        private void ValidateWarrantyDate(object sender, EventArgs e)
        {
            if (dtpWarrantyExpiration.Value < dtpPurchaseDate.Value)
            {
                errorProvider.SetError(dtpWarrantyExpiration, "Warranty date must be after purchase date");
                lblValidation.Text = "Warranty date must be after purchase date";
            }
            else
            {
                errorProvider.SetError(dtpWarrantyExpiration, "");
                lblValidation.Text = "";
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtItemName.Text))
            {
                ShowError("Item name is required.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSerialNumber.Text))
            {
                ShowError("Serial number is required.");
                return false;
            }

            if (!decimal.TryParse(txtPurchasePrice.Text, out decimal price) || price <= 0)
            {
                ShowError("Please enter a valid purchase price greater than 0.");
                return false;
            }

            if (dtpWarrantyExpiration.Value < dtpPurchaseDate.Value)
            {
                ShowError("Warranty expiration date cannot be earlier than purchase date.");
                return false;
            }

            if (dtpPurchaseDate.Value > DateTime.Now)
            {
                ShowError("Purchase date cannot be in the future.");
                return false;
            }

            lblValidation.Text = "";
            return true;
        }

        private void ShowError(string message)
        {
            lblValidation.Text = message;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                var item = new PurchaseItem
                {
                    Name = txtItemName.Text,
                    SerialNumber = txtSerialNumber.Text,
                    PurchaseDate = dtpPurchaseDate.Value,
                    WarrantyExpirationDate = dtpWarrantyExpiration.Value,
                    PurchasePrice = decimal.Parse(txtPurchasePrice.Text),
                    MaintenanceNotes = txtMaintenanceNotes.Text
                };

                // TODO: Add database connectivity in Module 5
                MessageBox.Show("Item successfully saved!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearForm();
            }
        }

        private void ClearForm()
        {
            txtItemName.Clear();
            txtSerialNumber.Clear();
            dtpPurchaseDate.Value = DateTime.Now;
            dtpWarrantyExpiration.Value = DateTime.Now.AddYears(1);
            txtPurchasePrice.Clear();
            txtMaintenanceNotes.Clear();
            lblValidation.Text = "";
            errorProvider.Clear();
        }
    }
}
