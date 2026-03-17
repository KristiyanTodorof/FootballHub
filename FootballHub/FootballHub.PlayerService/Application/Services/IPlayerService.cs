using FootballHub.PlayerService.Application.DTOs;
using FootballHub.Shared.Models;

namespace FootballHub.PlayerService.Application.Services;

public interface IPlayerService
{
    Task<ApiResponse<List<PlayerDto>>> GetPlayersByTeamAsync(int teamId);
    Task<ApiResponse<List<PlayerDto>>> GetTopScorersAsync(int leagueId, int season);
    Task<ApiResponse<List<PlayerDto>>> GetTopAssistsAsync(int leagueId, int season);
    Task<ApiResponse<PlayerDto>> GetPlayerByIdAsync(int id);
    Task<ApiResponse<PlayerDto>> CreatePlayerAsync(CreatePlayerRequest request);
    Task<ApiResponse<PlayerDto>> UpdateStatisticAsync(int playerId, UpdatePlayerStatisticRequest request);
}