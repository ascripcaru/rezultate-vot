namespace ElectionResults.Core.Models
{
    public class ElectionResultsFile
    {
        public string URL { get; set; }

        public bool Active { get; set; }

        public string Name { get; set; }

        public ResultsType ResultsType { get; set; }

        public ResultsLocation ResultsLocation { get; set; }
    }
}