using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BlueberryPizzeria.Models;

namespace BlueberryPizzeria
{
    /// <summary>
    /// Main POS form for employee order entry
    /// Provides interface for taking customer orders with full menu customization
    /// </summary>
    public class POSForm : Form
    {
        private Panel menuPanel;
        private Panel cartPanel;
        private Label subtotalLabel;
        private Label taxLabel;
        private Label totalLabel;
        private Button checkoutButton;
        private string currentCategory = "Pizza";
        private List<IOrderItem> cart = new List<IOrderItem>();

        /// <summary>
        /// Initializes a new instance of the POSForm
        /// </summary>
        public POSForm()
        {
            InitializeComponents();
            LoadMenuItems();
        }

        /// <summary>
        /// Initializes all UI components
        /// </summary>
        private void InitializeComponents()
        {
            // Form properties
            this.Text = "Blueberry Pizzeria - Employee POS System";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(243, 244, 246);

            // Header Panel
            Panel headerPanel = CreateHeaderPanel();
            this.Controls.Add(headerPanel);

            // Main content panel
            Panel mainPanel = new Panel
            {
                Location = new Point(0, 80),
                Size = new Size(1200, 720),
                BackColor = Color.FromArgb(243, 244, 246)
            };

            // Left side - Menu
            Panel leftPanel = CreateMenuSection();
            leftPanel.Location = new Point(0, 0);
            leftPanel.Size = new Size(800, 720);
            mainPanel.Controls.Add(leftPanel);

            // Right side - Cart
            cartPanel = CreateCartSection();
            cartPanel.Location = new Point(800, 0);
            cartPanel.Size = new Size(400, 720);
            mainPanel.Controls.Add(cartPanel);

            this.Controls.Add(mainPanel);
        }

        /// <summary>
        /// Creates the header panel with title and buttons
        /// </summary>
        private Panel CreateHeaderPanel()
        {
            Panel panel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(1200, 80),
                BackColor = Color.FromArgb(185, 28, 28) // Red-700
            };

            // Title section
            Label titleLabel = new Label
            {
                Text = "🍕 Blueberry Pizzeria",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 12),
                Size = new Size(400, 38),
                BackColor = Color.Transparent
            };

            Label subtitleLabel = new Label
            {
                Text = "Employee POS System",
                Font = new Font("Arial", 12),
                ForeColor = Color.FromArgb(254, 202, 202), // Red-200
                Location = new Point(20, 50),
                Size = new Size(400, 22),
                BackColor = Color.Transparent
            };

            // Header buttons
            Button kitchenButton = CreateHeaderButton("Kitchen View", new Point(900, 20));
            Button customerButton = CreateHeaderButton("Find Customer", new Point(1020, 20));
            
            kitchenButton.Click += (s, e) => {
                KitchenDisplayForm kitchenForm = new KitchenDisplayForm();
                kitchenForm.Show();
            };
            customerButton.Click += (s, e) => MessageBox.Show("Customer Search - To be implemented", "Info");

            panel.Controls.AddRange(new Control[] { titleLabel, subtitleLabel, kitchenButton, customerButton });

