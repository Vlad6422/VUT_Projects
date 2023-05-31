namespace Time2Plan.App.Messages;

public record ProjectLeaveMessage
{
    public required Guid ProjectId { get; init; }
}