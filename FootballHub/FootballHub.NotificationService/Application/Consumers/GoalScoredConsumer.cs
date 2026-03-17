using FootballHub.Contracts.Events;
using FootballHub.NotificationService.Application.Services;
using MassTransit;
using static FootballHub.Contracts.Events.MatchEvents;

namespace FootballHub.NotificationService.Application.Consumers;

public class GoalScoredConsumer : IConsumer<GoalScored>
{
    private readonly INotificationService _notificationService;

    public GoalScoredConsumer(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task Consume(ConsumeContext<GoalScored> context)
    {
        var message = context.Message;

        await _notificationService.SendGoalScoredAsync(
            message.MatchId,
            message.Team,
            message.PlayerName,
            message.Minute,
            message.HomeScore,
            message.AwayScore);
    }
}