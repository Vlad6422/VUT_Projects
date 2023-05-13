using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Time2Plan.App.Messages;
using Time2Plan.App.Services;
using Time2Plan.BL.Facades;
using Time2Plan.BL.Models;
namespace Time2Plan.App.ViewModels;
[QueryProperty(nameof(Id), nameof(Id))]
public partial class UserDetailViewModel : ViewModelBase, IRecipient<UserEditMessage>, IRecipient<ProjectActivityAddMessage>, IRecipient<ProjectActivityDeleteMessage>
{
    private readonly IUserFacade _userFacade;
    private readonly INavigationService _navigationService;

    public Guid Id { get; set; }
    public UserDetailModel? User { get; set; }

    public UserDetailViewModel(
        IUserFacade userFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        _userFacade = userFacade;
        _navigationService = navigationService;
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        User = await _userFacade.GetAsync(Id);
    }
    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (User is not null)
        {
            await _userFacade.DeleteAsync(User.Id);

            MessengerService.Send(new UserDeleteMessage());

            _navigationService.SendBackButtonPressed();
        }
    }


    [RelayCommand]
    private async Task GoToEditAsync()
    {
        if (User is not null)
        {
            await _navigationService.GoToAsync("/edit",
                new Dictionary<string, object?> { [nameof(UserEditViewModel.User)] = User with { } });
        }
    }
    public async void Receive(UserEditMessage message)
    {
        if (message.UserId == User?.Id)
        {
            await LoadDataAsync();
        }
    }

    public async void Receive(ProjectActivityAddMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(ProjectActivityDeleteMessage message)
    {
        await LoadDataAsync();
    }
}
