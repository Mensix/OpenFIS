using System.Linq;
using Microsoft.EntityFrameworkCore;
using OpenFIS.Models;

namespace OpenFIS.Repositories
{
    public interface IAthleteRepository
    {
        Athlete GetAthleteByFisCode(int fisCode);
        AthleteResultModel[] GetAthleteCompetitionResults(int fisCode);
        void PostAthleteCompetitionResult(AthleteResultModel athleteResult);
    }

    public class AthleteRepository : IAthleteRepository
    {
        private readonly FisDbContext _context;
        public AthleteRepository(FisDbContext context) => _context = context;

        public Athlete GetAthleteByFisCode(int fisCode)
        {
            return _context.Athletes.SingleOrDefault(x => x.FisCode == fisCode);
        }

        public AthleteResultModel[] GetAthleteCompetitionResults(int fisCode)
        {
            return _context.AthleteResults.Include(x => x.CompetitionPlace).Include(x => x.AthleteResult).ThenInclude(x => x.Jumps).Where(x => x.AthleteResult.Athlete.FisCode == fisCode).ToArray();
        }

        public void PostAthleteCompetitionResult(AthleteResultModel athleteResult)
        {
            _context.AthleteResults.Add(athleteResult);
            _context.SaveChanges();
        }
    }
}