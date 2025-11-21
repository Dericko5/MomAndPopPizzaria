using System.Collections.Generic;

namespace BlueberryPizzeria.Models
{
    /// <summary>
    /// Simple data model for orders displayed on the kitchen screen.
    /// </summary>
    public class KitchenOrder
    {
        public int OrderNumber { get; set; }
        public string Customer { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }   // Dine-in / Pickup / Delivery
        public string Status { get; set; } // "new", "preparing", "ready", "completed"

        /// <summary>
        /// Each string is one item line to show on the card.
        /// </summary>
        public List<string> Items { get; set; }

        public KitchenOrder(
            int orderNumber,
            string customer,
            string time,
            string type,
            string status,
            List<string> items)
        {
            OrderNumber = orderNumber;
            Customer = customer;
            Time = time;
            Type = type;
            Status = status;
            Items = items ?? new List<string>();
        }
    }
}
