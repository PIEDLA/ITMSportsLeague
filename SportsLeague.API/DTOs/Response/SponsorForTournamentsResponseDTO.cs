using SportsLeague.Domain.Enums;

namespace SportsLeague.API.DTOs.Response;

public class SponsorForTournamentsResponseDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string WebsiteUrl { get; set; } = string.Empty;
    public decimal ContractAmount { get; set; }
    public SponsorCategory Category { get; set; }
    public DateTime JoinedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}