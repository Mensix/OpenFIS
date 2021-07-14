using Microsoft.AspNetCore.Mvc;
using OpenFIS.Models;
using OpenFIS.Services;

namespace OpenFIS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AthleteController : ControllerBase
    {
        private readonly IAthleteService _athleteService;
        public AthleteController(IAthleteService athleteService) => _athleteService = athleteService;

        [HttpGet]
        [Route("{fisCode}")]
        public IActionResult GetAthleteByFisCode(int fisCode)
        {
            Athlete matchedAthlete = _athleteService.GetAthleteByFisCode(fisCode);
            if (matchedAthlete == null)
            {
                return NotFound(new ErrorMessage { Message = "Athlete not found." });
            }

            return Ok(matchedAthlete);
        }

        [HttpGet]
        [Route("{fisCode}/competitions")]
        public IActionResult GetAthleteCompetitionResults(int fisCode)
        {
            AthleteResultModel[] matchedAthleteResults = _athleteService.GetAthleteCompetitionResults(fisCode);
            if (matchedAthleteResults.Length == 0)
            {
                return NotFound(new ErrorMessage { Message = "Athlete results not found." });
            }

            return Ok(matchedAthleteResults);
        }
    }
}