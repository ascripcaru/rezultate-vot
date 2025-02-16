﻿using System.Collections.Generic;
using ElectionResults.Core.Infrastructure.CsvModels;

namespace ElectionResults.Core.Models
{
    public class ElectionStatistics
    {
        public string Id { get; set; }

        public string StatisticsJson { get; set; }

        public string Location { get; set; }

        public long Timestamp { get; set; }

        public string Type { get; set; }

        public static ElectionStatistics Default { get; } = new ElectionStatistics();
    }
}
