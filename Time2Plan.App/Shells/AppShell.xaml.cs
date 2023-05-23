using CommunityToolkit.Mvvm.Input;
using Time2Plan.App.Services;
using Time2Plan.App.ViewModels;

namespace Time2Plan.App.Shells;

public partial class AppShell : Shell
{
    private readonly INavigationService _navigationService;
    private AppShellViewModel _appShellViewModel;

    public AppShell(INavigationService navigationService, IMessengerService messengerService)
    {
        InitializeComponent();
        _appShellViewModel = new AppShellViewModel(messengerService);
        _navigationService = navigationService;
        BindingContext = _appShellViewModel;
    }

    [RelayCommand]
    private async Task GoToUsersAsync()
        => await _navigationService.GoToAsync<UserListViewModel>();
}

//[RelayCommand]
//private async Task GoToActivitiesAsync()
//    => await _navigationService.GoToAsync<UserListViewModel>();
//} 
