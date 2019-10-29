using System.Collections.Generic;

namespace ElectionResults.Core.Models
{
    public class ElectionResultsData
    {
        public List<CandidateStatistics> Candidates { get; set; }

        public List<PollingStation> PollingStations { get; set; }
    }
}
