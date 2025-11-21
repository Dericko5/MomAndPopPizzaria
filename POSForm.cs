using BlueberryPizzeria.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BlueberryPizzeria.Models
{
    // Simple placeholder models so the form can compile and run

    public interface IOrderItem
    {
        string Name { get; }
        string Details { get; }
        double Price { get; }
    }

    public class SimpleOrderItem : IOrderItem
    {
        public string Name { get; private set; }
        public string Details { get; private set; }
        public double Price { get; private set; }

        public SimpleOrderItem(string name, double price, string details = "")
        {
            Name = name;
            Price = price;
            Details = details;
        }
    }

    public class PizzaOrder : IOrderItem
    {
        public string Size { get; private set; }
        public string Crust { get; private set; }
        public List<string> Toppings { get; private set; }

        public string Name => $"{Size} Pizza";

        public string Details
        {
            get
            {
                string toppingText = (Toppings != null && Toppings.Count > 0)
                    ? string.Join(", ", Toppings)
                    : "None";

                return $"Crust: {Crust}; Toppings: {toppingText}";
            }
        }

        public double Price
        {
            get
            {
                // Very simple placeholder pricing logic
                double basePrice;
                switch (Size)
                {
                    case "Small": basePrice = 5.00; break;
                    case "Large": basePrice = 9.00; break;
                    default: basePrice = 7.00; break;  // Medium or anything else
                }

                double toppingCost = (Toppings != null ? Toppings.Count : 0) * 0.50;
                return basePrice + toppingCost;
            }
        }

        public PizzaOrder(string size, string crust, List<string> toppings)
        {
            Size = size;
            Crust = crust;
            Toppings = toppings ?? new List<string>();
        }
    }
}

namespace BlueberryPizzeria
{
    public class POSForm : Form
    {
        private Panel menuPanel;
        private Panel cartPanel;
        private RichTextBox cartTextBox;
        private Label subtotalLabel;
        private Label taxLabel;
        private Label totalLabel;
        private Button checkoutButton;
        private string currentCategory = "Pizza";
        private List<IOrderItem> cart = new List<IOrderItem>();

        public POSForm()
        {
            InitializeComponents();
            LoadMenuItems();
        }

        private void InitializeComponents()
        {
            this.Text = "Blueberry Pizzeria - Employee POS System";
            this.Size = new Size(1200, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(243, 244, 246);

            Panel headerPanel = CreateHeaderPanel();
            this.Controls.Add(headerPanel);

            Panel mainPanel = new Panel
            {
                Location = new Point(0, 80),
                Size = new Size(1200, 720),
                BackColor = Color.FromArgb(243, 244, 246)
            };

            Panel leftPanel = CreateMenuSection();
            leftPanel.Location = new Point(0, 0);
            leftPanel.Size = new Size(800, 720);
            mainPanel.Controls.Add(leftPanel);

            cartPanel = CreateCartSection();
            cartPanel.Location = new Point(800, 0);
            cartPanel.Size = new Size(400, 720);
            mainPanel.Controls.Add(cartPanel);

            this.Controls.Add(mainPanel);
        }

        private Panel CreateHeaderPanel()
        {
            Panel panel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(1200, 80),
                BackColor = Color.FromArgb(185, 28, 28) // Red-700
            };

            Label titleLabel = new Label
            {
                Text = "Blueberry Pizzeria",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 15),
                Size = new Size(400, 35),
                BackColor = Color.Transparent
            };

            Label subtitleLabel = new Label
            {
                Text = "Employee POS System",
                Font = new Font("Arial", 12),
                ForeColor = Color.FromArgb(254, 202, 202), // Red-200
                Location = new Point(20, 50),
                Size = new Size(400, 20),
                BackColor = Color.Transparent
            };

            Button kitchenButton = CreateHeaderButton("Kitchen View", new Point(900, 20));
            Button customerButton = CreateHeaderButton("Find Customer", new Point(1020, 20));

            kitchenButton.Click += (s, e) =>
                MessageBox.Show("Kitchen Display - To be implemented", "Info");

            customerButton.Click += (s, e) =>
                MessageBox.Show("Customer Search - To be implemented", "Info");

            panel.Controls.AddRange(new Control[] { titleLabel, subtitleLabel, kitchenButton, customerButton });

            return panel;
        }

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

