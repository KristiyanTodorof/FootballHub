using FootballHub.PlayerService.Application.DTOs;
using FootballHub.PlayerService.Domain.Entities;
using FootballHub.PlayerService.Infrastructure.Data;
using FootballHub.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballHub.PlayerService.Application.Services;

public class PlayerAppService : IPlayerService
{
    private readonly PlayerDbContext _context;

    public PlayerAppService(PlayerDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<PlayerDto>>> GetPlayersByTeamAsync(int teamId)
    {
        var players = await _context.Players
            .Include(p => p.Statistics)
            .Where(p => p.TeamId == teamId)
            .OrderBy(p => p.LastName)
            .ToListAsync();

        return ApiResponse<List<PlayerDto>>.Ok(players.Select(MapToDto).ToList());
    }

    public async Task<ApiResponse<List<PlayerDto>>> GetTopScorersAsync(int leagueId, int season)
    {
        var players = await _context.Players
            .Include(p => p.Statistics)
            .Where(p => p.LeagueId == leagueId && p.Statistics.Any(s => s.Season == season))
            .OrderByDescending(p => p.Statistics
                .Where(s => s.Season == season && s.LeagueId == leagueId)
                .Sum(s => s.Goals))
            .Take(20)
            .ToListAsync();

        return ApiResponse<List<PlayerDto>>.Ok(players.Select(MapToDto).ToList());
    }

    public async Task<ApiResponse<List<PlayerDto>>> GetTopAssistsAsync(int leagueId, int season)
    {
        var players = await _context.Players
            .Include(p => p.Statistics)
            .Where(p => p.LeagueId == leagueId && p.Statistics.Any(s => s.Season == season))
            .OrderByDescending(p => p.Statistics
                .Where(s => s.Season == season && s.LeagueId == leagueId)
                .Sum(s => s.Assists))
            .Take(20)
            .ToListAsync();

        return ApiResponse<List<PlayerDto>>.Ok(players.Select(MapToDto).ToList());
    }

    public async Task<ApiResponse<PlayerDto>> GetPlayerByIdAsync(int id)
    {
        var player = await _context.Players
            .Include(p => p.Statistics)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (player is null)
            return ApiResponse<PlayerDto>.Fail($"Играч с Id {id} не е намерен.");

        return ApiResponse<PlayerDto>.Ok(MapToDto(player));
    }

    public async Task<ApiResponse<PlayerDto>> CreatePlayerAsync(CreatePlayerRequest request)
    {
        var player = new Player
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            Nationality = request.Nationality,
            Position = request.Position,
            ShirtNumber = request.ShirtNumber,
            PhotoUrl = request.PhotoUrl,
            TeamId = request.TeamId,
            TeamName = request.TeamName,
            LeagueId = request.LeagueId
        };

        _context.Players.Add(player);
        await _context.SaveChangesAsync();

        return ApiResponse<PlayerDto>.Ok(MapToDto(player));
    }

    public async Task<ApiResponse<PlayerDto>> UpdateStatisticAsync(int playerId, UpdatePlayerStatisticRequest request)
    {
        var player = await _context.Players
            .Include(p => p.Statistics)
            .FirstOrDefaultAsync(p => p.Id == playerId);

        if (player is null)
            return ApiResponse<PlayerDto>.Fail($"Играч с Id {playerId} не е намерен.");

        var stat = player.Statistics
            .FirstOrDefault(s => s.Season == request.Season && s.LeagueId == request.LeagueId);

        if (stat is null)
        {
            stat = new PlayerStatistic
            {
                PlayerId = playerId,
                Season = request.Season,
                LeagueId = request.LeagueId,
                LeagueName = request.LeagueName
            };
            _context.PlayerStatistics.Add(stat);
        }

        stat.Appearances = request.Appearances;
        stat.Goals = request.Goals;
        stat.Assists = request.Assists;
        stat.YellowCards = request.YellowCards;
        stat.RedCards = request.RedCards;
        stat.MinutesPlayed = request.MinutesPlayed;
        stat.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ApiResponse<PlayerDto>.Ok(MapToDto(player));
    }

    private static PlayerDto MapToDto(Player p) => new(
        p.Id,
        p.FirstName,
        p.LastName,
        p.FullName,
        p.DateOfBirth,
        CalculateAge(p.DateOfBirth),
        p.Nationality,
        p.Position,
        p.ShirtNumber,
        p.PhotoUrl,
        p.TeamId,
        p.TeamName,
        p.Statistics.Select(s => new PlayerStatisticDto(
            s.Season,
            s.LeagueName,
            s.Appearances,
            s.Goals,
            s.Assists,
            s.YellowCards,
            s.RedCards,
            s.MinutesPlayed
        )).ToList()
    );

    private static int CalculateAge(DateTime dateOfBirth)
    {
        var today = DateTime.UtcNow;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }
}