using Microsoft.EntityFrameworkCore;
using OpenFIS.Models;
using OpenFIS.Models.Competition;
using OpenFIS.Models.Competition.Competitor;

namespace OpenFIS
{
    public class FisDbContext : DbContext
    {
        public FisDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Athlete> Athletes { get; set; }
        public DbSet<AthleteResultModel> AthleteResults { get; set; }
        public DbSet<CompetitionPlace> CompetitionPlaces { get; set; }
        public DbSet<CompetitionResult> CompetitionResults { get; set; }
        public DbSet<CompetitorResult> CompetitorResults { get; set; }
        public DbSet<CompetitorResultJumps> CompetitorResultJumps { get; set; }
    }
}