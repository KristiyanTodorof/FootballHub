using FootballHub.MatchService.Application.DTOs;
using FootballHub.Shared.Models;

namespace FootballHub.MatchService.Application.Services;

public interface IMatchService
{
    Task<ApiResponse<List<MatchDto>>> GetTodaysMatchesAsync();
    Task<ApiResponse<List<MatchDto>>> GetLiveMatchesAsync();
    Task<ApiResponse<MatchDto>> GetMatchByIdAsync(int id);
    Task<ApiResponse<MatchDto>> CreateMatchAsync(CreateMatchRequest request);
    Task<ApiResponse<MatchDto>> StartMatchAsync(int id);
    Task<ApiResponse<MatchDto>> UpdateScoreAsync(int id, UpdateScoreRequest request);
    Task<ApiResponse<MatchDto>> AddMatchEventAsync(int id, AddMatchEventRequest request);
    Task<ApiResponse<MatchDto>> FinishMatchAsync(int id);
}