using Blanza_Midterm.Models;

namespace Blanza_Midterm.Services
{
    // Registered as Scoped: one instance per user session (Blazor Server circuit),
    // so every page sees the same cart.
    public class CartService
    {
        public const int MaxQuantityPerItem = 20;

        private static readonly string[] Riders = { "Mark", "Jessa", "Paolo", "Liza", "Carlo" };
        private readonly List<CartItem> _items = new();

        // Raised after any change so shared UI (e.g. the navbar badge) can refresh.
        public event Action? Changed;

        public IReadOnlyList<CartItem> Items => _items;
        public int Count => _items.Sum(i => i.Quantity);
        public decimal Total => _items.Sum(i => i.TotalPrice);
        public OrderTrack? LastOrder { get; private set; }

        public void Add(MenuItem item)
        {
            var existing = _items.FirstOrDefault(i => i.Item.Id == item.Id);
            if (existing is null)
                _items.Add(new CartItem { Item = item, Quantity = 1 });
            else if (existing.Quantity < MaxQuantityPerItem)
                existing.Quantity++;

            Changed?.Invoke();
        }

        public void Increase(int itemId)
        {
            var existing = _items.FirstOrDefault(i => i.Item.Id == itemId);
            if (existing is not null && existing.Quantity < MaxQuantityPerItem)
            {
                existing.Quantity++;
                Changed?.Invoke();
            }
        }

        public void Decrease(int itemId)
        {
            var existing = _items.FirstOrDefault(i => i.Item.Id == itemId);
            if (existing is null) return;

            existing.Quantity--;
            if (existing.Quantity <= 0) _items.Remove(existing);
            Changed?.Invoke();
        }

        public void Remove(int itemId)
        {
            _items.RemoveAll(i => i.Item.Id == itemId);
            Changed?.Invoke();
        }

        public void Clear()
        {
            _items.Clear();
            Changed?.Invoke();
        }

        // Turns the current cart into an order for the tracking page, then empties the cart.
        public OrderTrack PlaceOrder(DeliveryDetails details)
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("Cannot place an order with an empty cart.");

            LastOrder = new OrderTrack
            {
                OrderId = "FD-" + Random.Shared.Next(100000, 999999),
                EstimatedMinutes = 30,
                RiderName = Riders[Random.Shared.Next(Riders.Length)],
                OrderTime = DateTime.Now,
                Delivery = new DeliveryDetails
                {
                    FullName = details.FullName.Trim(),
                    Phone = details.Phone.Trim(),
                    Address = details.Address.Trim(),
                    PaymentMethod = details.PaymentMethod
                },
                Items = _items.Select(i => new CartItem { Item = i.Item, Quantity = i.Quantity }).ToList(),
                Total = Total
            };

            _items.Clear();
            Changed?.Invoke();
            return LastOrder;
        }
    }
}
