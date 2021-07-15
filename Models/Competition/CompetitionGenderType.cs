using System.Text.Json.Serialization;

namespace OpenFIS.Models.Competition
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CompetitionGenderType
    {
        Men,
        Women
    }
}