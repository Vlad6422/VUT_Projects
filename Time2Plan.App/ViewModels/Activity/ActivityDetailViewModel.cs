using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Time2Plan.App.Messages;
using Time2Plan.App.Resources.Texts;
using Time2Plan.App.Services;
using Time2Plan.BL.Facades.Interfaces;
using Time2Plan.BL.Models;

namespace Time2Plan.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class ActivityDetailViewModel : ViewModelBase, IRecipient<ActivityEditMessage>
{
    private readonly IActivityFacade _ActivityFacade;
    private readonly INavigationService _navigationService;
    private readonly IAlertService _alertService;

    public Guid Id { get; set; }
    public ActivityDetailModel? Activity { get; private set; }

    public ActivityDetailViewModel(
        IActivityFacade ActivityFacade,
        INavigationService navigationService,
        IMessengerService messengerService,
        IAlertService alertService)
        : base(messengerService)
    {
        _ActivityFacade = ActivityFacade;
        _navigationService = navigationService;
        _alertService = alertService;
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Activity = await _ActivityFacade.GetAsync(Id);
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Activity is not null)
        {
            try
            {
                await _ActivityFacade.DeleteAsync(Activity.Id);
                MessengerService.Send(new ActivityDeleteMessage());
                _navigationService.SendBackButtonPressed();
            }
            catch (InvalidOperationException)
            {
                await _alertService.DisplayAsync(ActivityDetailViewModelTexts.DeleteError_Alert_Title, ActivityDetailViewModelTexts.DeleteError_Alert_Message);
            }
        }
    }

    [RelayCommand]
    private async Task GoToEditAsync()
    {
        await _navigationService.GoToAsync("/edit",
            new Dictionary<string, object?> { [nameof(ActivityEditViewModel.Activity)] = Activity });
    }

    public async void Receive(ActivityEditMessage message)
    {
        if (message.ActivityId == Activity?.Id)
        {
            await LoadDataAsync();
        }
    }
}
