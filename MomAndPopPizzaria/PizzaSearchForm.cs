using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BlueberryPizzeria.Models;

namespace BlueberryPizzeria
{
    /// <summary>
    /// Dialog for customizing pizza orders
    /// Allows selection of size, crust type, and toppings with real-time pricing
    /// </summary>
    public class PizzaCustomizerForm : Form
    {
        private string selectedSize = "Medium";
        private string selectedCrust = "Regular";
        private List<string> selectedToppings = new List<string>();
        private Label priceLabel;
        private Panel toppingsPanel;
        private bool confirmed = false;

        // Available pizza sizes with prices
        private static readonly string[] SIZES = { "Small", "Medium", "Large", "Extra Large" };
        private static readonly double[] SIZE_PRICES = { 5.0, 7.0, 9.0, 11.0 };
        private static readonly double[] TOPPING_PRICES = { 0.75, 1.0, 1.25, 1.50 };

        // Available crust types
        private static readonly string[] CRUSTS = { "Thin", "Regular", "Pan" };

        // Available toppings
        private static readonly string[] TOPPINGS = {
            "Cheese", "Pepperoni", "Sausage", "Ham",
            "Green Pepper", "Onion", "Tomato", "Mushroom", "Pineapple"
        };

        /// <summary>
        /// Initializes a new instance of the PizzaCustomizerForm
        /// </summary>
        public PizzaCustomizerForm()
        {
            // Start with cheese as default topping
            selectedToppings.Add("Cheese");
            InitializeComponents();
            UpdatePrice();
        }

        /// <summary>
        /// Initializes all UI components
        /// </summary>
        private void InitializeComponents()
        {
            // Form properties
            this.Text = "Build Your Pizza";
            this.Size = new Size(700, 680);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            Panel scrollContainer = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(700, 580),
                AutoScroll = true,
                BackColor = Color.White
            };

            Panel contentPanel = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(640, 900),
                BackColor = Color.White
            };

            int yPos = 0;

            // Title
            Label titleLabel = new Label
            {
                Text = "🍕 Build Your Pizza",
                Font = new Font("Arial", 22, FontStyle.Bold),
                Location = new Point(0, yPos),
                Size = new Size(640, 35),
                BackColor = Color.Transparent
            };
            contentPanel.Controls.Add(titleLabel);
            yPos += 50;

            // Size Section
            contentPanel.Controls.Add(CreateSizePanel(yPos));
            yPos += 120;

            // Crust Section
            contentPanel.Controls.Add(CreateCrustPanel(yPos));
            yPos += 100;

            // Toppings Section
            Panel toppingsSection = CreateToppingsPanel(yPos);
            contentPanel.Controls.Add(toppingsSection);

            scrollContainer.Controls.Add(contentPanel);
            this.Controls.Add(scrollContainer);

