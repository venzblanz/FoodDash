namespace Blanza_Midterm.Models
{
    public class OrderTrack
    {
        // Demo timing: the order moves to the next stage every few seconds.
        public const int SecondsPerStage = 10;

        public static readonly string[] Stages =
        {
            "Order Confirmed", "Preparing Food", "Out for Delivery", "Delivered"
        };

        public string OrderId { get; set; } = string.Empty;
        public int EstimatedMinutes { get; set; }
        public string RiderName { get; set; } = string.Empty;
        public DateTime OrderTime { get; set; } = DateTime.Now;

        public DeliveryDetails Delivery { get; set; } = new();
        public List<CartItem> Items { get; set; } = new();
        public decimal Total { get; set; }

        public int Stage =>
            Math.Clamp((int)((DateTime.Now - OrderTime).TotalSeconds / SecondsPerStage), 0, Stages.Length - 1);

        public string Status => Stages[Stage];
        public bool IsDelivered => Stage == Stages.Length - 1;

        public int MinutesLeft =>
            EstimatedMinutes * (Stages.Length - 1 - Stage) / (Stages.Length - 1);
    }
}
