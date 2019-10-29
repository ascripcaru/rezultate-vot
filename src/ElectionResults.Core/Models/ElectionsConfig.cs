using System.Collections.Generic;
using ElectionResults.Core.Infrastructure.CsvModels;

namespace ElectionResults.Core.Models
{
    public class ElectionsConfig
    {
        public List<ElectionResultsFile> Files { get; set; }

        public List<CandidateConfig> Candidates { get; set; }

        public static ElectionsConfig Default => new ElectionsConfig
        {
            Candidates = new List<CandidateConfig>(),
            Files = new List<ElectionResultsFile>()
        };
    }
}