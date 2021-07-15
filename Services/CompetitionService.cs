using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;
using OpenFIS.Models;
using OpenFIS.Models.Competition;
using OpenFIS.Models.Competition.Competitor;
using OpenFIS.Repositories;

namespace OpenFIS.Services
{
    public interface ICompetitionService
    {
        Task<CompetitionPlace> GetCompetitionPlace(IPage page);
        Task<CompetitionSubtype> GetCompetitionSubtype(IPage page);
        Task<CompetitionType> GetCompetitionType(IPage page);
        Task<CompetitionGenderType> GetCompetitionGenderType(IPage page);
        Task<int?> GetCompetitionConstructionPoint(IPage page);
        Task<int?> GetCompetitionHillSize(IPage page);
        Task<List<CompetitorResult>> GetCompetitorsResult(IPage page);
        Task<CompetitionResult> GetCompetitionResult(IPage page);
        CompetitionResult GetCompetitionResultById(int id);
        void PostCompetitionResult(CompetitionResult competitionResult);
    }

    public class CompetitionService : ICompetitionService
    {
        private readonly ICompetitionRepository _competitionRepository;
        private readonly IAthleteRepository _athleteRepository;
        public CompetitionService(ICompetitionRepository competitionRepository, IAthleteRepository athleteRepository)
        {
            _competitionRepository = competitionRepository;
            _athleteRepository = athleteRepository;
        }

        public async Task<CompetitionType> GetCompetitionType(IPage page)
        {
            if (await page.QuerySelectorAsync("div.error") != null || await page.QuerySelectorAsync("text='No results found for this competition.'") != null)
            {
                return CompetitionType.NotFound;
            }

            if ((await page.QuerySelectorAsync(".event-header__kind")).InnerTextAsync().Result.Contains("Team"))
            {
                return CompetitionType.TeamCup;
            }

            return await page.QuerySelectorAsync(".event-header__subtitle").Result.InnerTextAsync() switch
            {
                "Alpen Cup" => CompetitionType.AlpenCup,
                "Children" => CompetitionType.Children,
                "Continental Cup" => CompetitionType.ContinentalCup,
                "European Youth Olympic Festival" => CompetitionType.EuropeanYouthOlympicFestival,
                "Grand Prix" => CompetitionType.GrandPrix,
                "FIS" => CompetitionType.Fis,
                "FIS Cup" => CompetitionType.FisCup,
                "Junior" => CompetitionType.Junior,
                "FIS Junior World Ski Championships" => CompetitionType.JuniorWorldSkiChampionships,
                "Olympic Winter Games" => CompetitionType.OlympicWinterGames,
                "FIS Ski-Flying World Championships" => CompetitionType.SkiFlyingWorldChampionships,
                "World Cup" => CompetitionType.WorldCup,
                "World Ski Championships" => CompetitionType.WorldSkiChampionships,
                "Qualification" or "Viessmann FIS Ski Jumping Qualification" => CompetitionType.Qualification,
                "Universiade" => CompetitionType.Universiade,
                "Youth Olympic Winter Games" => CompetitionType.YouthOlympicWinterGames,
                _ => CompetitionType.NotAvailable
            };
        }

        public async Task<CompetitionGenderType> GetCompetitionGenderType(IPage page)
        {
            string[] fromHtml = (await page.QuerySelectorAsync(".event-header__kind").Result.InnerTextAsync())?.Split(' ');
            return fromHtml[0].Contains("Men") ? CompetitionGenderType.Men : CompetitionGenderType.Women;
        }

        public async Task<int?> GetCompetitionConstructionPoint(IPage page)
        {
            string[] fromHtml = (await page.QuerySelectorAsync(".event-header__kind").Result.InnerTextAsync())?.Split(' ');
            if (fromHtml[^1].Contains("K"))
            {
                return Convert.ToInt32(fromHtml[^1].Replace("K", ""));
            }

            return null;
        }

