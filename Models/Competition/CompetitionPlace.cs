using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OpenFIS.Models.Competition
{
    public class CompetitionPlace
    {
        [JsonIgnore]
        public int Id { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        public int? ConstructionPoint { get; set; }
        public int? HillSize { get; set; }
    }
}