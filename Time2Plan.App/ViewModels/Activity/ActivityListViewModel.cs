using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Time2Plan.App.Messages;
using Time2Plan.App.Services;
using Time2Plan.BL.Facades.Interfaces;
using Time2Plan.BL.Models;

namespace Time2Plan.App.ViewModels;

public partial class ActivityListViewModel : ViewModelBase, IRecipient<ActivityEditMessage>, IRecipient<ActivityDeleteMessage>
{
    private readonly IActivityFacade _ActivityFacade;
    private readonly INavigationService _navigationService;

    public IEnumerable<ActivityListModel> Activitys { get; set; } = null!;

    public ActivityListViewModel(
        IActivityFacade ActivityFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        _ActivityFacade = ActivityFacade;
        _navigationService = navigationService;
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Activitys = await _ActivityFacade.GetAsync();
    }

    [RelayCommand]
    private async Task GoToCreateAsync()
    {
        await _navigationService.GoToAsync("/edit");
    }

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
    {
        await _navigationService.GoToAsync<ActivityDetailViewModel>(
            new Dictionary<string, object?> { [nameof(ActivityDetailViewModel.Id)] = id });
    }

    public async void Receive(ActivityEditMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(ActivityDeleteMessage message)
    {
        await LoadDataAsync();
    }
}
