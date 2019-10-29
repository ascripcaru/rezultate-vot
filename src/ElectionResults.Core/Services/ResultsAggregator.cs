using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ElectionResults.Core.Models;
using ElectionResults.Core.Services.CsvProcessing;
using ElectionResults.Core.Storage;
using Newtonsoft.Json;

namespace ElectionResults.Core.Services
{
    public class ResultsAggregator : IResultsAggregator
    {
        private readonly IResultsRepository _resultsRepository;

        public ResultsAggregator(IResultsRepository resultsRepository)
        {
            _resultsRepository = resultsRepository;
        }

        public async Task<ElectionResultsData> GetResults(ResultsType type)
        {
            string resultsType = type.ConvertEnumToString();

            var localResults = await _resultsRepository.GetLatestResults(Consts.LOCAL, resultsType);
            var diasporaResults = await _resultsRepository.GetLatestResults(Consts.DIASPORA, resultsType);
            var localResultsData = JsonConvert.DeserializeObject<ElectionResultsData>(localResults.StatisticsJson);
            var diasporaResultsData = JsonConvert.DeserializeObject<ElectionResultsData>(diasporaResults.StatisticsJson);
            var electionResultsData = StatisticsAggregator.CombineResults(localResultsData, diasporaResultsData);
            return electionResultsData;
        }
    }
}
