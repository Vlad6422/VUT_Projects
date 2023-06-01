namespace Time2Plan.App.Messages;

public record UserChangeMessage
{
    public required Guid UserId { get; init; }
}