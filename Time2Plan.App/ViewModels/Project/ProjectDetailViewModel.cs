using CommunityToolkit.Mvvm.Input;
using Time2Plan.App.Messages;
using Time2Plan.App.Services;
using Time2Plan.BL.Facades;
using Time2Plan.BL.Models;

namespace Time2Plan.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]

public partial class ProjectDetailViewModel : ViewModelBase
{
    private readonly IProjectFacade _projectFacade;
    private readonly INavigationService _navigationService;

    public Guid Id { get; set; }
    public ProjectDetailModel? Project { get; set; }

    public ProjectDetailViewModel(
        IProjectFacade projectFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        _projectFacade = projectFacade;
        _navigationService = navigationService;
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Project = await _projectFacade.GetAsync(Id);
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Project is not null) 
        {
            await _projectFacade.DeleteAsync(Id);

            MessengerService.Send(new ProjectDeleteMessage());

            _navigationService.SendBackButtonPressed();
        }
    }

}
