using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BlueberryPizzeria.Models;

namespace BlueberryPizzeria
{
    /// <summary>
    /// Kitchen display frame showing active orders
    /// Allows cooks to view orders and update their status
    /// </summary>
    public class KitchenDisplayForm : Form
    {
        private Panel ordersPanel;
        private List<KitchenOrder> orders = new List<KitchenOrder>();

        /// <summary>
        /// Initializes a new instance of the KitchenDisplayForm
        /// </summary>
        public KitchenDisplayForm()
        {
            LoadSampleOrders();
            InitializeComponents();
        }

        /// <summary>
        /// Initializes all UI components
        /// </summary>
        private void InitializeComponents()
        {
            // Form properties
            this.Text = "Blueberry Pizzeria - Kitchen Display";
            this.Size = new Size(1400, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(17, 24, 39); // Gray-900

            // Header
            Panel headerPanel = CreateHeaderPanel();
            this.Controls.Add(headerPanel);

            // Orders grid container
            Panel scrollContainer = new Panel
            {
                Location = new Point(0, 80),
                Size = new Size(1400, 820),
                BackColor = Color.FromArgb(17, 24, 39),
                AutoScroll = true
            };

            ordersPanel = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(1340, 1500),
                BackColor = Color.FromArgb(17, 24, 39)
            };

            scrollContainer.Controls.Add(ordersPanel);
            this.Controls.Add(scrollContainer);

            RefreshOrders();
        }

        /// <summary>
        /// Creates the header panel
        /// </summary>
        private Panel CreateHeaderPanel()
        {
            Panel panel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(1400, 80),
                BackColor = Color.FromArgb(31, 41, 55) // Gray-800
            };

            // Border at bottom
            Panel border = new Panel
            {
                Location = new Point(0, 76),
                Size = new Size(1400, 4),
                BackColor = Color.FromArgb(220, 38, 38) // Red-600
            };
            panel.Controls.Add(border);

            Label titleLabel = new Label
            {
                Text = "🍕 Kitchen Display - Active Orders",
                Font = new Font("Arial", 26, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                Size = new Size(600, 40),
                BackColor = Color.Transparent
            };

            Button closeButton = new Button
            {
                Text = "Close",
                Font = new Font("Arial", 13, FontStyle.Bold),
                Location = new Point(1250, 20),
                Size = new Size(120, 40),
                BackColor = Color.FromArgb(220, 38, 38),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.Click += (s, e) => this.Close();

            panel.Controls.AddRange(new Control[] { titleLabel, closeButton });

            return panel;
        }

        /// <summary>
        /// Refreshes the orders display
        /// </summary>
        private void RefreshOrders()
        {
            ordersPanel.Controls.Clear();

            var activeOrders = orders.FindAll(o => o.Status != "completed");
            
            int xPos = 0;
            int yPos = 0;
            int cardWidth = 420;
            int cardHeight = 450;
            int gap = 20;
            int cardsPerRow = 3;
            int currentCard = 0;

            foreach (var order in activeOrders)
            {
                Panel orderCard = CreateOrderCard(order);
                orderCard.Location = new Point(xPos, yPos);
                orderCard.Size = new Size(cardWidth, cardHeight);
                ordersPanel.Controls.Add(orderCard);

                currentCard++;
                if (currentCard % cardsPerRow == 0)
                {
                    xPos = 0;
                    yPos += cardHeight + gap;
                }
                else
                {
                    xPos += cardWidth + gap;
                }
            }
        }

        /// <summary>
        /// Creates an order card
        /// </summary>
        private Panel CreateOrderCard(KitchenOrder order)
        {
            Panel card = new Panel
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Header based on status
            Panel headerPanel = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(420, 80),
                BackColor = GetStatusColor(order.Status)
            };

            // Status border at bottom of header
            Panel statusBorder = new Panel
            {
                Location = new Point(0, 76),
                Size = new Size(420, 4),
                BackColor = GetStatusBorderColor(order.Status)
            };
            headerPanel.Controls.Add(statusBorder);

            // Left side of header
            Label orderNumLabel = new Label
            {
                Text = $"#{order.OrderNumber}",
                Font = new Font("Arial", 22, FontStyle.Bold),
                ForeColor = GetStatusTextColor(order.Status),
                Location = new Point(15, 10),
                Size = new Size(150, 32),
                BackColor = Color.Transparent
            };

            Label timeLabel = new Label
            {
                Text = order.Time,
                Font = new Font("Arial", 11),
                ForeColor = GetStatusTextColor(order.Status),
                Location = new Point(15, 45),
                Size = new Size(150, 20),
                BackColor = Color.Transparent
            };

            // Right side of header
            Label typeLabel = new Label
            {
                Text = order.Type,
                Font = new Font("Arial", 11, FontStyle.Bold),
                ForeColor = GetStatusTextColor(order.Status),
                Location = new Point(200, 15),
                Size = new Size(200, 22),
                TextAlign = ContentAlignment.TopRight,
                BackColor = Color.Transparent
            };

            Label customerLabel = new Label
            {
                Text = order.Customer,
                Font = new Font("Arial", 10),
                ForeColor = GetStatusTextColor(order.Status),
                Location = new Point(200, 42),
                Size = new Size(200, 20),
                TextAlign = ContentAlignment.TopRight,
                BackColor = Color.Transparent
            };

            headerPanel.Controls.AddRange(new Control[] { orderNumLabel, timeLabel, typeLabel, customerLabel });

            // Items panel
            Panel itemsPanel = new Panel
            {
                Location = new Point(0, 80),
                Size = new Size(420, 290),
                BackColor = Color.White,
                AutoScroll = true
            };

            int itemYPos = 15;
            foreach (var item in order.Items)
            {
                Panel itemCard = new Panel
                {
                    Location = new Point(15, itemYPos),
                    Size = new Size(380, 70),
                    BackColor = Color.FromArgb(249, 250, 251),
                    BorderStyle = BorderStyle.FixedSingle
                };

                // Red left border
                Panel leftBorder = new Panel
                {
                    Location = new Point(0, 0),
                    Size = new Size(4, 70),
                    BackColor = Color.FromArgb(220, 38, 38)
                };
                itemCard.Controls.Add(leftBorder);

                Label itemLabel = new Label
                {
                    Text = item,
                    Font = new Font("Arial", 10),
                    Location = new Point(12, 8),
                    Size = new Size(360, 54),
                    BackColor = Color.Transparent
                };
                itemCard.Controls.Add(itemLabel);

                itemsPanel.Controls.Add(itemCard);
                itemYPos += 80;
            }

            // Action buttons panel
            Panel buttonPanel = new Panel
            {
                Location = new Point(0, 370),
                Size = new Size(420, 80),
                BackColor = Color.FromArgb(243, 244, 246)
            };

            // Top border
            Panel topBorder = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(420, 1),
                BackColor = Color.FromArgb(229, 231, 235)
            };
            buttonPanel.Controls.Add(topBorder);

