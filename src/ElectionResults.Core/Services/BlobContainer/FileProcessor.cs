using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ElectionResults.Core.Infrastructure;
using ElectionResults.Core.Services.CsvProcessing;
using ElectionResults.Core.Storage;
using Microsoft.Extensions.Options;

namespace ElectionResults.Core.Services.BlobContainer
{
    public class FileProcessor : IFileProcessor
    {
        private readonly IResultsRepository _resultsRepository;
        private readonly IStatisticsAggregator _statisticsAggregator;

        public FileProcessor(IResultsRepository resultsRepository,
            IElectionConfigurationSource electionConfigurationSource,
            IStatisticsAggregator statisticsAggregator,
            IOptions<AppConfig> config)
        {
            _resultsRepository = resultsRepository;
            _statisticsAggregator = statisticsAggregator;
            _statisticsAggregator.CsvParsers = new List<ICsvParser>
            {
                new CandidatesResultsParser(config)
            };
        }

        public async Task ProcessStream(Stream csvStream, string fileName)
        {
            var csvContent = await ReadCsvContent(csvStream);
            var aggregationResult = await _statisticsAggregator.RetrieveElectionData(csvContent);
            if (aggregationResult.IsSuccess)
            {
                var electionStatistics = FileNameParser.BuildElectionStatistics(fileName, aggregationResult.Value);
                Console.WriteLine($"Uploading file {fileName} with timestamp {electionStatistics.FileTimestamp}");
                await _resultsRepository.InsertResults(electionStatistics);
            }
        }

        protected virtual async Task<string> ReadCsvContent(Stream csvStream)
        {
            var buffer = new byte[csvStream.Length];
            await csvStream.ReadAsync(buffer, 0, (int)csvStream.Length);
            var csvContent = Encoding.UTF8.GetString(buffer);
            return csvContent;
        }
    }
}
