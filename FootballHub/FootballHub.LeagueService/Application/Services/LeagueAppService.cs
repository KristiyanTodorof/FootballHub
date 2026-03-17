using FootballHub.LeagueService.Application.DTOs;
using FootballHub.LeagueService.Domain.Entities;
using FootballHub.LeagueService.Infrastructure.Data;
using FootballHub.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballHub.LeagueService.Application.Services;

public class LeagueAppService : ILeagueService
{
    private readonly LeagueDbContext _context;

    public LeagueAppService(LeagueDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<LeagueDto>>> GetAllLeaguesAsync()
    {
        var leagues = await _context.Leagues
            .Include(l => l.Teams)
            .OrderBy(l => l.Name)
            .ToListAsync();

        return ApiResponse<List<LeagueDto>>.Ok(leagues.Select(MapToDto).ToList());
    }

    public async Task<ApiResponse<LeagueDto>> GetLeagueByIdAsync(int id)
    {
        var league = await _context.Leagues
            .Include(l => l.Teams)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (league is null)
            return ApiResponse<LeagueDto>.Fail($"Лига с Id {id} не е намерена.");

        return ApiResponse<LeagueDto>.Ok(MapToDto(league));
    }

    public async Task<ApiResponse<LeagueDto>> CreateLeagueAsync(CreateLeagueRequest request)
    {
        var league = new League
        {
            Name = request.Name,
            Country = request.Country,
            LogoUrl = request.LogoUrl,
            Season = request.Season
        };

        _context.Leagues.Add(league);
        await _context.SaveChangesAsync();

        return ApiResponse<LeagueDto>.Ok(MapToDto(league));
    }

    public async Task<ApiResponse<TeamDto>> CreateTeamAsync(CreateTeamRequest request)
    {
        var leagueExists = await _context.Leagues.AnyAsync(l => l.Id == request.LeagueId);
        if (!leagueExists)
            return ApiResponse<TeamDto>.Fail($"Лига с Id {request.LeagueId} не е намерена.");

        var team = new Team
        {
            Name = request.Name,
            ShortName = request.ShortName,
            Country = request.Country,
            LogoUrl = request.LogoUrl,
            LeagueId = request.LeagueId
        };

        _context.Teams.Add(team);
        await _context.SaveChangesAsync();

        return ApiResponse<TeamDto>.Ok(MapTeamToDto(team));
    }

    public async Task<ApiResponse<List<StandingDto>>> GetStandingsAsync(int leagueId)
    {
        var standings = await _context.Standings
            .Include(s => s.Team)
            .Where(s => s.LeagueId == leagueId)
            .OrderBy(s => s.Position)
            .ToListAsync();

        return ApiResponse<List<StandingDto>>.Ok(standings.Select(MapStandingToDto).ToList());
    }

    public async Task<ApiResponse<StandingDto>> UpdateStandingAsync(int leagueId, UpdateStandingRequest request)
    {
        var standing = await _context.Standings
            .Include(s => s.Team)
            .FirstOrDefaultAsync(s => s.LeagueId == leagueId && s.TeamId == request.TeamId);

        if (standing is null)
        {
            standing = new Standing
            {
                LeagueId = leagueId,
                TeamId = request.TeamId
            };
            _context.Standings.Add(standing);
        }

        standing.Played = request.Played;
        standing.Won = request.Won;
        standing.Drawn = request.Drawn;
        standing.Lost = request.Lost;
        standing.GoalsFor = request.GoalsFor;
        standing.GoalsAgainst = request.GoalsAgainst;
        standing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await RecalculatePositionsAsync(leagueId);

        return ApiResponse<StandingDto>.Ok(MapStandingToDto(standing));
    }

    private async Task RecalculatePositionsAsync(int leagueId)
    {
        var standings = await _context.Standings
            .Where(s => s.LeagueId == leagueId)
            .OrderByDescending(s => s.Won * 3 + s.Drawn)
            .ThenByDescending(s => s.GoalsFor - s.GoalsAgainst)
            .ThenByDescending(s => s.GoalsFor)
            .ToListAsync();

        for (int i = 0; i < standings.Count; i++)
            standings[i].Position = i + 1;

        await _context.SaveChangesAsync();
    }

    private static LeagueDto MapToDto(League league) => new(
        league.Id,
        league.Name,
        league.Country,
        league.LogoUrl,
        league.Season,
        league.Teams.Select(MapTeamToDto).ToList()
    );

    private static TeamDto MapTeamToDto(Team team) => new(
        team.Id,
        team.Name,
        team.ShortName,
        team.Country,
        team.LogoUrl
    );

    private static StandingDto MapStandingToDto(Standing s) => new(
        s.Position,
        s.TeamId,
        s.Team.Name,
        s.Team.ShortName,
        s.Team.LogoUrl,
        s.Played,
        s.Won,
        s.Drawn,
        s.Lost,
        s.GoalsFor,
        s.GoalsAgainst,
        s.GoalDifference,
        s.Points
    );
}