            return panel;
        }

        /// <summary>
        /// Creates a styled header button
        /// </summary>
        private Button CreateHeaderButton(string text, Point location)
        {
            Button button = new Button
            {
                Text = text,
                Font = new Font("Arial", 11, FontStyle.Bold),
                Location = location,
                Size = new Size(110, 40),
                BackColor = Color.FromArgb(153, 27, 27), // Red-800
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        /// <summary>
        /// Creates the menu section with category tabs and items
        /// </summary>
        private Panel CreateMenuSection()
        {
            Panel panel = new Panel
            {
                BackColor = Color.FromArgb(243, 244, 246)
            };

            // Category tabs
            Panel tabPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(800, 50),
                BackColor = Color.White
            };

            string[] categories = { "Pizza", "Sides", "Drinks" };
            int buttonWidth = 266;

            for (int i = 0; i < categories.Length; i++)
            {
                string category = categories[i];
                Button tabButton = new Button
                {
                    Text = category,
                    Font = new Font("Arial", 14, FontStyle.Bold),
                    Location = new Point(i * buttonWidth, 0),
                    Size = new Size(buttonWidth, 50),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand
                };
                tabButton.FlatAppearance.BorderSize = 0;

                if (category == currentCategory)
                {
                    tabButton.BackColor = Color.FromArgb(185, 28, 28);
                    tabButton.ForeColor = Color.White;
                }
                else
                {
                    tabButton.BackColor = Color.FromArgb(229, 231, 235);
                    tabButton.ForeColor = Color.FromArgb(55, 65, 81);
                }

                tabButton.Click += (s, e) => SwitchCategory(category);
                tabPanel.Controls.Add(tabButton);
            }

            // Menu items panel with scroll
            menuPanel = new Panel
            {
                Location = new Point(10, 60),
                Size = new Size(780, 650),
                BackColor = Color.FromArgb(243, 244, 246),
                AutoScroll = true
            };

            panel.Controls.Add(tabPanel);
            panel.Controls.Add(menuPanel);

            return panel;
        }

        /// <summary>
        /// Creates the cart section on the right side
        /// </summary>
        private Panel CreateCartSection()
        {
            Panel panel = new Panel
            {
                BackColor = Color.White
            };

            return panel;
        }

        /// <summary>
        /// Switches between menu categories
        /// </summary>
        private void SwitchCategory(string category)
        {
            currentCategory = category;
            
            // Update tab button colors
            Panel tabPanel = (Panel)menuPanel.Parent.Controls[0];
            foreach (Control control in tabPanel.Controls)
            {
                if (control is Button button)
                {
                    if (button.Text == category)
                    {
                        button.BackColor = Color.FromArgb(185, 28, 28);
                        button.ForeColor = Color.White;
                    }
                    else
                    {
                        button.BackColor = Color.FromArgb(229, 231, 235);
                        button.ForeColor = Color.FromArgb(55, 65, 81);
                    }
                }
            }

            LoadMenuItems();
        }

        /// <summary>
        /// Loads menu items based on current category
        /// </summary>
        private void LoadMenuItems()
        {
            menuPanel.Controls.Clear();

            int cols = 3;
            int itemWidth = 240;
            int itemHeight = 200;
            int spacing = 15;
            int row = 0, col = 0;

            if (currentCategory == "Pizza")
            {
                AddMenuItem("Build Your Pizza", "🍕", "Starting at $5.00", row, col++, itemWidth, itemHeight, spacing, true);
            }
            else if (currentCategory == "Sides")
            {
                string[] sides = { "Bread Sticks|🥖|$4.00", "Bread Stick Bites|🥖|$2.00", "Big Chocolate Chip Cookie|🍪|$4.00" };
                foreach (var side in sides)
                {
                    var parts = side.Split('|');
                    AddMenuItem(parts[0], parts[1], parts[2], row, col++, itemWidth, itemHeight, spacing, false);
                    if (col >= cols) { col = 0; row++; }
                }
            }
            else if (currentCategory == "Drinks")
            {
                string[] drinks = { "Pepsi", "Diet Pepsi", "Orange", "Diet Orange", "Root Beer", "Diet Root Beer", "Starry", "Lemonade" };
                foreach (var drink in drinks)
                {
                    AddMenuItem(drink, "🥤", "$1.75", row, col++, itemWidth, itemHeight, spacing, false);
                    if (col >= cols) { col = 0; row++; }
                }
            }
        }

        /// <summary>
        /// Adds a menu item button to the panel
        /// </summary>
        private void AddMenuItem(string name, string emoji, string price, int row, int col, int width, int height, int spacing, bool isPizza)
        {
            Panel itemPanel = new Panel
            {
                Location = new Point(col * (width + spacing), row * (height + spacing)),
                Size = new Size(width, height),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand
            };

            Label emojiLabel = new Label
            {
                Text = emoji,
                Font = new Font("Segoe UI Emoji", 52),
                Location = new Point(0, 15),
                Size = new Size(width, 80),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label nameLabel = new Label
            {
                Text = name,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(10, 100),
                Size = new Size(width - 20, 45),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label priceLabel = new Label
            {
                Text = price,
                Font = new Font("Arial", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(185, 28, 28),
                Location = new Point(10, 150),
                Size = new Size(width - 20, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            EventHandler clickHandler = (s, e) => {
                if (isPizza)
                {
                    MessageBox.Show("Pizza Customizer - To be fully implemented\nAdding Medium Pizza with Cheese for demo", "Info");
                    var pizza = new PizzaOrder("Medium", "Regular", new List<string> { "Cheese" });
                    cart.Add(pizza);
                    UpdateCart();
                }
                else
                {
                    double itemPrice = double.Parse(price.Replace("$", ""));
                    cart.Add(new SimpleOrderItem(name, itemPrice));
                    UpdateCart();
                }
            };

            // Add click event to panel and all labels
            itemPanel.Click += clickHandler;
            emojiLabel.Click += clickHandler;
            nameLabel.Click += clickHandler;
            priceLabel.Click += clickHandler;

            itemPanel.MouseEnter += (s, e) => itemPanel.BackColor = Color.FromArgb(249, 250, 251);
            itemPanel.MouseLeave += (s, e) => itemPanel.BackColor = Color.White;

            itemPanel.Controls.AddRange(new Control[] { emojiLabel, nameLabel, priceLabel });
            menuPanel.Controls.Add(itemPanel);
        }

        /// <summary>
        /// Updates the cart display with current items
        /// </summary>
        private void UpdateCart()
        {
            // Clear existing cart display
            cartPanel.Controls.Clear();

            // Re-add header
            Panel headerPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(400, 60),
                BackColor = Color.FromArgb(31, 41, 55)
            };

            Label cartLabel = new Label
            {
                Text = "🛒 Current Order",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(15, 17),
                Size = new Size(370, 28),
                BackColor = Color.Transparent
            };
            headerPanel.Controls.Add(cartLabel);
            cartPanel.Controls.Add(headerPanel);

            // Items container with scroll
            Panel itemsContainer = new Panel
            {
                Location = new Point(0, 60),
                Size = new Size(400, 400),
                BackColor = Color.White,
                AutoScroll = true
            };

            double subtotal = 0.0;
            int yPos = 10;

            if (cart.Count == 0)
            {
                Label emptyLabel = new Label
                {
                    Text = "No items in cart",
                    Font = new Font("Arial", 12),
                    ForeColor = Color.FromArgb(156, 163, 175),
                    Location = new Point(0, 150),
                    Size = new Size(400, 30),
                    TextAlign = ContentAlignment.MiddleCenter,
                    BackColor = Color.Transparent
                };
                itemsContainer.Controls.Add(emptyLabel);
            }
            else
            {
                for (int i = 0; i < cart.Count; i++)
                {
                    var item = cart[i];
                    int itemIndex = i; // Capture for lambda

                    Panel itemPanel = new Panel
                    {
                        Location = new Point(10, yPos),
                        Size = new Size(370, 85),
                        BackColor = Color.FromArgb(249, 250, 251),
                        BorderStyle = BorderStyle.FixedSingle
                    };

                    // Item name
                    Label nameLabel = new Label
                    {
                        Text = $"{i + 1}. {item.Name}",
                        Font = new Font("Arial", 11, FontStyle.Bold),
                        Location = new Point(10, 8),
                        Size = new Size(280, 22),
                        BackColor = Color.Transparent
                    };
                    itemPanel.Controls.Add(nameLabel);

                    // Details
                    if (!string.IsNullOrEmpty(item.Details))
                    {
                        Label detailsLabel = new Label
                        {
                            Text = item.Details,
                            Font = new Font("Arial", 9),
                            ForeColor = Color.FromArgb(107, 114, 128),
                            Location = new Point(10, 32),
                            Size = new Size(280, 25),
                            BackColor = Color.Transparent
                        };
                        itemPanel.Controls.Add(detailsLabel);
                    }

                    // Price
                    Label priceLabel = new Label
                    {
                        Text = $"${item.Price:F2}",
                        Font = new Font("Arial", 12, FontStyle.Bold),
                        ForeColor = Color.FromArgb(22, 163, 74),
                        Location = new Point(10, 57),
                        Size = new Size(100, 22),
                        BackColor = Color.Transparent
                    };
                    itemPanel.Controls.Add(priceLabel);

                    // Remove button
                    Button removeButton = new Button
                    {
                        Text = "✕ Remove",
                        Font = new Font("Arial", 9, FontStyle.Bold),
                        Location = new Point(270, 55),
                        Size = new Size(90, 26),
                        BackColor = Color.FromArgb(220, 38, 38),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand
                    };
                    removeButton.FlatAppearance.BorderSize = 0;
                    removeButton.Click += (s, e) => {
                        cart.RemoveAt(itemIndex);
                        UpdateCart();
                    };
                    itemPanel.Controls.Add(removeButton);

                    itemsContainer.Controls.Add(itemPanel);
                    yPos += 95;
                    subtotal += item.Price;
                }
            }

            cartPanel.Controls.Add(itemsContainer);

            // Totals panel
            Panel totalsPanel = new Panel
            {
                Location = new Point(0, 460),
                Size = new Size(400, 140),
                BackColor = Color.FromArgb(249, 250, 251)
            };

            Panel separatorLine = new Panel
            {
                Location = new Point(20, 0),
                Size = new Size(360, 2),
                BackColor = Color.FromArgb(229, 231, 235)
            };
            totalsPanel.Controls.Add(separatorLine);

            subtotalLabel = new Label
            {
                Text = $"Subtotal: ${subtotal:F2}",
                Font = new Font("Arial", 14),
                Location = new Point(20, 20),
                Size = new Size(360, 28),
                BackColor = Color.Transparent
            };

            taxLabel = new Label
            {
                Text = $"Tax (8%): ${(subtotal * 0.08):F2}",
                Font = new Font("Arial", 14),
                Location = new Point(20, 50),
                Size = new Size(360, 28),
                BackColor = Color.Transparent
            };

            Panel separator2 = new Panel
            {
                Location = new Point(20, 80),
                Size = new Size(360, 2),
                BackColor = Color.FromArgb(229, 231, 235)
            };
            totalsPanel.Controls.Add(separator2);

            totalLabel = new Label
            {
                Text = $"Total: ${(subtotal * 1.08):F2}",
                Font = new Font("Arial", 22, FontStyle.Bold),
                ForeColor = Color.FromArgb(185, 28, 28),
                Location = new Point(20, 90),
                Size = new Size(360, 38),
                BackColor = Color.Transparent
            };

            totalsPanel.Controls.AddRange(new Control[] { subtotalLabel, taxLabel, totalLabel });
            cartPanel.Controls.Add(totalsPanel);

            // Checkout button
            checkoutButton = new Button
            {
                Text = "Proceed to Payment",
                Font = new Font("Arial", 16, FontStyle.Bold),
                Location = new Point(0, 600),
                Size = new Size(400, 60),
                BackColor = Color.FromArgb(22, 163, 74),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = cart.Count > 0
            };
            checkoutButton.FlatAppearance.BorderSize = 0;
            checkoutButton.Click += CheckoutButton_Click;
            cartPanel.Controls.Add(checkoutButton);
        }

        /// <summary>
        /// Handles checkout button click
        /// </summary>
        private void CheckoutButton_Click(object sender, EventArgs e)
        {
            if (cart.Count == 0)
            {
                MessageBox.Show("Cart is empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double total = cart.Sum(item => item.Price) * 1.08;
            MessageBox.Show($"Order Total: ${total:F2}\n\nReceipt generation will be fully implemented in next phase.\n\nOrder placed successfully!", 
                          "Checkout", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Clear cart
            cart.Clear();
            UpdateCart();
        }
    }
}