using FootballHub.Contracts.Events;
using FootballHub.NotificationService.Application.Services;
using MassTransit;
using static FootballHub.Contracts.Events.MatchEvents;

namespace FootballHub.NotificationService.Application.Consumers;

public class MatchStartedConsumer : IConsumer<MatchStarted>
{
    private readonly INotificationService _notificationService;

    public MatchStartedConsumer(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task Consume(ConsumeContext<MatchStarted> context)
    {
        var message = context.Message;

        await _notificationService.SendMatchStartedAsync(
            message.MatchId,
            message.HomeTeam,
            message.AwayTeam);
    }
}