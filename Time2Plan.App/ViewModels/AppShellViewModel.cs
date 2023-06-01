
using Time2Plan.App.Services;

namespace Time2Plan.App.ViewModels;

public class AppShellViewModel : ViewModelBase
{
    public Guid UserId { get; set; }
    public AppShellViewModel(IMessengerService messengerService) : base(messengerService)
    {
    }
}