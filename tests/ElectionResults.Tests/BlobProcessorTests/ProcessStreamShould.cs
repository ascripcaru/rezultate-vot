using System.IO;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ElectionResults.Core.Infrastructure;
using ElectionResults.Core.Models;
using ElectionResults.Core.Services.CsvProcessing;
using ElectionResults.Core.Storage;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace ElectionResults.Tests.BlobProcessorTests
{
    public class ProcessStreamShould
    {
        private readonly IStatisticsAggregator _statisticsAggregator;
        private readonly IElectionConfigurationSource _electionConfigurationSource;
        private readonly IResultsRepository _resultsRepository;
        private readonly string _fileName;

        public ProcessStreamShould()
        {
            _statisticsAggregator = Substitute.For<IStatisticsAggregator>();
            _electionConfigurationSource = Substitute.For<IElectionConfigurationSource>();
            _resultsRepository = Substitute.For<IResultsRepository>();
            _fileName = "a_b_1";
        }

        [Fact]
        public async Task convert_stream_to_string()
        {
            var blobProcessor = CreateTestableBlobProcessor();
            MapStatisticsAggregatorToSuccessfulResult();

            await blobProcessor.ProcessStream(new MemoryStream(), _fileName);

            blobProcessor.CsvWasReadAsString.Should().BeTrue();
        }

        [Fact]
        public async Task apply_data_aggregators_to_the_csv_content()
        {
            var blobProcessor = CreateTestableBlobProcessor();
            MapStatisticsAggregatorToSuccessfulResult();

            await blobProcessor.ProcessStream(new MemoryStream(), _fileName);

            await _statisticsAggregator.ReceivedWithAnyArgs(1).RetrieveElectionData("");
        }

        [Fact]
        public async Task apply_at_least_one_aggregator()
        {
            var blobProcessor = CreateTestableBlobProcessor();
            MapStatisticsAggregatorToSuccessfulResult();

            await blobProcessor.ProcessStream(new MemoryStream(), _fileName);

            _statisticsAggregator.CsvParsers.Should().NotBeEmpty();
        }

        [Fact]
        public async Task save_json_in_database()
        {
            var blobProcessor = CreateTestableBlobProcessor();
            MapStatisticsAggregatorToSuccessfulResult();

            await blobProcessor.ProcessStream(new MemoryStream(), _fileName);

            await _resultsRepository.ReceivedWithAnyArgs(1).InsertResults(null);
        }

        private TestableFileProcessor CreateTestableBlobProcessor()
        {
            return new TestableFileProcessor(_resultsRepository, _electionConfigurationSource, _statisticsAggregator, null);
        }

        private void MapStatisticsAggregatorToSuccessfulResult()
        {
            _statisticsAggregator.RetrieveElectionData("")
                .ReturnsForAnyArgs(Task.FromResult(Result.Ok(new ElectionResultsData())));
        }
    }
}
