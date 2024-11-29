using System.ComponentModel.DataAnnotations.Schema;

namespace ShortTermStayAPI.Models
{
    public class Listing {
        public int Id { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int NoOfPeople { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public double Rating { get; set; }
    }

    public class ListingV2 {
        public int Id { get; set; }

        public required string Title { get; set; }
        public required string Description { get; set; }
        public required int NoOfPeople { get; set; }
        public required string Country { get; set; }
        public required string City { get; set; }
        public required string Address { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        public double Rating { get; set; }
    }
}