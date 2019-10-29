using System;
using System.IO;
using ElectionResults.Core.Models;
using Newtonsoft.Json;

namespace ElectionResults.Core.Services
{
    public class FileNameParser
    {
        public static ElectionStatistics BuildElectionStatistics(string fileName, ElectionResultsData electionResultsData)
        {
            var electionStatistics = new ElectionStatistics();
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            electionStatistics.Id = $"{DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks:D19}";
            var attributes = fileNameWithoutExtension.Split('_');
            electionStatistics.Type = attributes[0];
            electionStatistics.Location = attributes[1];
            electionStatistics.FileTimestamp = long.Parse(attributes[2]);
            electionStatistics.StatisticsJson = JsonConvert.SerializeObject(electionResultsData);
            return electionStatistics;
        }
    }
}
