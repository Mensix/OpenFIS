using System.Collections.Generic;
using System.Text.Json.Serialization;
using OpenFIS.Models.Competition.Competitor;

namespace OpenFIS.Models.Competition
{
    public class CompetitionResult
    {
        [JsonIgnore]
        public int Id { get; set; }
        public CompetitionType CompetitionType { get; set; }
        public CompetitionPlace CompetitionPlace { get; set; }
        public string CompetitionDate { get; set; }
        public List<CompetitorResult> CompetitorsResult { get; set; }
    }
}