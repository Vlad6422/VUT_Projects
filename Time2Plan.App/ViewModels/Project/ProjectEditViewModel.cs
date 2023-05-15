using CommunityToolkit.Mvvm.Input;
using Time2Plan.App.Messages;
using Time2Plan.App.Services;
using Time2Plan.BL.Facades;
using Time2Plan.BL.Models;

namespace Time2Plan.App.ViewModels;

[QueryProperty(nameof(Project), nameof(Project))]
public partial class ProjectEditViewModel : ViewModelBase
{
	private readonly IProjectFacade _ProjectFacade;
	private readonly INavigationService _navigationService;

	public ProjectDetailModel Project { get; set; }
	public ProjectEditViewModel(
		IProjectFacade ProjectFacade,
		INavigationService navigationService,
		IMessengerService messengerService)
		: base(messengerService)
	{
		_ProjectFacade = ProjectFacade;
		_navigationService = navigationService;
	}

    [RelayCommand]
    private async Task SaveAsync()
    {
        await _ProjectFacade.SaveAsync(Project);
        MessengerService.Send(new ProjectEditMessage { ProjectId = Project.Id });
        _navigationService.SendBackButtonPressed();
    }
}