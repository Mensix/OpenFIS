using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Playwright;
using OpenFIS.Models;
using OpenFIS.Models.Competition;
using OpenFIS.Services;

namespace OpenFIS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompetitionController : ControllerBase
    {
        private readonly ICompetitionService _competitionService;
        public CompetitionController(ICompetitionService competitionService) => _competitionService = competitionService;

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetCompetitionById(int id)
        {
            CompetitionResult matchedCompetitionResult = _competitionService.GetCompetitionResultById(id);
            if (matchedCompetitionResult == null)
            {
                using IPlaywright playwright = await Playwright.CreateAsync();
                await using IBrowser browser = await playwright.Chromium.LaunchAsync();
                IPage page = await browser.NewPageAsync();
                await page.GotoAsync($"https://www.fis-ski.com/DB/general/results.html?sectorcode=JP&raceid={id}");

                CompetitionType competitionType = await _competitionService.GetCompetitionType(page);
                if (competitionType == CompetitionType.NotFound)
                {
                    return NotFound(new ErrorMessage { Message = "Competition not found." });
                }
                else if (competitionType == CompetitionType.TeamCup)
                {
                    return NotFound(new ErrorMessage { Message = "Team competitions are not supported." });
                }

                CompetitionResult competitionResult = await _competitionService.GetCompetitionResult(page);
                _competitionService.PostCompetitionResult(competitionResult);
                return Ok(competitionResult);
            }

            return Ok(matchedCompetitionResult);
        }
    }
}