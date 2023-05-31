namespace Time2Plan.App.Services;

public class AlertService : IAlertService
{
    public async Task DisplayAsync(string title, string message)
    {
        var displayAlert = Application.Current?.MainPage?.DisplayAlert(title, message, "OK");

        if (displayAlert is not null)
        {
            await displayAlert;
        }
    }

    public async Task<bool> DisplayConfirmAsync(string title, string message)
    {
        var result = await Application.Current?.MainPage?.DisplayAlert(title, message, "Yes", "No");
        return result;
    }
}