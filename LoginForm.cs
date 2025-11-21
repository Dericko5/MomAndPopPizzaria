using System;
using System.Drawing;
using System.Windows.Forms;

namespace BlueberryPizzeria
{
    /// <summary>
    /// Login form for user authentication
    /// Provides access to employee POS or manager dashboard based on credentials
    /// </summary>
    public class LoginForm : Form
    {
        private TextBox usernameTextBox;
        private TextBox passwordTextBox;
        private Button loginButton;
        private Label statusLabel;
        private Label titleLabel;
        private Label usernameLabel;
        private Label passwordLabel;
        private Label infoLabel;

        /// <summary>
        /// Initializes a new instance of the LoginForm
        /// </summary>
        public LoginForm()
        {
            InitializeComponents();
        }

        /// <summary>
        /// Initializes all UI components for the login form
        /// </summary>
        private void InitializeComponents()
        {
            // Form properties
            this.Text = "Blueberry Pizzeria - Login";
            this.Size = new Size(450, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(185, 28, 28); // Red-700

            // Main panel
            Panel mainPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(450, 400),
                BackColor = Color.FromArgb(185, 28, 28)
            };

            // Title label
            titleLabel = new Label
            {
                Text = "🍕 Blueberry Pizzeria",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(50, 30),
                Size = new Size(350, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Form panel (white background)
            Panel formPanel = new Panel
            {
                Location = new Point(50, 90),
                Size = new Size(350, 200),
                BackColor = Color.White
            };

            // Username label
            usernameLabel = new Label
            {
                Text = "Username:",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(30, 20),
                Size = new Size(100, 25)
            };

            // Username textbox
            usernameTextBox = new TextBox
            {
                Font = new Font("Arial", 12),
                Location = new Point(140, 20),
                Size = new Size(180, 25)
            };

            // Password label
            passwordLabel = new Label
            {
                Text = "Password:",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(30, 60),
                Size = new Size(100, 25)
            };

            // Password textbox
            passwordTextBox = new TextBox
            {
                Font = new Font("Arial", 12),
                Location = new Point(140, 60),
                Size = new Size(180, 25),
                PasswordChar = '●'
            };

            // Login button
            loginButton = new Button
            {
                Text = "Login",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(30, 110),
                Size = new Size(290, 40),
                BackColor = Color.FromArgb(22, 163, 74), // Green-600
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            loginButton.FlatAppearance.BorderSize = 0;
            loginButton.Click += LoginButton_Click;

            // Status label
            statusLabel = new Label
            {
                Text = "",
                Font = new Font("Arial", 10),
                ForeColor = Color.Red,
                Location = new Point(30, 160),
                Size = new Size(290, 25),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Info panel
            Panel infoPanel = new Panel
            {
                Location = new Point(50, 300),
                Size = new Size(350, 60),
                BackColor = Color.FromArgb(243, 244, 246)
            };

            // Info label
            infoLabel = new Label
            {
                Text = "Demo Accounts:\nEmployee: mike / password123\nManager: admin / admin123",
                Font = new Font("Arial", 9),
                ForeColor = Color.FromArgb(75, 85, 99),
                Location = new Point(10, 5),
                Size = new Size(330, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };

            // Add controls to form panel
            formPanel.Controls.Add(usernameLabel);
            formPanel.Controls.Add(usernameTextBox);
            formPanel.Controls.Add(passwordLabel);
            formPanel.Controls.Add(passwordTextBox);
            formPanel.Controls.Add(loginButton);
            formPanel.Controls.Add(statusLabel);

            // Add controls to info panel
            infoPanel.Controls.Add(infoLabel);

            // Add controls to main panel
            mainPanel.Controls.Add(titleLabel);
            mainPanel.Controls.Add(formPanel);
            mainPanel.Controls.Add(infoPanel);

            // Add main panel to form
            this.Controls.Add(mainPanel);

            // Set enter key to trigger login
            this.AcceptButton = loginButton;
        }

        /// <summary>
        /// Handles the login button click event
        /// Validates credentials and opens appropriate form
        /// </summary>
        private void LoginButton_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text.Trim();
            string password = passwordTextBox.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                statusLabel.Text = "Please enter username and password";
                return;
            }

            // Check credentials
            if (username == "admin" && password == "admin123")
            {
                // Manager login
                OpenManagerDashboard();
            }
            else if (username == "mike" && password == "password123")
            {
                // Employee login
                OpenPOSSystem();
            }
            else
            {
                statusLabel.Text = "Invalid username or password";
            }
        }

        /// <summary>
        /// Opens the POS system for employees
        /// </summary>
        private void OpenPOSSystem()
        {
            POSForm posForm = new POSForm();
            posForm.FormClosed += (s, args) => this.Close();
            posForm.Show();
            this.Hide();
        }

        /// <summary>
        /// Opens the manager dashboard
        /// </summary>
        private void OpenManagerDashboard()
        {
            ManagerDashboardForm dashboardForm = new ManagerDashboardForm();
            dashboardForm.FormClosed += (s, args) => this.Close();
            dashboardForm.Show();
            this.Hide();
        }
    }
}