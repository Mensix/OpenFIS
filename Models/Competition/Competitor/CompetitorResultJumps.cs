using System;
using System.Text.Json.Serialization;

namespace OpenFIS.Models.Competition.Competitor
{
    public class CompetitorResultJumps
    {
        [JsonIgnore]
        public int Id { get; set; }
        public float? Length { get; set; }
        public float? Point { get; set; }
    }
}