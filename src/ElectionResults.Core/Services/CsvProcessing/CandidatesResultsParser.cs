using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using CsvHelper;
using ElectionResults.Core.Models;
using ElectionResults.Core.Storage;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ElectionResults.Core.Services.CsvProcessing
{
    public class CandidatesResultsParser : ICsvParser
    {
        private readonly IOptions<AppConfig> _config;

        public CandidatesResultsParser(IOptions<AppConfig> config)
        {
            _config = config;
        }
        public async Task<Result<ElectionResultsData>> Parse(ElectionResultsData electionResultsData, string csvContent)
        {
            if (electionResultsData == null)
                electionResultsData = new ElectionResultsData();
            var electionsConfig = DeserializeElectionsConfig();

            electionResultsData.Candidates = electionsConfig.Candidates.Select(c => new CandidateStatistics
            {
                Id = c.CsvId,
                ImageUrl = c.ImageUrl,
                Name = c.Name
            }).ToList();
            await PopulateCandidatesListWithVotes(csvContent, electionResultsData.Candidates);
            var sumOfVotes = electionResultsData.Candidates.Sum(c => c.Votes);
            StatisticsAggregator.CalculatePercentagesForCandidates(electionResultsData, sumOfVotes);

            return Result.Ok(electionResultsData);
        }

        private ElectionsConfig DeserializeElectionsConfig()
        {
            try
            {
                return JsonConvert.DeserializeObject<ElectionsConfig>(_config.Value.ElectionsConfig);
            }
            catch (Exception)
            {
                return ElectionsConfig.Default;
            }
        }

        protected virtual async Task PopulateCandidatesListWithVotes(string csvContent,
            List<CandidateStatistics> candidates)
        {
            var csvParser = new CsvParser(new StringReader(csvContent));
            var headers = (await csvParser.ReadAsync()).ToList();
            do
            {
                var rowValues = await csvParser.ReadAsync();
                if (rowValues == null)
                    break;
                foreach (var candidate in candidates)
                {
                    var votes = int.Parse(rowValues[headers.IndexOf(candidate.Id)]);
                    candidate.Votes += votes;
                }
            } while (true);
        }
    }
}
