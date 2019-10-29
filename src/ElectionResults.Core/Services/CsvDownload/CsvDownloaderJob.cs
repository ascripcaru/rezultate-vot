using System.Linq;
using System.Threading.Tasks;
using ElectionResults.Core.Infrastructure;
using ElectionResults.Core.Models;
using ElectionResults.Core.Services.BlobContainer;

namespace ElectionResults.Core.Services.CsvDownload
{
    public class CsvDownloaderJob: ICsvDownloaderJob
    {
        private readonly IBucketUploader _bucketUploader;
        private readonly IElectionConfigurationSource _electionConfigurationSource;

        public CsvDownloaderJob(IBucketUploader bucketUploader, IElectionConfigurationSource electionConfigurationSource)
        {
            _bucketUploader = bucketUploader;
            _electionConfigurationSource = electionConfigurationSource;
        }

        public async Task DownloadFilesToBlobStorage()
        {
            var files = _electionConfigurationSource.GetListOfFilesWithElectionResults();
            var timestamp = SystemTime.Now.ToUnixTimeSeconds();
            foreach (var file in files.Where(f => f.Active))
            {
                file.Name = $"{file.ResultsType.ConvertEnumToString()}_{file.ResultsLocation.ConvertEnumToString()}_{timestamp}.csv";
                await _bucketUploader.UploadFromUrl(file);
            }
        }
    }
}
