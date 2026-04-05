using AutoMapper;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;

namespace SportsLeague.API.Mappings;

public class MappingProfile : Profile
{

    public MappingProfile()
    {
        // Team mappings
        CreateMap<TeamRequestDTO, Team>();
        CreateMap<Team, TeamResponseDTO>();

        // Player mappings
        CreateMap<PlayerRequestDTO, Player>();
        CreateMap<Player, PlayerResponseDTO>()
        .ForMember(
        dest => dest.TeamName,
        opt => opt.MapFrom(src => src.Team.Name));
        
        // Referee mappings
        CreateMap<RefereeRequestDTO, Referee>();
        CreateMap<Referee, RefereeResponseDTO>();

        // Sponsor mappings
        CreateMap<SponsorRequestDTO, Sponsor>();
        CreateMap<Sponsor, SponsorResponseDTO>()
        .ForMember(
            dest => dest.TournamentsCount,
            opt => opt.MapFrom(src =>
            src.TournamentSponsors != null ? src.TournamentSponsors.Count : 0));
        CreateMap<TournamentSponsor, SponsorForTournamentsResponseDTO>()
            .ForMember(dest => dest.Name,
            opt => opt.MapFrom(
                src => src.Sponsor.Name))
            .ForMember(dest => dest.ContactEmail,
            opt => opt.MapFrom(
                src => src.Sponsor.ContactEmail))
            .ForMember(dest => dest.Phone,
            opt => opt.MapFrom(
                src => src.Sponsor.Phone))
            .ForMember(dest => dest.WebsiteUrl,
            opt => opt.MapFrom(
                src => src.Sponsor.WebsiteUrl));

        // Tournament mappings
        CreateMap<TournamentRequestDTO, Tournament>();
        CreateMap<Tournament, TournamentResponseDTO>()
        .ForMember(
        dest => dest.TeamsCount,
        opt => opt.MapFrom(src =>
        src.TournamentTeams != null ? src.TournamentTeams.Count : 0))
        .ForMember(
        dest => dest.SponsorsCount,
        opt => opt.MapFrom(src =>
        src.TournamentSponsors != null ? src.TournamentSponsors.Count : 0));
    }
}