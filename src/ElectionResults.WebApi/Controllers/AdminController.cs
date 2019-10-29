using System.Collections.Generic;
using System.Threading.Tasks;
using ElectionResults.Core.Infrastructure;
using ElectionResults.Core.Infrastructure.CsvModels;
using ElectionResults.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElectionResults.WebApi.Controllers
{
    [Route("api/settings")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IElectionConfigurationSource _electionConfigurationSource;

        public AdminController(IElectionConfigurationSource electionConfigurationSource)
        {
            _electionConfigurationSource = electionConfigurationSource;
        }

        [HttpPut("jobTimer")]
        public async Task<ActionResult> SetJobTimer([FromBody] string interval)
        {
            var result = await _electionConfigurationSource.UpdateJobTimer(interval);
            if (result.IsSuccess)
                return Ok();
            return BadRequest(result.Error);
        }

        [HttpPut("election-config")]
        public async Task<ActionResult> UpdateSettings([FromBody] ElectionsConfig config)
        {
            //temporary config until the admin frontend is ready
            var newConfig = new ElectionsConfig();
            newConfig.Candidates = new List<CandidateConfig>
            {
                new CandidateConfig
                {
                    Name = "Candidate A",
                    Id = "1",
                    CsvId = "g1",
                    ImageUrl = "https://code4-presidential-2019.s3.eu-central-1.amazonaws.com/c1.png"
                },
                new CandidateConfig
                {
                    Name = "Candidate B",
                    Id = "2",
                    CsvId = "g2",
                    ImageUrl = "https://code4-presidential-2019.s3.eu-central-1.amazonaws.com/c2.png"
                },
                new CandidateConfig
                {
                    Name = "Candidate C",
                    Id = "3",
                    CsvId = "g3",
                    ImageUrl = "https://code4-presidential-2019.s3.eu-central-1.amazonaws.com/c1.png"
                },
                new CandidateConfig
                {
                    Name = "Candidate D",
                    Id = "4",
                    CsvId = "g4",
                    ImageUrl = "https://code4-presidential-2019.s3.eu-central-1.amazonaws.com/c2.png"
                },
                new CandidateConfig
                {
                    Name = "Candidate E",
                    Id = "5",
                    CsvId = "g5",
                    ImageUrl = "https://code4-presidential-2019.s3.eu-central-1.amazonaws.com/c1.png"
                },
                new CandidateConfig
                {
                    Name = "Candidate F",
                    Id = "6",
                    CsvId = "g6",
                    ImageUrl = "https://code4-presidential-2019.s3.eu-central-1.amazonaws.com/c2.png"
                },
                new CandidateConfig
                {
                    Name = "Candidate G",
                    Id = "7",
                    CsvId = "g7",
                    ImageUrl = "https://code4-presidential-2019.s3.eu-central-1.amazonaws.com/c1.png"
                },
                new CandidateConfig
                {
                    Name = "Candidate H",
                    Id = "8",
                    CsvId = "g8",
                    ImageUrl = "https://code4-presidential-2019.s3.eu-central-1.amazonaws.com/c2.png"
                },
                new CandidateConfig
                {
                    Name = "Candidate I",
                    Id = "9",
                    CsvId = "g9",
                    ImageUrl = "https://code4-presidential-2019.s3.eu-central-1.amazonaws.com/c1.png"
                },
                new CandidateConfig
                {
                    Name = "Candidate J",
                    Id = "10",
                    CsvId = "g10",
                    ImageUrl = "https://code4-presidential-2019.s3.eu-central-1.amazonaws.com/c2.png"
                },
                new CandidateConfig
                {
                    Name = "Candidate K",
                    Id = "11",
                    CsvId = "g11",
                    ImageUrl = "https://code4-presidential-2019.s3.eu-central-1.amazonaws.com/c1.png"
                },
                new CandidateConfig
                {
                    Name = "Candidate L",
                    Id = "12",
                    CsvId = "g12",
                    ImageUrl = "https://code4-presidential-2019.s3.eu-central-1.amazonaws.com/c2.png"
                },
                new CandidateConfig
                {
                    Name = "Candidate M",
                    Id = "13",
                    CsvId = "g13",
                    ImageUrl = "https://code4-presidential-2019.s3.eu-central-1.amazonaws.com/c1.png"
                },
                new CandidateConfig
                {
                    Name = "Candidate N",
                    Id = "14",
                    CsvId = "g14",
                    ImageUrl = "https://code4-presidential-2019.s3.eu-central-1.amazonaws.com/c2.png"
                },
                new CandidateConfig
                {
                    Name = "Candidate O",
                    Id = "15",
                    CsvId = "g15",
                    ImageUrl = "https://code4-presidential-2019.s3.eu-central-1.amazonaws.com/c1.png"
                },
                new CandidateConfig
                {
                    Name = "Candidate P",
                    Id = "16",
                    CsvId = "g16",
                    ImageUrl = "https://code4-presidential-2019.s3.eu-central-1.amazonaws.com/c2.png"
                }
            };
            newConfig.Files = new List<ElectionResultsFile>
            {
                new ElectionResultsFile
                {
                    Active = true,
                    ResultsType = ResultsType.Partial,
                    ResultsLocation = ResultsLocation.Romania,
                    URL = "https://prezenta.bec.ro/europarlamentare26052019/data/pv/csv/pv_RO_EUP_PART.csv"
                },
                new ElectionResultsFile
                {
                    Active = true,
                    ResultsType = ResultsType.Partial,
                    ResultsLocation = ResultsLocation.Diaspora,
                    URL = "https://prezenta.bec.ro/europarlamentare26052019/data/pv/csv/pv_SR_EUP_PART.csv"
                },
                new ElectionResultsFile
                {
                    Active = true,
                    ResultsType = ResultsType.Final,
                    ResultsLocation = ResultsLocation.Romania,
                    URL = "https://prezenta.bec.ro/europarlamentare26052019/data/pv/csv/pv_RO_EUP_FINAL.csv"
                },
                new ElectionResultsFile
                {
                    Active = true,
                    ResultsType = ResultsType.Final,
                    ResultsLocation = ResultsLocation.Diaspora,
                    URL = "https://prezenta.bec.ro/europarlamentare26052019/data/pv/csv/pv_SR_EUP_FINAL.csv"
                },
                new ElectionResultsFile
                {
                    Active = true,
                    ResultsType = ResultsType.Provisional,
                    ResultsLocation = ResultsLocation.Romania,
                    URL = "https://prezenta.bec.ro/europarlamentare26052019/data/pv/csv/pv_RO_EUP_PROV.csv"
                },
                new ElectionResultsFile
                {
                    Active = true,
                    ResultsType = ResultsType.Provisional,
                    ResultsLocation = ResultsLocation.Diaspora,
                    URL = "https://prezenta.bec.ro/europarlamentare26052019/data/pv/csv/pv_SR_EUP_PROV.csv"
                }
            };
            await _electionConfigurationSource.UpdateElectionConfig(newConfig);
            return Ok();
        }
    }
}