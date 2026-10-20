using System;

namespace Blanza_Midterm.Models
{
    public class OrderTrack
    {
        public string OrderId { get; set; } = string.Empty;
        public string Status { get; set; } = "Preparing";
        public int EstimatedMinutes { get; set; }
        public string RiderName { get; set; } = string.Empty;
        public DateTime OrderTime { get; set; } = DateTime.Now;
    }
}