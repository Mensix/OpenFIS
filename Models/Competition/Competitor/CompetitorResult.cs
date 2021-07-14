using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace OpenFIS.Models.Competition.Competitor
{
    public class CompetitorResult
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int Rank { get; set; }
        public int Bib { get; set; }
        public Athlete Athlete { get; set; }
        public List<CompetitorResultJumps> Jumps { get; set; }
        public float? TotalPoints { get; set; }
    }
}