using Microsoft.AspNetCore.SignalR;

namespace FootballHub.MatchService.API.Hubs;

public class MatchHub : Hub
{
    public async Task JoinMatch(string matchId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"match_{matchId}");
    }

    public async Task LeaveMatch(string matchId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"match_{matchId}");
    }

    public async Task JoinLiveMatches()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "live_matches");
    }

    public async Task LeaveLiveMatches()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "live_matches");
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}