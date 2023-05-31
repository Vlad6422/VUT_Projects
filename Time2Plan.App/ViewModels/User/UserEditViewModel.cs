using CommunityToolkit.Mvvm.Input;
using Time2Plan.App.Messages;
using Time2Plan.App.Services;
using Time2Plan.BL.Facades;
using Time2Plan.BL.Models;

namespace Time2Plan.App.ViewModels;

[QueryProperty(nameof(User), nameof(User))]
public partial class UserEditViewModel : ViewModelBase
{
    private readonly IUserFacade _UserFacade;
    private readonly INavigationService _navigationService;
    private readonly IAlertService _alertService;

    public UserDetailModel User { get; init; } = UserDetailModel.Empty;
    public IEnumerable<UserListModel> Users { get; set; }

    public UserEditViewModel(
        IUserFacade UserFacade,
        INavigationService navigationService,
        IMessengerService messengerService,
        IAlertService alertService)
        : base(messengerService)
    {
        _UserFacade = UserFacade;
        _navigationService = navigationService;
        _alertService = alertService;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (await CheckNicknames(User.NickName, User.Id))
        {
            await _UserFacade.SaveAsync(User);
            MessengerService.Send(new UserEditMessage { UserId = User.Id });
            _navigationService.SendBackButtonPressed();
        }
    }

    public async Task<bool> CheckNicknames(string newNickname, Guid userEditID)
    {
        Users = await _UserFacade.GetAsync();

        foreach (var user in Users)
        {
            if (user.NickName == newNickname && user.Id != userEditID)
            {
                await _alertService.DisplayAsync("Create user failed", "Another user with nickname '" + user.NickName + "' already exists.");
                return false;
            }
        }
        return true;
    }
}
