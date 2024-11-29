using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShortTermStayAPI.Models {
    public class Booking {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ListingId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public required string Username { get; set; }
        public string[]? GuestNames { get; set; }

    }

    public class BookingRequest {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int ListingId { get; set; }
        public required string[] GuestNames { get; set; }
    }

    public class BookingReview {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public double Rating { get; set; }
        public required string Comment { get; set; }

    }
}