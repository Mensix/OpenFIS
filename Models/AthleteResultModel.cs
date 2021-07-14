using System.Text.Json.Serialization;
using OpenFIS.Models.Competition;
using OpenFIS.Models.Competition.Competitor;

namespace OpenFIS.Models
{
    public class AthleteResultModel
    {
        [JsonIgnore]
        public int Id { get; set; }
        public CompetitionType CompetitionType { get; set; }
        public CompetitionPlace CompetitionPlace { get; set; }
        public string CompetitionDate { get; set; }
        public CompetitorResult AthleteResult { get; set; }
    }
}