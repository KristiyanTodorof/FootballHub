using FootballHub.Contracts.Events;
using FootballHub.MatchService.Application.DTOs;
using FootballHub.MatchService.Domain.Entities;
using FootballHub.MatchService.Infrastructure.Data;
using FootballHub.Shared.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using static FootballHub.Contracts.Events.MatchEvents;

namespace FootballHub.MatchService.Application.Services;

public class MatchAppService : IMatchService
{
    private readonly MatchDbContext _context;
    private readonly IPublishEndpoint _publishEndpoint;

    public MatchAppService(MatchDbContext context, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<ApiResponse<List<MatchDto>>> GetTodaysMatchesAsync()
    {
        var today = DateTime.UtcNow.Date;
        var matches = await _context.Matches
            .Include(m => m.Events)
            .Where(m => m.KickOff.Date == today)
            .OrderBy(m => m.KickOff)
            .ToListAsync();

        return ApiResponse<List<MatchDto>>.Ok(matches.Select(MapToDto).ToList());
    }

    public async Task<ApiResponse<List<MatchDto>>> GetLiveMatchesAsync()
    {
        var matches = await _context.Matches
            .Include(m => m.Events)
            .Where(m => m.Status == MatchStatus.Live || m.Status == MatchStatus.HalfTime)
            .OrderBy(m => m.KickOff)
            .ToListAsync();

        return ApiResponse<List<MatchDto>>.Ok(matches.Select(MapToDto).ToList());
    }

    public async Task<ApiResponse<MatchDto>> GetMatchByIdAsync(int id)
    {
        var match = await _context.Matches
            .Include(m => m.Events)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (match is null)
            return ApiResponse<MatchDto>.Fail($"Match with Id {id} is not found.");

        return ApiResponse<MatchDto>.Ok(MapToDto(match));
    }

    public async Task<ApiResponse<MatchDto>> CreateMatchAsync(CreateMatchRequest request)
    {
        var match = new Match
        {
            HomeTeamId = request.HomeTeamId,
            HomeTeamName = request.HomeTeamName,
            AwayTeamId = request.AwayTeamId,
            AwayTeamName = request.AwayTeamName,
            LeagueId = request.LeagueId,
            LeagueName = request.LeagueName,
            KickOff = request.KickOff
        };

        _context.Matches.Add(match);
        await _context.SaveChangesAsync();

        return ApiResponse<MatchDto>.Ok(MapToDto(match));
    }

    public async Task<ApiResponse<MatchDto>> StartMatchAsync(int id)
    {
        var match = await _context.Matches.FindAsync(id);
        if (match is null)
            return ApiResponse<MatchDto>.Fail($"Match with Id {id} is not found.");

        match.Status = MatchStatus.Live;
        match.Minute = 1;
        match.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        await _publishEndpoint.Publish(new MatchStarted
        {
            MatchId = match.Id,
            HomeTeam = match.HomeTeamName,
            AwayTeam = match.AwayTeamName,
            KickOff = match.KickOff
        });

        return ApiResponse<MatchDto>.Ok(MapToDto(match));
    }

    public async Task<ApiResponse<MatchDto>> UpdateScoreAsync(int id, UpdateScoreRequest request)
    {
        var match = await _context.Matches.FindAsync(id);
        if (match is null)
            return ApiResponse<MatchDto>.Fail($"Match with Id  {id}  is not found.");

        match.HomeScore = request.HomeScore;
        match.AwayScore = request.AwayScore;
        match.Minute = request.Minute;
        match.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return ApiResponse<MatchDto>.Ok(MapToDto(match));
    }

    public async Task<ApiResponse<MatchDto>> AddMatchEventAsync(int id, AddMatchEventRequest request)
    {
        var match = await _context.Matches
            .Include(m => m.Events)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (match is null)
            return ApiResponse<MatchDto>.Fail($"Match with Id  {id}  is not found.");

        var matchEvent = new MatchEvent
        {
            MatchId = id,
            Type = request.Type,
            Minute = request.Minute,
            PlayerId = request.PlayerId,
            PlayerName = request.PlayerName,
            AssistPlayerName = request.AssistPlayerName,
            Team = request.Team
        };

        _context.MatchEvents.Add(matchEvent);

        if (request.Type == MatchEventType.Goal)
        {
            if (request.Team == match.HomeTeamName) match.HomeScore++;
            else match.AwayScore++;

            await _publishEndpoint.Publish(new GoalScored
            {
                MatchId = match.Id,
                Team = request.Team,
                PlayerName = request.PlayerName,
                Minute = request.Minute,
                HomeScore = match.HomeScore,
                AwayScore = match.AwayScore
            });
        }
        else if (request.Type == MatchEventType.OwnGoal)
        {
            if (request.Team == match.HomeTeamName) match.AwayScore++;
            else match.HomeScore++;
        }

        match.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return ApiResponse<MatchDto>.Ok(MapToDto(match));
    }

    public async Task<ApiResponse<MatchDto>> FinishMatchAsync(int id)
    {
        var match = await _context.Matches
            .Include(m => m.Events)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (match is null)
            return ApiResponse<MatchDto>.Fail($"Match with Id  {id}  is not found.");

        match.Status = MatchStatus.Finished;
        match.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        await _publishEndpoint.Publish(new MatchFinished
        {
            MatchId = match.Id,
            HomeScore = match.HomeScore,
            AwayScore = match.AwayScore,
            FinishedAt = DateTime.UtcNow
        });

        return ApiResponse<MatchDto>.Ok(MapToDto(match));
    }

    private static MatchDto MapToDto(Match match) => new(
        match.Id,
        match.HomeTeamName,
        match.AwayTeamName,
        match.LeagueName,
        match.KickOff,
        match.Status.ToString(),
        match.HomeScore,
        match.AwayScore,
        match.Minute,
        match.Events.Select(e => new MatchEventDto(
            e.Id,
            e.Type.ToString(),
            e.Minute,
            e.PlayerName,
            e.AssistPlayerName,
            e.Team
        )).ToList()
    );
}