using SportsLeague.Domain.Enums;

namespace SportsLeague.API.DTOs.Request;
public class RegisterSponsorDTO
{
    public int SponsorId { get; set; }
    public decimal ContractAmount { get; set; }
    public SponsorCategory SponsorCategory { get; set; }
}