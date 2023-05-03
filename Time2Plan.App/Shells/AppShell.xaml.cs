using CommunityToolkit.Mvvm.Input;
using Time2Plan.App.Services;
using Time2Plan.App.ViewModels;

namespace Time2Plan.App.Shells;

public partial class AppShell
{
    private readonly INavigationService _navigationService;

    public AppShell(INavigationService navigationService)
    {
        _navigationService = navigationService;

        InitializeComponent();
    }

    [RelayCommand]
    private async Task GoToUsersAsync()
        => await _navigationService.GoToAsync<UserListViewModel>();

    [RelayCommand]
    private async Task GoToActivitiesAsync()
        => await _navigationService.GoToAsync<UserListViewModel>();
}
