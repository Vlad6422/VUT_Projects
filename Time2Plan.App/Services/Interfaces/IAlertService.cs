namespace Time2Plan.App.Services;

public interface IAlertService
{
    Task DisplayAsync(string title, string message);
}
