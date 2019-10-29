namespace ElectionResults.Core.Models
{
    public class CandidateStatistics
    {
        public string Id { get; set; }

        public int Votes { get; set; }

        public string Name { get; set; }

        public decimal Percentage { get; set; }

        public string ImageUrl { get; set; }
    }
}