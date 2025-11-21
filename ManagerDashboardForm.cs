using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BlueberryPizzeria.Models;

namespace BlueberryPizzeria
{
    /// <summary>
    /// Manager dashboard for viewing sales, orders, and employee status
    /// Provides comprehensive overview of restaurant operations
    /// </summary>
    public class ManagerDashboardForm : Form
    {
        private List<EmployeeData> employees = new List<EmployeeData>();
        private List<OrderData> orders = new List<OrderData>();
        private Panel mainContentPanel;

        /// <summary>
        /// Initializes a new instance of the ManagerDashboardForm
        /// </summary>
        public ManagerDashboardForm()
        {
            LoadSampleData();
            InitializeComponents();
        }

        /// <summary>
        /// Initializes all UI components
        /// </summary>
        private void InitializeComponents()
        {
            // Form properties
            this.Text = "Blueberry Pizzeria - Manager Dashboard";
            this.Size = new Size(1400, 900);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(243, 244, 246);

            // Header
            Panel headerPanel = CreateHeaderPanel();
            this.Controls.Add(headerPanel);

            // Scrollable main content
            Panel scrollContainer = new Panel
            {
                Location = new Point(0, 80),
                Size = new Size(1400, 820),
                AutoScroll = true,
                BackColor = Color.FromArgb(243, 244, 246)
            };

            mainContentPanel = CreateMainContent();
            scrollContainer.Controls.Add(mainContentPanel);

            this.Controls.Add(scrollContainer);
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
                BackColor = Color.FromArgb(22, 163, 74) // Green-700
            };

            Label titleLabel = new Label
            {
                Text = "🍕 Manager Dashboard",
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 15),
                Size = new Size(400, 35),
                BackColor = Color.Transparent
            };

            Label subtitleLabel = new Label
            {
                Text = "Blueberry Pizzeria - Complete Operations View",
                Font = new Font("Arial", 12),
                ForeColor = Color.FromArgb(220, 252, 231), // Green-100
                Location = new Point(20, 50),
                Size = new Size(500, 20),
                BackColor = Color.Transparent
            };

            Label dateLabel = new Label
            {
                Text = "Today's Date",
                Font = new Font("Arial", 10),
                ForeColor = Color.FromArgb(220, 252, 231),
                Location = new Point(1200, 20),
                Size = new Size(180, 15),
                TextAlign = ContentAlignment.TopRight,
                BackColor = Color.Transparent
            };

            Label timeLabel = new Label
            {
                Text = DateTime.Now.ToString("MMMM dd, yyyy - h:mm tt"),
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(1000, 40),
                Size = new Size(380, 25),
                TextAlign = ContentAlignment.TopRight,
                BackColor = Color.Transparent
            };

            panel.Controls.AddRange(new Control[] { titleLabel, subtitleLabel, dateLabel, timeLabel });

