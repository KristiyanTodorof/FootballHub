using FootballHub.LeagueService.Application.DTOs;
using FootballHub.Shared.Models;

namespace FootballHub.LeagueService.Application.Services;

public interface ILeagueService
{
    Task<ApiResponse<List<LeagueDto>>> GetAllLeaguesAsync();
    Task<ApiResponse<LeagueDto>> GetLeagueByIdAsync(int id);
    Task<ApiResponse<LeagueDto>> CreateLeagueAsync(CreateLeagueRequest request);
    Task<ApiResponse<TeamDto>> CreateTeamAsync(CreateTeamRequest request);
    Task<ApiResponse<List<StandingDto>>> GetStandingsAsync(int leagueId);
    Task<ApiResponse<StandingDto>> UpdateStandingAsync(int leagueId, UpdateStandingRequest request);
}