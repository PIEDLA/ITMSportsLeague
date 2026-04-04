using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;

namespace SportsLeague.Domain.Interfaces.Services;

public interface ITournamentService
{
    Task<IEnumerable<Tournament>> GetAllAsync();
    Task<Tournament?> GetByIdAsync(int id);
    Task<Tournament> CreateAsync(Tournament tournament);
    Task UpdateAsync(int id, Tournament tournament);
    Task DeleteAsync(int id);
    Task UpdateStatusAsync(int id, TournamentStatus newStatus);
    Task UpdateContractAmountAsync(int id, int sponsorId, decimal contractAmount);
    Task UpdateSponsorCategoryAsync(int id, int sponsorId, SponsorCategory category);
    Task RegisterTeamAsync(int tournamentId, int teamId);
    Task RegisterSponsorAsync(int tournamentId, int sponsorId, decimal contractAmount, SponsorCategory category);
    Task<IEnumerable<Team>> GetTeamsByTournamentAsync(int tournamentId);
    Task<IEnumerable<Sponsor>> GetSponsorsByTournamentAsync(int tournamentId);
}