            return panel;
        }

        /// <summary>
        /// Creates the main content panel with all dashboard sections
        /// </summary>
        private Panel CreateMainContent()
        {
            Panel panel = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(1340, 2000),
                BackColor = Color.FromArgb(243, 244, 246)
            };

            int leftWidth = 880;
            int rightWidth = 400;
            int gap = 20;
            int yPos = 0;

            // Left column - Sales and Orders
            Panel leftColumn = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(leftWidth, 2000),
                BackColor = Color.Transparent
            };

            // Metrics
            Panel metricsPanel = CreateMetricsPanel();
            metricsPanel.Location = new Point(0, yPos);
            leftColumn.Controls.Add(metricsPanel);
            yPos += 220;

            // Order Status
            Panel orderStatusPanel = CreateOrderStatusPanel();
            orderStatusPanel.Location = new Point(0, yPos);
            leftColumn.Controls.Add(orderStatusPanel);
            yPos += 200;

            // Sales by Type
            Panel salesTypePanel = CreateSalesByTypePanel();
            salesTypePanel.Location = new Point(0, yPos);
            leftColumn.Controls.Add(salesTypePanel);
            yPos += 280;

            // Recent Orders
            Panel recentOrdersPanel = CreateRecentOrdersPanel();
            recentOrdersPanel.Location = new Point(0, yPos);
            leftColumn.Controls.Add(recentOrdersPanel);

            // Right column - Employee Status
            Panel rightColumn = new Panel
            {
                Location = new Point(leftWidth + gap, 0),
                Size = new Size(rightWidth, 2000),
                BackColor = Color.Transparent
            };

            yPos = 0;

            // Staff Status
            Panel staffStatusPanel = CreateStaffStatusPanel();
            staffStatusPanel.Location = new Point(0, yPos);
            rightColumn.Controls.Add(staffStatusPanel);
            yPos += 360;

            // All Employees
            Panel employeesPanel = CreateEmployeeListPanel();
            employeesPanel.Location = new Point(0, yPos);
            rightColumn.Controls.Add(employeesPanel);
            yPos += 500;

            // Top Performers
            Panel performersPanel = CreateTopPerformersPanel();
            performersPanel.Location = new Point(0, yPos);
            rightColumn.Controls.Add(performersPanel);

            panel.Controls.AddRange(new Control[] { leftColumn, rightColumn });

            return panel;
        }

        /// <summary>
        /// Creates the key metrics panel
        /// </summary>
        private Panel CreateMetricsPanel()
        {
            Panel panel = new Panel
            {
                Size = new Size(880, 200),
                BackColor = Color.Transparent
            };

            double totalRevenue = orders.Sum(o => o.Total);
            int totalOrders = orders.Count;
            int activeOrders = orders.Count(o => o.Status != "completed");
            double avgOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0;

            int cardWidth = 210;
            int gap = 10;

            panel.Controls.Add(CreateMetricCard("Today's Revenue", $"${totalRevenue:F2}", "💵", Color.FromArgb(22, 163, 74), 0));
            panel.Controls.Add(CreateMetricCard("Total Orders Today", totalOrders.ToString(), "🛒", Color.FromArgb(37, 99, 235), cardWidth + gap));
            panel.Controls.Add(CreateMetricCard("Active Orders", activeOrders.ToString(), "⏰", Color.FromArgb(234, 179, 8), (cardWidth + gap) * 2));
            panel.Controls.Add(CreateMetricCard("Avg Order Value", $"${avgOrderValue:F2}", "📈", Color.FromArgb(147, 51, 234), (cardWidth + gap) * 3));

            return panel;
        }

        /// <summary>
        /// Creates a metric card
        /// </summary>
        private Panel CreateMetricCard(string title, string value, string emoji, Color color, int xPos)
        {
            Panel card = new Panel
            {
                Location = new Point(xPos, 0),
                Size = new Size(210, 200),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label titleLabel = new Label
            {
                Text = title,
                Font = new Font("Arial", 11),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(15, 20),
                Size = new Size(180, 40),
                BackColor = Color.Transparent
            };

            Label valueLabel = new Label
            {
                Text = value,
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = color,
                Location = new Point(15, 65),
                Size = new Size(180, 40),
                BackColor = Color.Transparent
            };

            Label emojiLabel = new Label
            {
                Text = emoji,
                Font = new Font("Arial", 36),
                ForeColor = color,
                Location = new Point(15, 120),
                Size = new Size(180, 60),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            card.Controls.AddRange(new Control[] { titleLabel, valueLabel, emojiLabel });

            return card;
        }

        /// <summary>
        /// Creates the order status panel
        /// </summary>
        private Panel CreateOrderStatusPanel()
        {
            Panel panel = CreateSectionPanel("Current Orders Status", 880, 180);

            int newOrders = orders.Count(o => o.Status == "new");
            int preparing = orders.Count(o => o.Status == "preparing");
            int ready = orders.Count(o => o.Status == "ready");

            int cardWidth = 280;
            int gap = 20;
            int yPos = 60;

            panel.Controls.Add(CreateStatusCard("New Orders", newOrders.ToString(), Color.FromArgb(37, 99, 235), gap, yPos));
            panel.Controls.Add(CreateStatusCard("Preparing", preparing.ToString(), Color.FromArgb(234, 179, 8), gap + cardWidth + gap, yPos));
            panel.Controls.Add(CreateStatusCard("Ready", ready.ToString(), Color.FromArgb(22, 163, 74), gap + (cardWidth + gap) * 2, yPos));

            return panel;
        }

        /// <summary>
        /// Creates a status card
        /// </summary>
        private Panel CreateStatusCard(string label, string count, Color color, int xPos, int yPos)
        {
            Panel card = new Panel
            {
                Location = new Point(xPos, yPos),
                Size = new Size(280, 100),
                BackColor = Color.FromArgb(color.R, color.G, color.B, 30),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label countLabel = new Label
            {
                Text = count,
                Font = new Font("Arial", 32, FontStyle.Bold),
                ForeColor = color,
                Location = new Point(0, 15),
                Size = new Size(280, 45),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label labelLabel = new Label
            {
                Text = label,
                Font = new Font("Arial", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(75, 85, 99),
                Location = new Point(0, 65),
                Size = new Size(280, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            card.Controls.AddRange(new Control[] { countLabel, labelLabel });

            return card;
        }

        /// <summary>
        /// Creates sales by type panel
        /// </summary>
        private Panel CreateSalesByTypePanel()
        {
            Panel panel = CreateSectionPanel("Sales by Order Type", 880, 260);

            var dineInSales = orders.Where(o => o.Type == "Dine-in").Sum(o => o.Total);
            var pickupSales = orders.Where(o => o.Type == "Pickup").Sum(o => o.Total);
            var deliverySales = orders.Where(o => o.Type == "Delivery").Sum(o => o.Total);

            var dineInCount = orders.Count(o => o.Type == "Dine-in");
            var pickupCount = orders.Count(o => o.Type == "Pickup");
            var deliveryCount = orders.Count(o => o.Type == "Delivery");

            int yPos = 65;
            int gap = 15;

            panel.Controls.Add(CreateSalesTypeRow("Dine-in", dineInCount, dineInSales, Color.FromArgb(37, 99, 235), 20, yPos));
            yPos += 60 + gap;
            panel.Controls.Add(CreateSalesTypeRow("Pickup", pickupCount, pickupSales, Color.FromArgb(22, 163, 74), 20, yPos));
            yPos += 60 + gap;
            panel.Controls.Add(CreateSalesTypeRow("Delivery", deliveryCount, deliverySales, Color.FromArgb(234, 179, 8), 20, yPos));

            return panel;
        }

        /// <summary>
        /// Creates a sales type row
        /// </summary>
        private Panel CreateSalesTypeRow(string type, int count, double sales, Color color, int xPos, int yPos)
        {
            Panel row = new Panel
            {
                Location = new Point(xPos, yPos),
                Size = new Size(840, 60),
                BackColor = Color.FromArgb(color.R, color.G, color.B, 20),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Color bar
            Panel colorBar = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(4, 60),
                BackColor = color
            };

            Label typeLabel = new Label
            {
                Text = type,
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(20, 10),
                Size = new Size(200, 25),
                BackColor = Color.Transparent
            };

            Label countLabel = new Label
            {
                Text = $"{count} orders",
                Font = new Font("Arial", 11),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(20, 35),
                Size = new Size(200, 20),
                BackColor = Color.Transparent
            };

            Label salesLabel = new Label
            {
                Text = $"${sales:F2}",
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = color,
                Location = new Point(640, 15),
                Size = new Size(180, 30),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent
            };

            row.Controls.AddRange(new Control[] { colorBar, typeLabel, countLabel, salesLabel });

            return row;
        }

        /// <summary>
        /// Creates recent orders panel
        /// </summary>
        private Panel CreateRecentOrdersPanel()
        {
            Panel panel = CreateSectionPanel("Recent Orders", 880, 400);

            Panel ordersContainer = new Panel
            {
                Location = new Point(0, 55),
                Size = new Size(880, 345),
                AutoScroll = true,
                BackColor = Color.White
            };

            int yPos = 10;
            foreach (var order in orders.Take(5))
            {
                ordersContainer.Controls.Add(CreateOrderRow(order, yPos));
                yPos += 80;
            }

            panel.Controls.Add(ordersContainer);

            return panel;
        }

        /// <summary>
        /// Creates an order row
        /// </summary>
        private Panel CreateOrderRow(OrderData order, int yPos)
        {
            Panel row = new Panel
            {
                Location = new Point(10, yPos),
                Size = new Size(840, 70),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label orderNumLabel = new Label
            {
                Text = $"#{order.OrderNumber}",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(15, 10),
                Size = new Size(100, 25),
                BackColor = Color.Transparent
            };

            Label statusLabel = new Label
            {
                Text = order.Status.ToUpper(),
                Font = new Font("Arial", 9, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = GetStatusColor(order.Status),
                Location = new Point(120, 13),
                Size = new Size(80, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label customerLabel = new Label
            {
                Text = $"{order.Customer} • {order.Type} • {order.Time}",
                Font = new Font("Arial", 10),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(15, 40),
                Size = new Size(400, 20),
                BackColor = Color.Transparent
            };

            Label totalLabel = new Label
            {
                Text = $"${order.Total:F2}",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 163, 74),
                Location = new Point(720, 15),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent
            };

            Label itemsLabel = new Label
            {
                Text = $"{order.ItemCount} items",
                Font = new Font("Arial", 10),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(720, 42),
                Size = new Size(100, 18),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent
            };

            row.Controls.AddRange(new Control[] { orderNumLabel, statusLabel, customerLabel, totalLabel, itemsLabel });

            return row;
        }

        /// <summary>
        /// Creates staff status panel
        /// </summary>
        private Panel CreateStaffStatusPanel()
        {
            Panel panel = CreateSectionPanel("Staff Status", 400, 340);

            int clockedIn = employees.Count(e => e.ClockedIn);
            int notClockedIn = employees.Count(e => !e.ClockedIn);
            int onBreak = employees.Count(e => e.Status == "on_break");

            int yPos = 65;
            int gap = 15;

            panel.Controls.Add(CreateStaffCard("Clocked In", clockedIn.ToString(), "✓", Color.FromArgb(22, 163, 74), yPos));
            yPos += 85 + gap;
            panel.Controls.Add(CreateStaffCard("Not Clocked In", notClockedIn.ToString(), "✗", Color.FromArgb(220, 38, 38), yPos));
            yPos += 85 + gap;
            panel.Controls.Add(CreateStaffCard("On Break", onBreak.ToString(), "⏰", Color.FromArgb(234, 179, 8), yPos));

            return panel;
        }

        /// <summary>
        /// Creates a staff status card
        /// </summary>
        private Panel CreateStaffCard(string label, string count, string icon, Color color, int yPos)
        {
            Panel card = new Panel
            {
                Location = new Point(15, yPos),
                Size = new Size(370, 85),
                BackColor = Color.FromArgb(color.R, color.G, color.B, 20),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label iconLabel = new Label
            {
                Text = icon,
                Font = new Font("Arial", 28),
                ForeColor = color,
                Location = new Point(15, 20),
                Size = new Size(50, 45),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };

            Label countLabel = new Label
            {
                Text = count,
                Font = new Font("Arial", 24, FontStyle.Bold),
                ForeColor = color,
                Location = new Point(80, 15),
                Size = new Size(100, 35),
                BackColor = Color.Transparent
            };

            Label labelLabel = new Label
            {
                Text = label,
                Font = new Font("Arial", 11),
                ForeColor = Color.FromArgb(75, 85, 99),
                Location = new Point(80, 50),
                Size = new Size(200, 20),
                BackColor = Color.Transparent
            };

            card.Controls.AddRange(new Control[] { iconLabel, countLabel, labelLabel });

            return card;
        }

        /// <summary>
        /// Creates employee list panel
        /// </summary>
        private Panel CreateEmployeeListPanel()
        {
            Panel panel = CreateSectionPanel("All Employees", 400, 480);

            Panel empContainer = new Panel
            {
                Location = new Point(0, 55),
                Size = new Size(400, 425),
                AutoScroll = true,
                BackColor = Color.White
            };

            int yPos = 10;
            foreach (var emp in employees)
            {
                empContainer.Controls.Add(CreateEmployeeRow(emp, yPos));
                yPos += 85;
            }

            panel.Controls.Add(empContainer);

            return panel;
        }

        /// <summary>
        /// Creates an employee row
        /// </summary>
        private Panel CreateEmployeeRow(EmployeeData emp, int yPos)
        {
            Panel row = new Panel
            {
                Location = new Point(10, yPos),
                Size = new Size(360, 75),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label nameLabel = new Label
            {
                Text = emp.Name,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(10, 10),
                Size = new Size(200, 20),
                BackColor = Color.Transparent
            };

            Label roleLabel = new Label
            {
                Text = emp.Role,
                Font = new Font("Arial", 10),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(10, 32),
                Size = new Size(200, 18),
                BackColor = Color.Transparent
            };

            Label statusLabel = new Label
            {
                Text = emp.StatusLabel,
                Font = new Font("Arial", 9, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = emp.StatusColor,
                Location = new Point(10, 52),
                Size = new Size(100, 18),
                TextAlign = ContentAlignment.MiddleCenter
            };

            if (emp.ClockedIn)
            {
                Label ordersLabel = new Label
                {
                    Text = $"{emp.OrdersProcessed} orders",
                    Font = new Font("Arial", 10),
                    ForeColor = Color.FromArgb(107, 114, 128),
                    Location = new Point(250, 10),
                    Size = new Size(100, 18),
                    TextAlign = ContentAlignment.TopRight,
                    BackColor = Color.Transparent
                };

                Label salesLabel = new Label
                {
                    Text = $"${emp.TotalSales:F2}",
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    ForeColor = Color.FromArgb(22, 163, 74),
                    Location = new Point(250, 32),
                    Size = new Size(100, 22),
                    TextAlign = ContentAlignment.TopRight,
                    BackColor = Color.Transparent
                };

                row.Controls.AddRange(new Control[] { ordersLabel, salesLabel });
            }

            row.Controls.AddRange(new Control[] { nameLabel, roleLabel, statusLabel });

            return row;
        }

        /// <summary>
        /// Creates top performers panel
        /// </summary>
        private Panel CreateTopPerformersPanel()
        {
            Panel panel = CreateSectionPanel("Top Performers", 400, 280);

            var topPerformers = employees
                .Where(e => e.ClockedIn && e.Role == "Cashier")
                .OrderByDescending(e => e.TotalSales)
                .Take(3)
                .ToList();

            int yPos = 70;
            int rank = 1;
            foreach (var emp in topPerformers)
            {
                panel.Controls.Add(CreatePerformerRow(emp, rank++, yPos));
                yPos += 65;
            }

            return panel;
        }

        /// <summary>
        /// Creates a performer row
        /// </summary>
        private Panel CreatePerformerRow(EmployeeData emp, int rank, int yPos)
        {
            Panel row = new Panel
            {
                Location = new Point(15, yPos),
                Size = new Size(370, 55),
                BackColor = Color.FromArgb(240, 253, 244),
                BorderStyle = BorderStyle.FixedSingle
            };

            Label rankLabel = new Label
            {
                Text = rank.ToString(),
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(22, 163, 74),
                Location = new Point(10, 12),
                Size = new Size(32, 32),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label nameLabel = new Label
            {
                Text = emp.Name,
                Font = new Font("Arial", 12, FontStyle.Bold),
                Location = new Point(52, 10),
                Size = new Size(180, 20),
                BackColor = Color.Transparent
            };

            Label ordersLabel = new Label
            {
                Text = $"{emp.OrdersProcessed} orders",
                Font = new Font("Arial", 10),
                ForeColor = Color.FromArgb(107, 114, 128),
                Location = new Point(52, 32),
                Size = new Size(180, 18),
                BackColor = Color.Transparent
            };

            Label salesLabel = new Label
            {
                Text = $"${emp.TotalSales:F2}",
                Font = new Font("Arial", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 163, 74),
                Location = new Point(250, 17),
                Size = new Size(110, 22),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.Transparent
            };

            row.Controls.AddRange(new Control[] { rankLabel, nameLabel, ordersLabel, salesLabel });

            return row;
        }

        /// <summary>
        /// Creates a section panel with title
        /// </summary>
        private Panel CreateSectionPanel(string title, int width, int height)
        {
            Panel panel = new Panel
            {
                Size = new Size(width, height),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label titleLabel = new Label
            {
                Text = title,
                Font = new Font("Arial", 16, FontStyle.Bold),
                Location = new Point(15, 15),
                Size = new Size(width - 30, 30),
                BackColor = Color.Transparent
            };

            // Separator line
            Panel separator = new Panel
            {
                Location = new Point(0, 50),
                Size = new Size(width, 2),
                BackColor = Color.FromArgb(229, 231, 235)
            };

            panel.Controls.AddRange(new Control[] { titleLabel, separator });

            return panel;
        }

        /// <summary>
        /// Gets color for order status
        /// </summary>
        private Color GetStatusColor(string status)
        {
            return status switch
            {
                "new" => Color.FromArgb(37, 99, 235),
                "preparing" => Color.FromArgb(234, 179, 8),
                "ready" => Color.FromArgb(22, 163, 74),
                "completed" => Color.FromArgb(107, 114, 128),
                _ => Color.FromArgb(156, 163, 175)
            };
        }

        /// <summary>
        /// Loads sample data for dashboard
        /// </summary>
        private void LoadSampleData()
        {
            // Load employees
            employees.Add(new EmployeeData(1, "Mike Johnson", "Cashier", true, "9:00 AM", 3, 41.39, "active"));
            employees.Add(new EmployeeData(2, "Sarah Davis", "Cashier", true, "9:15 AM", 3, 49.09, "active"));
            employees.Add(new EmployeeData(3, "Tom Wilson", "Cook", true, "8:45 AM", 0, 0, "active"));
            employees.Add(new EmployeeData(4, "Lisa Anderson", "Delivery Driver", false, null, 0, 0, "not_clocked_in"));
            employees.Add(new EmployeeData(5, "James Brown", "Cook", false, null, 0, 0, "not_clocked_in"));
            employees.Add(new EmployeeData(6, "Emma Martinez", "Cashier", true, "10:00 AM", 0, 0, "on_break"));

            // Load orders
            orders.Add(new OrderData(1001, "John Smith", "555-0123", "2:15 PM", "preparing", "Dine-in", "Mike Johnson", 3, 18.39));
            orders.Add(new OrderData(1002, "Sarah Johnson", "555-0456", "2:18 PM", "new", "Delivery", "Sarah Davis", 2, 19.44));
            orders.Add(new OrderData(1003, "Mike Davis", "555-0789", "2:10 PM", "ready", "Pickup", "Mike Johnson", 2, 18.36));
            orders.Add(new OrderData(1004, "Guest", "N/A", "2:20 PM", "new", "Dine-in", "Sarah Davis", 1, 5.40));
            orders.Add(new OrderData(1000, "Emily White", "555-0111", "1:45 PM", "completed", "Pickup", "Mike Johnson", 1, 8.64));
            orders.Add(new OrderData(999, "Tom Brown", "555-0222", "1:30 PM", "completed", "Dine-in", "Sarah Davis", 2, 14.58));
            orders.Add(new OrderData(998, "Robert Lee", "555-0333", "1:15 PM", "completed", "Pickup", "Sarah Davis", 1, 9.18));
            orders.Add(new OrderData(997, "Nancy Green", "555-0444", "12:45 PM", "completed", "Delivery", "Mike Johnson", 3, 24.93));