namespace Time2Plan.App.Messages;

public record ActivityEditMessage
{
    public required Guid ActivityId { get; init; }
}