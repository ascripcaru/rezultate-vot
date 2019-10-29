using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ElectionResults.Core.Models;
using ElectionResults.Core.Services.CsvProcessing;

namespace ElectionResults.Tests.StatisticsAggregatorTests.Fakes
{
    public class FakeCandidatesParser : ICsvParser
    {
        public Task<Result<ElectionResultsData>> Parse(ElectionResultsData electionResultsData, string csvContent)
        {
            WasInvoked = true;
            electionResultsData.Candidates = new List<CandidateStatistics>();
            return Task.FromResult(Result.Ok(electionResultsData));
        }

        public bool WasInvoked { get; set; }
    }
}