        public async Task<int?> GetCompetitionHillSize(IPage page)
        {
            string[] fromHtml = (await page.QuerySelectorAsync(".event-header__kind").Result.InnerTextAsync())?.Split(' ');
            if (fromHtml[^1].Contains("HS"))
            {
                return Convert.ToInt32(fromHtml[^1].Replace("HS", ""));
            }

            return null;
        }

        public async Task<CompetitionPlace> GetCompetitionPlace(IPage page)
        {
            string[] fromHtml = (await page.QuerySelectorAsync(".event-header__name > h1").Result.InnerTextAsync())?.Split(' ');
            return new CompetitionPlace()
            {
                City = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(string.Join(' ', fromHtml[..^1]).ToLower()),
                Country = fromHtml[^1].Replace("(", "").Replace(")", ""),
                ConstructionPoint = await GetCompetitionConstructionPoint(page),
                HillSize = await GetCompetitionHillSize(page)
            };
        }

        public async Task<CompetitionSubtype> GetCompetitionSubtype(IPage page)
        {
            string[] tableRows = await page.EvaluateAsync<string[]>("[...document.querySelector('#ajx_results .g-row').children].map(x => x.innerText)");
            switch (tableRows.Where(x => x.StartsWith("Distance")).ToArray().Length)
            {
                case 0:
                    if (tableRows.Any(x => x == "Bib"))
                        return CompetitionSubtype.NoLengths;
                    else
                        return CompetitionSubtype.NoBibsAndLenghts;
                default:
                    if (tableRows.Any(x => x == "Bib"))
                        return CompetitionSubtype.WithBibsAndLenghts;
                    break;
            }

            return CompetitionSubtype.WithoutBibsWithLenghts;
        }

