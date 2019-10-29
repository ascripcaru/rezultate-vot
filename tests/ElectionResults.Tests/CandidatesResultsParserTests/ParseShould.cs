using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectionResults.Core.Models;
using ElectionResults.Core.Services.CsvProcessing;
using ElectionResults.Core.Storage;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace ElectionResults.Tests.CandidatesResultsParserTests
{
    public class ParseShould
    {
        [Fact]
        public async Task not_return_null_list_of_candidates()
        {
            var candidatesResultsParser = new TestableCandidatesResultsParser(null);
            candidatesResultsParser.ParsedCandidates = new List<CandidateStatistics>();

            var result = await candidatesResultsParser.Parse(null, "");

            result.Value.Candidates.Should().NotBeNull();
        }

        [Theory]
        [InlineData(20, 20, 60, 60, 20, 20)]
        [InlineData(30, 33.33, 30, 33.33, 30, 33.33)]
        [InlineData(1, 1, 37, 37, 62, 62)]
        public void set_percentages_for_each_candidate(int c1Votes, decimal c1Percentage,
            int c2Votes, decimal c2Percentage,
            int c3Votes, decimal c3Percentage)
        {
            var candidatesResultsParser = new TestableCandidatesResultsParser(null)
            {
                ParsedCandidates = CreateListOfCandidatesWithVotes(c1Votes, c2Votes, c3Votes)
            };
            var electionResultsData = new ElectionResultsData {Candidates = CreateListOfCandidatesWithVotes(c1Votes, c2Votes, c3Votes) };
            var sumOfVotes = candidatesResultsParser.ParsedCandidates.Sum(c => c.Votes);
            StatisticsAggregator.CalculatePercentagesForCandidates(electionResultsData, sumOfVotes);

            electionResultsData.Candidates[0].Percentage.Should().Be(c1Percentage);
            electionResultsData.Candidates[1].Percentage.Should().Be(c2Percentage);
            electionResultsData.Candidates[2].Percentage.Should().Be(c3Percentage);
        }

        private static List<CandidateStatistics> CreateListOfCandidatesWithVotes(int c1Votes, int c2Votes, int c3Votes)
        {
            return new List<CandidateStatistics>
            {
                new CandidateStatistics
                {
                    Name = "Candidate1",
                    Votes = c1Votes
                },
                new CandidateStatistics
                {
                    Name = "Candidate2",
                    Votes = c2Votes
                },
                new CandidateStatistics
                {
                    Name = "Candidate3",
                    Votes = c3Votes
                }
            };
        }
    }
}
