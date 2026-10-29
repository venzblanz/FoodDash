using System.ComponentModel.DataAnnotations;

namespace Blanza_Midterm.Models
{
    // Form model for the checkout page; the attributes drive the validation messages.
    public class DeliveryDetails
    {
        public static readonly string[] PaymentMethods = { "Cash on Delivery", "GCash / Maya" };

        [Required(ErrorMessage = "Please enter your full name.")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "Name must be 2 to 60 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your phone number.")]
        [RegularExpression(@"^(\+63|0)\s?9\d{2}\s?\d{3}\s?\d{4}$",
            ErrorMessage = "Enter a valid PH mobile number, e.g. 0917 123 4567.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter your delivery address.")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Please enter a complete address (at least 10 characters).")]
        public string Address { get; set; } = string.Empty;

        public string PaymentMethod { get; set; } = PaymentMethods[0];
    }
}
