using OpenFIS.Models;
using OpenFIS.Repositories;

namespace OpenFIS.Services
{
    public interface IAthleteService
    {
        Athlete GetAthleteByFisCode(int fisCode);
        AthleteResultModel[] GetAthleteCompetitionResults(int fisCode);
    }

    public class AthleteService : IAthleteService
    {
        private readonly IAthleteRepository _athleteRepository;
        public AthleteService(IAthleteRepository athleteRepository) => _athleteRepository = athleteRepository;

        public Athlete GetAthleteByFisCode(int fisCode)
        {
            return _athleteRepository.GetAthleteByFisCode(fisCode);
        }

        public AthleteResultModel[] GetAthleteCompetitionResults(int fisCode)
        {
            return _athleteRepository.GetAthleteCompetitionResults(fisCode);
        }
    }
}