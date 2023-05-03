using CommunityToolkit.Mvvm.Input;
using Time2Plan.App.Messages;
using Time2Plan.App.Services;
using Time2Plan.BL.Facades.Interfaces;
using Time2Plan.BL.Models;

namespace Time2Plan.App.ViewModels;

[QueryProperty(nameof(User), nameof(User))]
public partial class UserEditViewModel : ViewModelBase
{
    private readonly IUserFacade _UserFacade;
    private readonly INavigationService _navigationService;

    public UserDetailModel User { get; init; } = UserDetailModel.Empty;

    public UserEditViewModel(
        IUserFacade UserFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        _UserFacade = UserFacade;
        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        await _UserFacade.SaveAsync(User);

        MessengerService.Send(new UserEditMessage { UserId = User.Id });

        _navigationService.SendBackButtonPressed();
    }
}
