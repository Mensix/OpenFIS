using System.Text.Json.Serialization;

namespace OpenFIS.Models.Competition
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CompetitionType
    {
        AlpenCup,
        Children,
        ContinentalCup,
        EuropeanYouthOlympicFestival,
        GrandPrix,
        Fis,
        FisCup,
        Junior,
        JuniorWorldSkiChampionships,
        OlympicWinterGames,
        SkiFlyingWorldChampionships,
        TeamCup,
        WorldCup,
        WorldSkiChampionships,
        Qualification,
        Universiade,
        YouthOlympicWinterGames,
        NotAvailable,
        NotFound
    }
}