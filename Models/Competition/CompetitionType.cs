using System.Text.Json.Serialization;

namespace OpenFIS.Models.Competition
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CompetitionType
    {
        AlpenCup,
        Children,
        ContinentalCup,
        GrandPrix,
        FisCup,
        OlympicWinterGames,
        SkiFlyingWorldChampionships,
        TeamCup,
        WorldCup,
        Qualification,
        NotAvailable,
        NotFound
    }
}