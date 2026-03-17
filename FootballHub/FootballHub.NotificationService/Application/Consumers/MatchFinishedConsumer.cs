using FootballHub.Contracts.Events;
using FootballHub.NotificationService.Application.Services;
using MassTransit;
using static FootballHub.Contracts.Events.MatchEvents;

namespace FootballHub.NotificationService.Application.Consumers;

public class MatchFinishedConsumer : IConsumer<MatchFinished>
{
    private readonly INotificationService _notificationService;

    public MatchFinishedConsumer(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task Consume(ConsumeContext<MatchFinished> context)
    {
        var message = context.Message;

        await _notificationService.SendMatchFinishedAsync(
            message.MatchId,
            message.HomeScore,
            message.AwayScore);
    }
}