using System;
using System.Linq;
using System.Threading.Tasks;
using ElectionResults.Core.Infrastructure;
using ElectionResults.Core.Models;
using ElectionResults.Core.Services.BlobContainer;
using ElectionResults.Core.Services.CsvDownload;
using NSubstitute;
using Xunit;

namespace ElectionResults.Tests.CsvDownloaderJobTests
{
    public class DownloadFilesToBlobStorageShould
    {
        private IBucketUploader _bucketUploader;
        private IElectionConfigurationSource _electionConfigurationSource;

        [Fact]
        public async Task RetrieveListOfCsvFiles()
        {
            var csvDownloaderJob = CreatecsvDownloaderJob();
            CreateResultsSourceMock(new ElectionResultsFile());

            await csvDownloaderJob.DownloadFilesToBlobStorage();

            _electionConfigurationSource.Received(1).GetListOfFilesWithElectionResults();
        }

        [Fact]
        public async Task UploadFilesToBlobContainer()
        {
            var csvDownloaderJob = CreatecsvDownloaderJob();
            CreateResultsSourceMock( new ElectionResultsFile() );

            await csvDownloaderJob.DownloadFilesToBlobStorage();

            await _bucketUploader
                .Received(1)
                .UploadFromUrl(Arg.Any<ElectionResultsFile>());
        }

        [Fact]
        public async Task UploadSameNumberOfFilesThatItReceived()
        {
            var csvDownloaderJob = CreatecsvDownloaderJob();
            CreateResultsSourceMock( new ElectionResultsFile(), new ElectionResultsFile() );

            await csvDownloaderJob.DownloadFilesToBlobStorage();

            await _bucketUploader
                .Received(2)
                .UploadFromUrl(Arg.Any<ElectionResultsFile>());
        }

        [Fact]
        public async Task UseSameTimestampForEachFile()
        {
            var csvDownloaderJob = CreatecsvDownloaderJob();
            CreateResultsSourceMock( new ElectionResultsFile(), new ElectionResultsFile() );
            SystemTime.Now = DateTimeOffset.UtcNow;
            var timestamp = SystemTime.Now.ToUnixTimeSeconds();

            await csvDownloaderJob.DownloadFilesToBlobStorage();

            await _bucketUploader
                .Received(2)
                .UploadFromUrl(Arg.Is<ElectionResultsFile>(f => f.Name.Contains(timestamp.ToString())));
        }

        [Fact]
        public async Task BuildNameOfUploadedFiles()
        {
            var csvDownloaderJob = CreatecsvDownloaderJob();
            CreateResultsSourceMock(new ElectionResultsFile { ResultsType = ResultsType.Final, ResultsLocation = ResultsLocation.Romania});
            SystemTime.Now = DateTimeOffset.UtcNow;
            var timestamp = SystemTime.Now.ToUnixTimeSeconds();

            await csvDownloaderJob.DownloadFilesToBlobStorage();

            await _bucketUploader
                .Received(1)
                .UploadFromUrl(Arg.Is<ElectionResultsFile>(f => f.Name == $"FINAL_RO_{timestamp}.csv"));
        }

        private CsvDownloaderJob CreatecsvDownloaderJob()
        {
            _bucketUploader = Substitute.For<IBucketUploader>();
            _electionConfigurationSource = Substitute.For<IElectionConfigurationSource>();
            return new CsvDownloaderJob(_bucketUploader, _electionConfigurationSource);
        }

        private void CreateResultsSourceMock(params ElectionResultsFile[] files)
        {
            _electionConfigurationSource.GetListOfFilesWithElectionResults()
                .ReturnsForAnyArgs(info => files.ToList());
        }
    }
}
