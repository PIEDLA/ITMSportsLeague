using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;
using System.ComponentModel.DataAnnotations;

namespace SportsLeague.Domain.Services;

public class SponsorService : ISponsorService
{
    private readonly ISponsorRepository _sponsorRepository;
    private readonly ITournamentSponsorRepository _tournamentSponsorRepository;
    private readonly ITournamentRepository _tournamentRepository;
    private readonly ILogger<SponsorService> _logger;

    public SponsorService(
    ISponsorRepository sponsorRepository,
    ITournamentRepository tournamentRepository,
    ITournamentSponsorRepository tournamentSponsorRepository,
    
    ILogger<SponsorService> logger)
    {
        _sponsorRepository = sponsorRepository;
        _tournamentRepository = tournamentRepository;
        _tournamentSponsorRepository = tournamentSponsorRepository;
        _logger = logger;
    }
    public async Task<IEnumerable<Sponsor>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all tournaments");
        return await _sponsorRepository.GetAllAsync();
    }
    public async Task<Sponsor?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving sponsor with ID: {SponsorId}", id);
        var sponsor = await _sponsorRepository.GetByIdWithTournamentsAsync(id);
        if (sponsor == null)
            _logger.LogWarning("Sponsor with ID {SponsorId} not found", id);
        return sponsor;
    }

    public async Task<Sponsor> CreateAsync(Sponsor sponsor)
    {
        // Validación de negocio: nombre único
        var existingSponsor = await _sponsorRepository.ExistsByNameAsync(sponsor.Name);
        if (existingSponsor)
        {
            _logger.LogWarning("Sponsor with name '{SponsorName}' already exists", sponsor.Name);
            throw new InvalidOperationException(
            $"Ya existe un patrocinador con el nombre '{sponsor.Name}'");
        }

        // Validación de negocio: formato de email
        var emailValidator = new EmailAddressAttribute();

        if (!string.IsNullOrEmpty(sponsor.ContactEmail) &&
            !emailValidator.IsValid(sponsor.ContactEmail))
        {
            _logger.LogWarning("Invalid email format for sponsor: {SponsorName}", sponsor.Name);
            throw new InvalidOperationException(
                $"El correo '{sponsor.ContactEmail}' no tiene un formato válido");
        }

        _logger.LogInformation("Creating sponsor: {SponsorName}", sponsor.Name);
        return await _sponsorRepository.CreateAsync(sponsor);
    }

    public async Task UpdateAsync(int id, Sponsor sponsor)
    {
        var existing = await _sponsorRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"No se encontró el patrocinador con ID {id}");

        var existingSponsor = await _sponsorRepository.ExistsByNameAsync(sponsor.Name);
        if (existingSponsor)
        {
            _logger.LogWarning("Sponsor with name '{SponsorName}' already exists", sponsor.Name);
            throw new InvalidOperationException(
            $"Ya existe un patrocinador con el nombre '{sponsor.Name}'");
        }

        existing.Name = sponsor.Name;
        existing.ContactEmail = sponsor.ContactEmail;
        existing.Phone = sponsor.Phone;
        existing.WebsiteUrl = sponsor.WebsiteUrl;

        _logger.LogInformation("Updating sponsor with ID: {SponsorId}", id);
        await _sponsorRepository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(int id)
    {
        var existing = await _sponsorRepository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"No se encontró el patrocinador con ID {id}");
        if (existing.TournamentSponsors != null)
        {
            throw new InvalidOperationException(
            "Solo se pueden eliminar patrocinadores que no tengan contrato activo con un torneo");
        }

        _logger.LogInformation("Deleting sponsor with ID: {SponsorId}", id);
        await _sponsorRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Tournament>> GetTournamentsBySponsorAsync(int sponsorId)
    {
        var sponsor = await _sponsorRepository.GetByIdAsync(sponsorId);
        if (sponsor == null)
            throw new KeyNotFoundException(
            $"No se encontró el patrocinador con ID {sponsorId}");

        if (sponsor.TournamentSponsors == null)
        {
            throw new InvalidOperationException(
            "El sponsor no tiene contrato activo con algún torneo");
        }

        var tournamentSponsor = await _tournamentSponsorRepository
        .GetBySponsorAsync(sponsorId);

        return tournamentSponsor.Select(ts => ts.Tournament);
    }
}