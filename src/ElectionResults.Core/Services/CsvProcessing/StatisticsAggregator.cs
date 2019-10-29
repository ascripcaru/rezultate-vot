using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ElectionResults.Core.Models;

namespace ElectionResults.Core.Services.CsvProcessing
{
    public class StatisticsAggregator : IStatisticsAggregator
    {
        public List<ICsvParser> CsvParsers { get; set;  } = new List<ICsvParser>();

        public async Task<Result<ElectionResultsData>> RetrieveElectionData(string csvContent)
        {
            var electionResults = new ElectionResultsData();
            foreach (var csvParser in CsvParsers)
            {
                await csvParser.Parse(electionResults, csvContent);
            }

            return Result.Ok(electionResults);
        }


        public static void CalculatePercentagesForCandidates(ElectionResultsData electionResultsData, int sumOfVotes)
        {
            foreach (var candidate in electionResultsData.Candidates)
            {
                decimal percentage = Math.Round((decimal)candidate.Votes / sumOfVotes * 100, 2);
                candidate.Percentage = percentage;
            }
            electionResultsData.Candidates = electionResultsData.Candidates.OrderByDescending(c => c.Percentage).ToList();
        }


        public static ElectionResultsData CombineResults(ElectionResultsData localResults, ElectionResultsData diasporaResults)
        {
            for (int i = 0; i < localResults.Candidates.Count; i++)
            {
                localResults.Candidates[i].Votes += diasporaResults.Candidates[i].Votes;
            }
            CalculatePercentagesForCandidates(localResults, localResults.Candidates.Sum(c => c.Votes));

            return localResults;
        }

    }
}