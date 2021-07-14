using System.Linq;
using Microsoft.EntityFrameworkCore;
using OpenFIS.Models.Competition;

namespace OpenFIS.Repositories
{
    public interface ICompetitionRepository
    {
        CompetitionResult GetCompetitionResultById(int id);
        void PostCompetitionResult(CompetitionResult competitionResult);
    }

    public class CompetitionRepository : ICompetitionRepository
    {
        private readonly FisDbContext _context;
        public CompetitionRepository(FisDbContext context) => _context = context;

        public CompetitionResult GetCompetitionResultById(int id)
        {
            return _context.CompetitionResults.Include(x => x.CompetitionPlace).Include(x => x.CompetitionPlace).Include(x => x.CompetitorsResult.OrderBy(x => x.Rank)).ThenInclude(x => x.Athlete).FirstOrDefault(x => x.Id == id);
        }

        public void PostCompetitionResult(CompetitionResult competitionResult)
        {
            _context.CompetitionResults.Add(competitionResult);
            _context.SaveChanges();
        }
    }
}