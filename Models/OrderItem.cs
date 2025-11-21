using System;
using System.Collections.Generic;
using System.Linq;

namespace BlueberryPizzeria.Models
{
    /// <summary>
    /// Interface for all order items in the system
    /// </summary>
    public interface IOrderItem
    {
        /// <summary>
        /// Gets the name of the order item
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Gets detailed description of the item
        /// </summary>
        string Details { get; }
        
        /// <summary>
        /// Gets the price of the item
        /// </summary>
        double Price { get; }
    }

    /// <summary>
    /// Simple order item for sides and drinks
    /// </summary>
    public class SimpleOrderItem : IOrderItem
    {
        /// <summary>
        /// Gets or sets the item name
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Gets the item details (empty for simple items)
        /// </summary>
        public string Details => "";
        
        /// <summary>
        /// Gets or sets the item price
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Initializes a new simple order item
        /// </summary>
        /// <param name="name">Item name</param>
        /// <param name="price">Item price</param>
        public SimpleOrderItem(string name, double price)
        {
            Name = name;
            Price = price;
        }
    }

    /// <summary>
    /// Pizza order with full customization
    /// Handles size, crust, toppings, and automatic pricing
    /// </summary>
    public class PizzaOrder : IOrderItem
    {
        private static readonly double[] SIZE_PRICES = { 5.0, 7.0, 9.0, 11.0 }; // Small, Medium, Large, XL
        private static readonly double[] TOPPING_PRICES = { 0.75, 1.0, 1.25, 1.50 }; // Per size

        /// <summary>
        /// Gets the pizza size
        /// </summary>
        public string Size { get; private set; }
        
        /// <summary>
        /// Gets the crust type
        /// </summary>
        public string Crust { get; private set; }
        
        /// <summary>
        /// Gets the list of toppings
        /// </summary>
        public List<string> Toppings { get; private set; }
        
        /// <summary>
        /// Gets the pizza name (includes size)
        /// </summary>
        public string Name => $"{Size} Pizza";
        
        /// <summary>
        /// Gets detailed description with crust and toppings
        /// </summary>
        public string Details
        {
            get
            {
                var details = $"{Crust} Crust";
                if (Toppings.Count > 0)
                {
                    details += $" - {string.Join(", ", Toppings)}";
                }
                return details;
            }
        }
        
        /// <summary>
        /// Gets the calculated price
        /// </summary>
        public double Price { get; private set; }

        /// <summary>
        /// Initializes a new pizza order
        /// </summary>
        /// <param name="size">Pizza size</param>
        /// <param name="crust">Crust type</param>
        /// <param name="toppings">List of toppings</param>
        public PizzaOrder(string size, string crust, List<string> toppings)
        {
            Size = size;
            Crust = crust;
            Toppings = new List<string>(toppings);
            CalculatePrice();
        }

        /// <summary>
        /// Calculates the total price based on size and toppings
        /// Base price includes cheese + 1 topping free, extras charged
        /// </summary>
        private void CalculatePrice()
        {
            int sizeIndex = GetSizeIndex();
            double basePrice = SIZE_PRICES[sizeIndex];
            double toppingPrice = TOPPING_PRICES[sizeIndex];
            
            // First 2 toppings (cheese + 1) are free, rest are extra
            int extraToppings = Math.Max(0, Toppings.Count - 2);
            Price = basePrice + (extraToppings * toppingPrice);
        }

        /// <summary>
        /// Gets the index of the size for pricing lookup
        /// </summary>
        /// <returns>Size index (0-3)</returns>
        private int GetSizeIndex()
        {
            switch (Size)
            {
                case "Small":
                    return 0;
                case "Medium":
                    return 1;
                case "Large":
                    return 2;
                case "Extra Large":
                    return 3;
                default:
                    return 1; // Default to Medium
            }
        }

    }
}