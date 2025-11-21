using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MomAndPopPizzaria
{
    /// <summary>
    /// Customer search dialog for looking up existing customers
    /// Allows searching by phone number or email
    /// </summary>
    /// 
    public class CustomerSearchForm : Form
    {
        private TextBox searchBox;
        private Panel resultsPanel;
        private Label statusLabel;
        private Customer selectedCustomer = null;
        private List<Customer> customers = new List<Customer>();


        /// <summary>
        /// Initializes a new instance of the CustomerSearchForm
        /// </summary>
        /// 
        public CustomerSearchForm()
        {
            LoadSampleCustomers();
            InitializeComponents();
        }

        /// <summary>
        /// Initializes all UI components
        /// </summary>
        private void InitializeComponents()
        {
            // Form properties
            this.Text = "Customer Search";
            this.Size = new Size(600, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            // Title
            Label titleLabel = new Label
            {
                Text = "👤 Find Customer",
                Font = new Font("Arial", 20, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(560, 35),
                BackColor = Color.Transparent
            };
            this.Controls.Add(titleLabel);

            // Search box
            Label searchLabel = new Label
            {
                Text = "Search by phone or email:",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(20, 70),
                Size = new Size(560, 22),
                BackColor = Color.Transparent
            };
            this.Controls.Add(searchLabel);

            searchBox = new TextBox
            {
                Font = new Font("Arial", 13),
                Location = new Point(20, 100),
                Size = new Size(420, 30)
            };
            searchBox.TextChanged += (s, e) => PerformSearch();
            this.Controls.Add(searchBox);

            // Search button
            Button searchButton = new Button
            {
                Text = "Search",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(450, 98),
                Size = new Size(110, 32),
                BackColor = Color.FromArgb(185, 28, 28),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            searchButton.FlatAppearance.BorderSize = 0;
            searchButton.Click += (s, e) => PerformSearch();
            this.Controls.Add(searchButton);

            // Guest button
            Button guestButton = new Button
            {
                Text = "Continue as Guest",
                Font = new Font("Arial", 11, FontStyle.Bold),
                Location = new Point(20, 140),
                Size = new Size(180, 35),
                BackColor = Color.FromArgb(107, 114, 128),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            guestButton.FlatAppearance.BorderSize = 0;
            guestButton.Click += (s, e) =>
            {
                selectedCustomer = new Customer("Guest", "", "", "");
                this.Close();
            };
            this.Controls.Add(guestButton);

            // Status label
            statusLabel = new Label
            {
                Text = "Enter phone number or email to search",
                Font = new Font("Arial", 11),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(20, 185),
                Size = new Size(560, 22),
                BackColor = Color.Transparent
            };
            this.Controls.Add(statusLabel);

            // Results panel
            resultsPanel = new Panel
            {
                Location = new Point(20, 215),
                Size = new Size(540, 220),
                BackColor = Color.FromArgb(249, 250, 251),
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = true
            };
            this.Controls.Add(resultsPanel);

            // Close button
            Button closeButton = new Button
            {
                Text = "Close",
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(460, 445),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(107, 114, 128),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.Click += (s, e) => this.Close();
            this.Controls.Add(closeButton);
        }

        /// <summary>
        /// Performs customer search based on input
        /// </summary>
        private void PerformSearch()
        {
            resultsPanel.Controls.Clear();
            string searchText = searchBox.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(searchText))
            {
                statusLabel.Text = "Enter phone number or email to search";
                statusLabel.ForeColor = Color.FromArgb(107, 114, 128);
                return;
            }

            // Search customers
            List<Customer> results = customers.Where(c =>
                c.Phone.ToLower().Contains(searchText) ||
                c.Email.ToLower().Contains(searchText) ||
                c.Name.ToLower().Contains(searchText)
            ).ToList();

            if (results.Count == 0)
            {
                statusLabel.Text = string.Format("No customers found matching '{0}'", searchText);
                statusLabel.ForeColor = Color.FromArgb(220, 38, 38);

                // Show "Create New Customer" option
                // Show "Create New Customer" option
                Panel createPanel = new Panel
                {
                    Location = new Point(10, 10),
                    Size = new Size(500, 80),
                    BackColor = Color.White,
                    BorderStyle = BorderStyle.FixedSingle,
                    Cursor = Cursors.Hand
                };

                Label createLabel = new Label
                {
                    Text = "➕ Create New Customer",
                    Font = new Font("Arial", 13, FontStyle.Bold),
                    ForeColor = Color.FromArgb(22, 163, 74),
                    Location = new Point(15, 15),
                    Size = new Size(470, 25),
                    BackColor = Color.Transparent
                };

                Label createSubLabel = new Label
                {
                    Text = "Click here to add a new customer to the system",
                    Font = new Font("Arial", 10),
                    ForeColor = Color.FromArgb(107, 114, 128),
                    Location = new Point(15, 42),
                    Size = new Size(470, 20),
                    BackColor = Color.Transparent
                };

                // ONE handler that everyone uses
                EventHandler createClick = (s, e) =>
                {
                    MessageBox.Show(
                        "Customer creation feature - To be fully implemented",
                        "Info",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                };

                createPanel.Click += createClick;
                createLabel.Click += createClick;
                createSubLabel.Click += createClick;

                createPanel.Controls.Add(createLabel);
                createPanel.Controls.Add(createSubLabel);
                resultsPanel.Controls.Add(createPanel);

            }
            else
            {
                statusLabel.Text = string.Format("Found {0} customer{1}", results.Count, results.Count > 1 ? "s" : "");
                statusLabel.ForeColor = Color.FromArgb(22, 163, 74);

                int yPos = 10;
                foreach (Customer customer in results)
                {
                    Panel customerPanel = CreateCustomerCard(customer, yPos);
                    resultsPanel.Controls.Add(customerPanel);
                    yPos += 110;
                }
            }
        }

        /// <summary>
        /// Creates a customer card for search results
        /// </summary>
        private Panel CreateCustomerCard(Customer customer, int yPos)
        {
            Panel panel = new Panel
            {
                Location = new Point(10, yPos),
                Size = new Size(500, 95),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand
            };

            // Customer icon
            Label iconLabel = new Label
            {
                Text = "👤",
                Font = new Font("Segoe UI Emoji", 32),
                Location = new Point(15, 25),
                Size = new Size(50, 50),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            panel.Controls.Add(iconLabel);

            // Name
            Label nameLabel = new Label
            {
                Text = customer.Name,
                Font = new Font("Arial", 13, FontStyle.Bold),
                Location = new Point(75, 10),
                Size = new Size(410, 25),
                BackColor = Color.Transparent
            };
            panel.Controls.Add(nameLabel);

            // Phone
            Label phoneLabel = new Label
            {
                Text = string.Format("📞 {0}", customer.Phone),
                Font = new Font("Arial", 10),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(75, 38),
                Size = new Size(200, 20),
                BackColor = Color.Transparent
            };
            panel.Controls.Add(phoneLabel);

            // Email
            Label emailLabel = new Label
            {
                Text = string.Format("✉️ {0}", customer.Email),
                Font = new Font("Arial", 10),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(75, 60),
                Size = new Size(300, 20),
                BackColor = Color.Transparent
            };
            panel.Controls.Add(emailLabel);

            // Select button
            Button selectButton = new Button
            {
                Text = "Select",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(400, 30),
                Size = new Size(85, 32),
                BackColor = Color.FromArgb(22, 163, 74),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            selectButton.FlatAppearance.BorderSize = 0;
            selectButton.Click += (s, e) =>
            {
                selectedCustomer = customer;
                this.Close();
            };
            panel.Controls.Add(selectButton);

            panel.MouseEnter += (s, e) => panel.BackColor = Color.FromArgb(249, 250, 251);
            panel.MouseLeave += (s, e) => panel.BackColor = Color.White;

            return panel;
        }

        /// <summary>
        /// Loads sample customers for demonstration
        /// </summary>
        private void LoadSampleCustomers()
        {
            customers.Add(new Customer("John Smith", "555-0123", "john.smith@email.com", "123 Main St, Anytown"));
            customers.Add(new Customer("Sarah Johnson", "555-0456", "sarah.j@email.com", "456 Oak Ave, Springfield"));
            customers.Add(new Customer("Mike Davis", "555-0789", "mike.davis@email.com", "789 Pine Rd, Riverside"));
            customers.Add(new Customer("Emily White", "555-0111", "emily.white@email.com", "111 Elm St, Lakeside"));
            customers.Add(new Customer("Tom Brown", "555-0222", "tom.brown@email.com", "222 Maple Dr, Hilltown"));
            customers.Add(new Customer("Lisa Anderson", "555-0333", "lisa.a@email.com", "333 Cedar Ln, Meadowview"));
            customers.Add(new Customer("Robert Lee", "555-0444", "robert.lee@email.com", "444 Birch Way, Sunset"));
            customers.Add(new Customer("Nancy Green", "555-0555", "nancy.green@email.com", "555 Spruce Ct, Parkville"));
        }

        /// <summary>
        /// Gets the selected customer
        /// </summary>
        public Customer GetSelectedCustomer()
        {
            return selectedCustomer;
        }
    }

    /// <summary>
    /// Customer data class
    /// </summary>
    public class Customer
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public Customer(string name, string phone, string email, string address)
        {
            Name = name;
            Phone = phone;
            Email = email;
            Address = address;
        }
    }
}