        private Panel CreateMenuSection()
        {
            Panel panel = new Panel
            {
                BackColor = Color.FromArgb(243, 244, 246)
            };

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

        private Panel CreateCartSection()
        {
            Panel panel = new Panel
            {
                BackColor = Color.White
            };

            Panel headerPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(400, 60),
                BackColor = Color.FromArgb(31, 41, 55) // Gray-800
            };

            Label cartLabel = new Label
            {
                Text = "🛒 Current Order",
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(15, 18),
                Size = new Size(370, 25),
                BackColor = Color.Transparent
            };

            headerPanel.Controls.Add(cartLabel);

            cartTextBox = new RichTextBox
            {
                Location = new Point(0, 60),
                Size = new Size(400, 400),
                Font = new Font("Courier New", 10),
                ReadOnly = true,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            Panel totalsPanel = new Panel
            {
                Location = new Point(0, 460),
                Size = new Size(400, 140),
                BackColor = Color.FromArgb(249, 250, 251)
            };

            subtotalLabel = new Label
            {
                Text = "Subtotal: $0.00",
                Font = new Font("Arial", 14),
                Location = new Point(20, 15),
                Size = new Size(360, 25)
            };

            taxLabel = new Label
            {
                Text = "Tax (8%): $0.00",
                Font = new Font("Arial", 14),
                Location = new Point(20, 45),
                Size = new Size(360, 25)
            };

            totalLabel = new Label
            {
                Text = "Total: $0.00",
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(185, 28, 28),
                Location = new Point(20, 80),
                Size = new Size(360, 35)
            };

            totalsPanel.Controls.AddRange(new Control[] { subtotalLabel, taxLabel, totalLabel });

            checkoutButton = new Button
            {
                Text = "Proceed to Payment",
                Font = new Font("Arial", 16, FontStyle.Bold),
                Location = new Point(0, 600),
                Size = new Size(400, 60),
                BackColor = Color.FromArgb(22, 163, 74), // Green-600
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Enabled = false
            };
            checkoutButton.FlatAppearance.BorderSize = 0;
            checkoutButton.Click += CheckoutButton_Click;

            panel.Controls.AddRange(new Control[] { headerPanel, cartTextBox, totalsPanel, checkoutButton });

            return panel;
        }

        private void SwitchCategory(string category)
        {
            currentCategory = category;

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
                AddMenuItem("Build Your Pizza", "🍕", "Starting at $5.00",
                    row, col++, itemWidth, itemHeight, spacing, true);
            }
            else if (currentCategory == "Sides")
            {
                string[] sides =
                {
                    "Bread Sticks|🥖|$4.00",
                    "Bread Stick Bites|🥖|$2.00",
                    "Big Chocolate Chip Cookie|🍪|$4.00"
                };

                foreach (var side in sides)
                {
                    var parts = side.Split('|');
                    AddMenuItem(parts[0], parts[1], parts[2],
                        row, col++, itemWidth, itemHeight, spacing, false);
                    if (col >= cols)
                    {
                        col = 0;
                        row++;
                    }
                }
            }
            else if (currentCategory == "Drinks")
            {
                string[] drinks =
                {
                    "Pepsi", "Diet Pepsi", "Orange", "Diet Orange",
                    "Root Beer", "Diet Root Beer", "Starry", "Lemonade"
                };

                foreach (var drink in drinks)
                {
                    AddMenuItem(drink, "🥤", "$1.75",
                        row, col++, itemWidth, itemHeight, spacing, false);
                    if (col >= cols)
                    {
                        col = 0;
                        row++;
                    }
                }
            }
        }

        private void AddMenuItem(
            string name,
            string emoji,
            string price,
            int row,
            int col,
            int width,
            int height,
            int spacing,
            bool isPizza)
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
                Font = new Font("Arial", 48),
                Location = new Point(0, 20),
                Size = new Size(width, 70),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label nameLabel = new Label
            {
                Text = name,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(10, 100),
                Size = new Size(width - 20, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label priceLabel = new Label
            {
                Text = price,
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(185, 28, 28),
                Location = new Point(10, 145),
                Size = new Size(width - 20, 25),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            EventHandler clickHandler = (s, e) =>
            {
                if (isPizza)
                {
                    MessageBox.Show(
                        "Pizza Customizer - To be fully implemented\nAdding Medium Pizza with Cheese for demo",
                        "Info");

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

            itemPanel.Click += clickHandler;
            emojiLabel.Click += clickHandler;
            nameLabel.Click += clickHandler;
            priceLabel.Click += clickHandler;
            menuPanel.Controls.Add(itemPanel);
        }

        private void UpdateCart()
        {
            cartTextBox.Clear();
            double subtotal = 0.0;

            for (int i = 0; i < cart.Count; i++)
            {
                var item = cart[i];
                cartTextBox.AppendText($"{i + 1}. {item.Name}\n");
                if (!string.IsNullOrEmpty(item.Details))
                {
                    cartTextBox.AppendText($"   {item.Details}\n");
                }
                cartTextBox.AppendText($"   ${item.Price:F2}\n\n");
                subtotal += item.Price;
            }

            double tax = subtotal * 0.06;
            double total = subtotal + tax;

            subtotalLabel.Text = $"Subtotal: ${subtotal:F2}";
            taxLabel.Text = $"Tax (6%): ${tax:F2}";
            totalLabel.Text = $"Total: ${total:F2}";

            checkoutButton.Enabled = cart.Count > 0;
        }

        private void CheckoutButton_Click(object sender, EventArgs e)
        {
            if (cart.Count == 0)
            {
                MessageBox.Show("Cart is empty!", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double total = cart.Sum(item => item.Price) * 1.08;
            MessageBox.Show(
                $"Order Total: ${total:F2}\n\nReceipt generation will be fully implemented in next phase.\n\nOrder placed successfully!",
                "Checkout", MessageBoxButtons.OK, MessageBoxIcon.Information);

            cart.Clear();
            UpdateCart();
        }
    }
}
