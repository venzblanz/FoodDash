namespace Blanza_Midterm.Models
{
    public class CartItem
    {
        public MenuItem Item { get; set; } = new();
        public int Quantity { get; set; } = 1;
        
        // Automatically calculates the total price for this specific item
        public decimal TotalPrice => Item.Price * Quantity; 
    }
}