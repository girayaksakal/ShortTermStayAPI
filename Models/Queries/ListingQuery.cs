namespace ShortTermStayAPI.Models.Queries
{
    public class ListingQuery
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int NoOfPeople { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
    }
}