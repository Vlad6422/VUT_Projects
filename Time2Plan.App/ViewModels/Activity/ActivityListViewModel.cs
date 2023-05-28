using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Time2Plan.App.Messages;
using Time2Plan.App.Services;
using Time2Plan.BL.Facades;
using Time2Plan.BL.Models;

namespace Time2Plan.App.ViewModels;

public partial class ActivityListViewModel : ViewModelBase, IRecipient<ActivityEditMessage>, IRecipient<ActivityDeleteMessage>, IRecipient<UserChangeMessage>
{
    private readonly IActivityFacade _activityFacade;
    private readonly INavigationService _navigationService;

    public IEnumerable<ActivityListModel> Activities { get; set; } = null!;

    public Guid userId { get; set; }

    public ActivityListViewModel(
       IActivityFacade ingredientFacade,
       INavigationService navigationService,
       IMessengerService messengerService)
       : base(messengerService)
    {
        _activityFacade = ingredientFacade;
        _navigationService = navigationService;
        var viewModel = (AppShellViewModel)Shell.Current.BindingContext;
        userId = viewModel.UserId;
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Activities = await _activityFacade.GetAsyncListByUser(userId);
    }

    [RelayCommand]
    private async Task GoToCreateAsync(Guid userId)
    {
        await _navigationService.GoToAsync($"/edit?userId={userId}");
    }

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
    {
        await _navigationService.GoToAsync<ActivityDetailViewModel>(
            new Dictionary<string, object> { [nameof(ActivityDetailViewModel.Id)] = id });
    }

    public async void Receive(ActivityEditMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(ActivityDeleteMessage message)
    {
        await LoadDataAsync();
    }

    [RelayCommand]
    private async Task GoToRefreshAsync()
    {
        await LoadDataAsync();
    }

    public async void Receive(UserChangeMessage message)
    {
        userId = message.UserId;
        await LoadDataAsync();
    }
}
