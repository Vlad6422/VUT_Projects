using Time2Plan.App.Services;
using Time2Plan.BL.Facades;
using Time2Plan.BL.Models;

namespace Time2Plan.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class ActivityDetailViewModel : ViewModelBase
{
	private readonly IActivityFacade _activityFacade;
	private readonly INavigationService _navigatationService;

	public Guid Id { get; set; }
	public ActivityDetailModel Activity { get; set; }
	public ActivityDetailViewModel(
		IActivityFacade activityFacade,
		INavigationService navigationService,
		IMessengerService messengerService)
		: base(messengerService)
	{
		_activityFacade = activityFacade;
		_navigatationService = navigationService;
	}

	protected override async Task LoadDataAsync()
	{
		await base.LoadDataAsync();
		Activity = await _activityFacade.GetAsync(Id);
	}
}