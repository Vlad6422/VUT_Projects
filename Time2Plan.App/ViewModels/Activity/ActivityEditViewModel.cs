using CommunityToolkit.Mvvm.Input;
using Time2Plan.App.Messages;
using Time2Plan.App.Services;
using Time2Plan.BL.Facades;
using Time2Plan.BL.Models;

namespace Time2Plan.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class ActivityEditViewModel : ViewModelBase
{
	private readonly IActivityFacade _activityFacade;
	private readonly INavigationService _navigationService;

	public Guid Id { get; set; }
	public ActivityDetailModel Activity { get; set; }
	public ActivityEditViewModel(
		IActivityFacade activityFacade,
		INavigationService navigationService,
		IMessengerService messengerService)
		: base(messengerService)
	{
		_activityFacade = activityFacade;
		_navigationService = navigationService;
	}

    [RelayCommand]
    private async Task SaveAsync()
    {
        await _activityFacade.SaveAsync(Activity);
        MessengerService.Send(new ActivityEditMessage { ActivityId = Activity.Id });
        _navigationService.SendBackButtonPressed();
    }
}