            Button actionButton = new Button
            {
                Font = new Font("Arial", 13, FontStyle.Bold),
                Location = new Point(15, 20),
                Size = new Size(390, 45),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            actionButton.FlatAppearance.BorderSize = 0;

            if (order.Status == "new")
            {
                actionButton.Text = "Start Preparing";
                actionButton.BackColor = Color.FromArgb(234, 179, 8); // Yellow-500
                actionButton.ForeColor = Color.FromArgb(17, 24, 39);
                actionButton.Click += (s, e) => {
                    order.Status = "preparing";
                    RefreshOrders();
                };
            }
            else if (order.Status == "preparing")
            {
                actionButton.Text = "Mark Ready";
                actionButton.BackColor = Color.FromArgb(22, 163, 74); // Green-600
                actionButton.ForeColor = Color.White;
                actionButton.Click += (s, e) => {
                    order.Status = "ready";
                    RefreshOrders();
                };
            }
            else if (order.Status == "ready")
            {
                actionButton.Text = "Complete Order";
                actionButton.BackColor = Color.FromArgb(107, 114, 128); // Gray-600
                actionButton.ForeColor = Color.White;
                actionButton.Click += (s, e) => {
                    order.Status = "completed";
                    RefreshOrders();
                };
            }

            buttonPanel.Controls.Add(actionButton);

            card.Controls.AddRange(new Control[] { headerPanel, itemsPanel, buttonPanel });

            return card;
        }

        /// <summary>
        /// Gets background color for order status
        /// </summary>
        private Color GetStatusColor(string status)
        {
            return status switch
            {
                "new" => Color.FromArgb(37, 99, 235), // Blue-600
                "preparing" => Color.FromArgb(234, 179, 8), // Yellow-500
                "ready" => Color.FromArgb(22, 163, 74), // Green-600
                _ => Color.FromArgb(156, 163, 175)
            };
        }

        /// <summary>
        /// Gets border color for order status
        /// </summary>
        private Color GetStatusBorderColor(string status)
        {
            return status switch
            {
                "new" => Color.FromArgb(29, 78, 216), // Blue-700
                "preparing" => Color.FromArgb(202, 138, 4), // Yellow-600
                "ready" => Color.FromArgb(21, 128, 61), // Green-700
                _ => Color.FromArgb(107, 114, 128)
            };
        }

        /// <summary>
        /// Gets text color for order status
        /// </summary>
        private Color GetStatusTextColor(string status)
        {
            if (status == "preparing")
            {
                return Color.FromArgb(17, 24, 39); // Dark text for yellow background
            }
            return Color.White;
        }

        /// <summary>
        /// Loads sample orders for display
        /// </summary>
        private void LoadSampleOrders()
        {
            orders.Add(new KitchenOrder(
                1001,
                "John Smith",
                "2:15 PM",
                "Dine-in",
                "preparing",
                new List<string>
                {
                    "1x Large Pizza\nSize: Large, Crust: Regular\nToppings: Cheese, Pepperoni, Sausage",
                    "2x Pepsi - Large",
                    "1x Bread Sticks"
                }
            ));

            orders.Add(new KitchenOrder(
                1002,
                "Sarah Johnson",
                "2:18 PM",
                "Delivery",
                "new",
                new List<string>
                {
                    "2x Medium Pizza\nSize: Medium, Crust: Thin\nToppings: Cheese, Pepperoni",
                    "1x Big Chocolate Chip Cookie"
                }
            ));

            orders.Add(new KitchenOrder(
                1003,
                "Mike Davis",
                "2:10 PM",
                "Pickup",
                "ready",
                new List<string>
                {
                    "1x Extra Large Pizza\nSize: Extra Large, Crust: Pan\nToppings: Cheese, Pepperoni, Sausage, Mushroom, Green Pepper",
                    "2x Bread Stick Bites"
                }
            ));

            orders.Add(new KitchenOrder(
                1004,
                "Guest",
                "2:20 PM",
                "Dine-in",
                "new",
                new List<string>
                {
                    "1x Small Pizza\nSize: Small, Crust: Regular\nToppings: Cheese"
                }
            ));
        }
    }
}