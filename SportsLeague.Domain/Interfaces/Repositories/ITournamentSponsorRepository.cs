using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;

namespace SportsLeague.Domain.Interfaces.Repositories;

public interface ITournamentSponsorRepository : IGenericRepository<TournamentSponsor>
{
    Task<bool> ExistsSponsorOnTournamentAsync(int tournamentId, int sponsorId);
    Task<TournamentSponsor?> GetByTournamentAndSponsorAsync(int tournamentId, int sponsorId);
    Task<IEnumerable<TournamentSponsor>> GetByTournamentAsync(int tournamentId);
    Task<IEnumerable<TournamentSponsor>> GetBySponsorAsync(int sponsorId);
}