        public async Task<List<CompetitorResult>> GetCompetitorsResult(IPage page)
        {
            CompetitionSubtype competitionSubtype = await GetCompetitionSubtype(page);
            List<CompetitorResult> competitorResults = new();

            int roundsCount = (await page.QuerySelectorAllAsync(".table .g-lg-10 > .justify-right.pale")).Count;
            int chunkSize = roundsCount * 2;

            string[][] tableRows = await page.EvaluateAsync<string[][]>("[...document.querySelectorAll('#events-info-results a > .g-row.container > .g-row.justify-sb')].map(x => [...x.children]).map((r, i) => r.map(c => c.innerText.trim()))");
            string[][] lengthRows = (await page.QuerySelectorAllAsync("#events-info-results a .split-row__item")).Select(x => x.InnerTextAsync().Result).ToArray().Select((x, y) => new { Value = x, Index = y }).GroupBy(x => x.Index / chunkSize).Select(x => x.Select(x => x.Value).ToArray()).ToArray();
            int competitorsCount = tableRows.Length;

            for (int i = 0; i < competitorsCount; i++)
            {
                string[] currentTableRow = tableRows[i];
                if (competitionSubtype == CompetitionSubtype.WithBibsAndLenghts)
                {
                    competitorResults.Add(new CompetitorResult
                    {
                        Rank = Convert.ToInt32(currentTableRow[0]),
                        Bib = Convert.ToInt32(currentTableRow[1]),
                        Athlete = _athleteRepository.GetAthleteByFisCode(Convert.ToInt32(currentTableRow[2])) ?? new Athlete()
                        {
                            FisCode = Convert.ToInt32(currentTableRow[2]),
                            Name = currentTableRow[3],
                            Year = int.TryParse(currentTableRow[4], out int a) ? a : null,
                            Nation = currentTableRow[5]
                        },
                        Jumps = new List<CompetitorResultJumps>(),
                        TotalPoints = float.TryParse(currentTableRow[^2], out float b) ? b : null
                    });
                }
                else if (competitionSubtype == CompetitionSubtype.WithoutBibsWithLenghts)
                {
                    competitorResults.Add(new CompetitorResult
                    {
                        Rank = Convert.ToInt32(currentTableRow[0]),
                        Athlete = _athleteRepository.GetAthleteByFisCode(Convert.ToInt32(currentTableRow[1])) ?? new Athlete()
                        {
                            FisCode = Convert.ToInt32(currentTableRow[1]),
                            Name = currentTableRow[2],
                            Year = int.TryParse(currentTableRow[3], out int a) ? a : null,
                            Nation = currentTableRow[4],
                        },
                        Jumps = new List<CompetitorResultJumps>(),
                        TotalPoints = float.TryParse(currentTableRow[^2], out float b) ? b : null
                    });
                }
                else if (competitionSubtype == CompetitionSubtype.NoLengths)
                {
                    if (float.TryParse(currentTableRow[^2], out float f))
                    {
                        competitorResults.Add(new CompetitorResult
                        {
                            Rank = Convert.ToInt32(currentTableRow[0]),
                            Bib = Convert.ToInt32(currentTableRow[1]),
                            Athlete = _athleteRepository.GetAthleteByFisCode(Convert.ToInt32(currentTableRow[2])) ?? new Athlete()
                            {
                                FisCode = Convert.ToInt32(currentTableRow[2]),
                                Name = currentTableRow[3],
                                Year = int.TryParse(currentTableRow[4], out int a) ? a : null,
                                Nation = currentTableRow[5],
                            },
                            TotalPoints = float.TryParse(currentTableRow[^2], out float b) ? b : null
                        });
                    }
                }
                else if (competitionSubtype == CompetitionSubtype.NoBibsAndLenghts)
                {
                    if (!string.IsNullOrEmpty(currentTableRow[0]) && !string.IsNullOrEmpty(currentTableRow[^2]))
                    {
                        competitorResults.Add(new CompetitorResult
                        {
                            Rank = Convert.ToInt32(currentTableRow[0]),
                            Athlete = _athleteRepository.GetAthleteByFisCode(Convert.ToInt32(currentTableRow[1])) ?? new Athlete()
                            {
                                FisCode = Convert.ToInt32(currentTableRow[1]),
                                Name = currentTableRow[2],
                                Year = int.TryParse(currentTableRow[3], out int a) ? a : null,
                                Nation = currentTableRow[4],
                            },
                            TotalPoints = float.TryParse(currentTableRow[^2], out float b) ? b : null
                        });
                    }
                }

                if (competitionSubtype == CompetitionSubtype.WithBibsAndLenghts || competitionSubtype == CompetitionSubtype.WithoutBibsWithLenghts)
                {
                    string[] currentLengthRow = lengthRows[i];
                    for (int j = 0; j <= roundsCount + 2; j += 2)
                    {
                        if (currentLengthRow.ElementAtOrDefault(j) != null && currentLengthRow.ElementAtOrDefault(j + 1) != null)
                        {
                            competitorResults[^1].Jumps.Add(new CompetitorResultJumps
                            {
                                Length = float.TryParse(currentLengthRow[j], out float a) ? a : null,
                                Point = float.TryParse(currentLengthRow[j + 1], out float b) ? b : null
                            });
                        }
                    }
                }
            }

            return competitorResults;
        }

        public async Task<CompetitionResult> GetCompetitionResult(IPage page)
        {
            return new CompetitionResult()
            {
                Id = await page.EvaluateAsync<int>("parseInt(Object.fromEntries(new URLSearchParams(window.location.search).entries()).raceid)"),
                CompetitionType = await GetCompetitionType(page),
                CompetitionPlace = await GetCompetitionPlace(page),
                CompetitionDate = (await page.QuerySelectorAsync("span.date__full")).InnerTextAsync().Result,
                CompetitorsResult = await GetCompetitorsResult(page)
            };
        }

        public CompetitionResult GetCompetitionResultById(int id)
        {
            return _competitionRepository.GetCompetitionResultById(id);
        }

        public void PostCompetitionResult(CompetitionResult competitionResult)
        {
            foreach (CompetitorResult cr in competitionResult.CompetitorsResult)
            {
                _athleteRepository.PostAthleteCompetitionResult(new AthleteResultModel
                {
                    CompetitionDate = competitionResult.CompetitionDate,
                    CompetitionPlace = competitionResult.CompetitionPlace,
                    CompetitionType = competitionResult.CompetitionType,
                    AthleteResult = cr
                });
            }
            _competitionRepository.PostCompetitionResult(competitionResult);
        }
    }
}