            // Bottom panel with price and buttons
            Panel bottomPanel = CreateBottomPanel();
            this.Controls.Add(bottomPanel);
        }

        /// <summary>
        /// Creates size selection panel
        /// </summary>
        private Panel CreateSizePanel(int yPos)
        {
            Panel panel = new Panel
            {
                Location = new Point(0, yPos),
                Size = new Size(640, 100),
                BackColor = Color.Transparent
            };

            Label label = new Label
            {
                Text = "Select Size:",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(0, 0),
                Size = new Size(640, 25),
                BackColor = Color.Transparent
            };
            panel.Controls.Add(label);

            int buttonWidth = 150;
            int gap = 10;
            int xPos = 0;

            for (int i = 0; i < SIZES.Length; i++)
            {
                string size = SIZES[i];
                double price = SIZE_PRICES[i];

                Button button = new Button
                {
                    Text = string.Format("{0}\n${1:F2}", size, price),
                    Font = new Font("Arial", 11, FontStyle.Bold),
                    Location = new Point(xPos, 35),
                    Size = new Size(buttonWidth, 55),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    BackColor = size == selectedSize ? Color.FromArgb(185, 28, 28) : Color.FromArgb(229, 231, 235),
                    ForeColor = size == selectedSize ? Color.White : Color.Black
                };
                button.FlatAppearance.BorderSize = 0;

                button.Click += (s, e) =>
                {
                    selectedSize = size;
                    UpdatePrice();
                    UpdateSizeButtons(panel);
                };

                panel.Controls.Add(button);
                xPos += buttonWidth + gap;
            }

            return panel;
        }

        /// <summary>
        /// Creates crust selection panel
        /// </summary>
        private Panel CreateCrustPanel(int yPos)
        {
            Panel panel = new Panel
            {
                Location = new Point(0, yPos),
                Size = new Size(640, 80),
                BackColor = Color.Transparent
            };

            Label label = new Label
            {
                Text = "Select Crust:",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(0, 0),
                Size = new Size(640, 25),
                BackColor = Color.Transparent
            };
            panel.Controls.Add(label);

            int buttonWidth = 200;
            int gap = 20;
            int xPos = 0;

            foreach (string crust in CRUSTS)
            {
                Button button = new Button
                {
                    Text = crust,
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    Location = new Point(xPos, 35),
                    Size = new Size(buttonWidth, 40),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    BackColor = crust == selectedCrust ? Color.FromArgb(185, 28, 28) : Color.FromArgb(229, 231, 235),
                    ForeColor = crust == selectedCrust ? Color.White : Color.Black
                };
                button.FlatAppearance.BorderSize = 0;

                button.Click += (s, e) =>
                {
                    selectedCrust = crust;
                    UpdateCrustButtons(panel);
                };

                panel.Controls.Add(button);
                xPos += buttonWidth + gap;
            }

            return panel;
        }

        /// <summary>
        /// Creates toppings selection panel
        /// </summary>
        private Panel CreateToppingsPanel(int yPos)
        {
            Panel panel = new Panel
            {
                Location = new Point(0, yPos),
                Size = new Size(640, 320),
                BackColor = Color.Transparent
            };

            Label label = new Label
            {
                Text = "Select Toppings (First topping free after cheese):",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(0, 0),
                Size = new Size(640, 25),
                BackColor = Color.Transparent
            };
            panel.Controls.Add(label);

            toppingsPanel = new Panel
            {
                Location = new Point(0, 35),
                Size = new Size(640, 280),
                BackColor = Color.Transparent
            };

            int buttonWidth = 200;
            int buttonHeight = 40;
            int gapX = 20;
            int gapY = 10;
            int col = 0;
            int row = 0;

            foreach (string topping in TOPPINGS)
            {
                Button button = new Button
                {
                    Text = topping,
                    Font = new Font("Arial", 11, FontStyle.Bold),
                    Location = new Point(col * (buttonWidth + gapX), row * (buttonHeight + gapY)),
                    Size = new Size(buttonWidth, buttonHeight),
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    BackColor = selectedToppings.Contains(topping) ? Color.FromArgb(185, 28, 28) : Color.FromArgb(229, 231, 235),
                    ForeColor = selectedToppings.Contains(topping) ? Color.White : Color.Black
                };
                button.FlatAppearance.BorderSize = 0;

                button.Click += (s, e) =>
                {
                    if (selectedToppings.Contains(topping))
                    {
                        selectedToppings.Remove(topping);
                        button.BackColor = Color.FromArgb(229, 231, 235);
                        button.ForeColor = Color.Black;
                    }
                    else
                    {
                        selectedToppings.Add(topping);
                        button.BackColor = Color.FromArgb(185, 28, 28);
                        button.ForeColor = Color.White;
                    }
                    UpdatePrice();
                };

                toppingsPanel.Controls.Add(button);

                col++;
                if (col >= 3)
                {
                    col = 0;
                    row++;
                }
            }

            panel.Controls.Add(toppingsPanel);
            return panel;
        }

        /// <summary>
        /// Creates bottom panel with price display and action buttons
        /// </summary>
        private Panel CreateBottomPanel()
        {
            Panel panel = new Panel
            {
                Location = new Point(0, 580),
                Size = new Size(700, 100),
                BackColor = Color.FromArgb(219, 234, 254), // Blue-100
                BorderStyle = BorderStyle.FixedSingle
            };

            // Price display
            priceLabel = new Label
            {
                Text = "Current Price: $7.00",
                Font = new Font("Arial", 18, FontStyle.Bold),
                Location = new Point(20, 20),
                Size = new Size(350, 30),
                BackColor = Color.Transparent
            };
            panel.Controls.Add(priceLabel);

            // Buttons
            Button cancelButton = new Button
            {
                Text = "Cancel",
                Font = new Font("Arial", 13, FontStyle.Bold),
                Location = new Point(420, 25),
                Size = new Size(120, 45),
                BackColor = Color.FromArgb(107, 114, 128),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            cancelButton.FlatAppearance.BorderSize = 0;
            cancelButton.Click += (s, e) => this.Close();

            Button addButton = new Button
            {
                Text = "Add to Cart",
                Font = new Font("Arial", 13, FontStyle.Bold),
                Location = new Point(550, 25),
                Size = new Size(130, 45),
                BackColor = Color.FromArgb(22, 163, 74),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            addButton.FlatAppearance.BorderSize = 0;
            addButton.Click += (s, e) => ConfirmOrder();

            panel.Controls.Add(cancelButton);
            panel.Controls.Add(addButton);

            return panel;
        }

        /// <summary>
        /// Updates size button colors
        /// </summary>
        private void UpdateSizeButtons(Panel sizePanel)
        {
            foreach (Control control in sizePanel.Controls)
            {
                if (control is Button button)
                {
                    string buttonSize = button.Text.Split('\n')[0];
                    if (buttonSize == selectedSize)
                    {
                        button.BackColor = Color.FromArgb(185, 28, 28);
                        button.ForeColor = Color.White;
                    }
                    else
                    {
                        button.BackColor = Color.FromArgb(229, 231, 235);
                        button.ForeColor = Color.Black;
                    }
                }
            }
        }

        /// <summary>
        /// Updates crust button colors
        /// </summary>
        private void UpdateCrustButtons(Panel crustPanel)
        {
            foreach (Control control in crustPanel.Controls)
            {
                if (control is Button button)
                {
                    if (button.Text == selectedCrust)
                    {
                        button.BackColor = Color.FromArgb(185, 28, 28);
                        button.ForeColor = Color.White;
                    }
                    else
                    {
                        button.BackColor = Color.FromArgb(229, 231, 235);
                        button.ForeColor = Color.Black;
                    }
                }
            }
        }

        /// <summary>
        /// Calculates and updates the current price
        /// </summary>
        private void UpdatePrice()
        {
            int sizeIndex = GetSizeIndex();
            double price = SIZE_PRICES[sizeIndex];
            double toppingPrice = TOPPING_PRICES[sizeIndex];

            // First 2 toppings free (cheese + 1), rest charged
            int extraToppings = Math.Max(0, selectedToppings.Count - 2);
            price += extraToppings * toppingPrice;

            string priceText = string.Format("Current Price: ${0:F2}", price);
            if (extraToppings > 0)
            {
                priceText += string.Format(" ({0} extra topping{1})", extraToppings, extraToppings > 1 ? "s" : "");
            }

            priceLabel.Text = priceText;
        }

        /// <summary>
        /// Gets size index for pricing
        /// </summary>
        private int GetSizeIndex()
        {
            for (int i = 0; i < SIZES.Length; i++)
            {
                if (SIZES[i] == selectedSize)
                {
                    return i;
                }
            }
            return 1; // Default to medium
        }

        /// <summary>
        /// Confirms the pizza order
        /// </summary>
        private void ConfirmOrder()
        {
            if (selectedToppings.Count == 0)
            {
                MessageBox.Show("Please select at least one topping!", "No Toppings Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            confirmed = true;
            this.Close();
        }

        /// <summary>
        /// Checks if order was confirmed
        /// </summary>
        public bool IsConfirmed()
        {
            return confirmed;
        }

        /// <summary>
        /// Gets the constructed pizza order
        /// </summary>
        public PizzaOrder GetPizzaOrder()
        {
            return new PizzaOrder(selectedSize, selectedCrust, selectedToppings);
        }
    }
}