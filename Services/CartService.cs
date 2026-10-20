using Blanza_Midterm.Models;

namespace Blanza_Midterm.Services
{
    // Registered as Scoped: one instance per user session (Blazor Server circuit),
    // so every page sees the same cart.
    public class CartService
    {
        private readonly List<CartItem> _items = new();
        private static readonly string[] Riders = { "Mark", "Jessa", "Paolo", "Liza", "Carlo" };

        public IReadOnlyList<CartItem> Items => _items;
        public int Count => _items.Sum(i => i.Quantity);
        public decimal Total => _items.Sum(i => i.TotalPrice);
        public OrderTrack? LastOrder { get; private set; }

        public void Add(MenuItem item)
        {
            var existing = _items.FirstOrDefault(i => i.Item.Id == item.Id);
            if (existing is null)
                _items.Add(new CartItem { Item = item, Quantity = 1 });
            else
                existing.Quantity++;
        }

        public void Increase(int itemId)
        {
            var existing = _items.FirstOrDefault(i => i.Item.Id == itemId);
            if (existing is not null) existing.Quantity++;
        }

        public void Decrease(int itemId)
        {
            var existing = _items.FirstOrDefault(i => i.Item.Id == itemId);
            if (existing is null) return;

            existing.Quantity--;
            if (existing.Quantity <= 0) _items.Remove(existing);
        }

        public void Remove(int itemId) => _items.RemoveAll(i => i.Item.Id == itemId);

        public void Clear() => _items.Clear();

        // Creates the order record for the tracking page, then empties the cart.
        public OrderTrack PlaceOrder()
        {
            LastOrder = new OrderTrack
            {
                OrderId = "FD-" + Random.Shared.Next(100000, 999999),
                Status = "Preparing",
                EstimatedMinutes = 30,
                RiderName = Riders[Random.Shared.Next(Riders.Length)],
                OrderTime = DateTime.Now
            };
            _items.Clear();
            return LastOrder;
        }
    }
}
