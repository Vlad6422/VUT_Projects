namespace Time2Plan.App.Messages;

public record UserEditMessage
{
    public required Guid UserId { get; init; }
}