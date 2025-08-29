using System;
using System.ComponentModel.DataAnnotations;

namespace AuctionLotManager.Models
{
    public class Lot
    {
        public int LotID { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Start Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Start Price must be greater than zero")]
        public decimal StartPrice { get; set; }

        [Range(0.00, double.MaxValue, ErrorMessage = "Current Bid must be zero or more")]
        public decimal CurrentBid { get; set; }

        [Required(ErrorMessage = "Start Time is required")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End Time is required")]
        [DateGreaterThan("StartTime", ErrorMessage = "End Time must be after Start Time")]
        public DateTime EndTime { get; set; }
    }

    // Custom validation attribute
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _otherPropertyName;

        public DateGreaterThanAttribute(string otherPropertyName)
        {
            _otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherProperty = validationContext.ObjectType.GetProperty(_otherPropertyName);
            if (otherProperty == null)
                return new ValidationResult($"Unknown property: {_otherPropertyName}");

            var otherDate = (DateTime)otherProperty.GetValue(validationContext.ObjectInstance);
            var thisDate = (DateTime)value;

            if (thisDate <= otherDate)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}