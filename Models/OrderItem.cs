using System;
using System.Collections.Generic;
using System.Linq;

namespace BlueberryPizzeria.Models
{
   
    public interface IOrderItem
    {
        string Name { get; }
        
     
        string Details { get; }
        
      
        double Price { get; }
    }

    
    public class SimpleOrderItem : IOrderItem
    {
        public string Name { get; set; }
        
      
        public string Details => "";
        
        
        public double Price { get; set; }

        /// <param name="name">Item name</param>
        /// <param name="price">Item price</param>
        public SimpleOrderItem(string name, double price)
        {
            Name = name;
            Price = price;
        }
    }


    public class PizzaOrder : IOrderItem
    {
        private static readonly double[] SIZE_PRICES = { 5.0, 7.0, 9.0, 11.0 }; // Small, Medium, Large, XL
        private static readonly double[] TOPPING_PRICES = { 0.75, 1.0, 1.25, 1.50 }; // Per size

        public string Size { get; private set; }
        
  
        public string Crust { get; private set; }
        
  
        public List<string> Toppings { get; private set; }
        
    
        public string Name => $"{Size} Pizza";
        
     
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
        
   
        public double Price { get; private set; }

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

    
        private void CalculatePrice()
        {
            int sizeIndex = GetSizeIndex();
            double basePrice = SIZE_PRICES[sizeIndex];
            double toppingPrice = TOPPING_PRICES[sizeIndex];
            
            // First 2 toppings (cheese + 1) are free, rest are extra
            int extraToppings = Math.Max(0, Toppings.Count - 2);
            Price = basePrice + (extraToppings * toppingPrice);
        }

      
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