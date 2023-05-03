using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Time2Plan.App.Messages;
using Time2Plan.App.Services;
using Time2Plan.BL.Facades.Interfaces;
using Time2Plan.BL.Models;

namespace Time2Plan.App.ViewModels;

[QueryProperty(nameof(Project), nameof(Project))]
public partial class ProjectEditViewModel : ViewModelBase, IRecipient<ProjectActivityEditMessage>, IRecipient<ProjectActivityAddMessage>, IRecipient<ProjectActivityDeleteMessage>
{
    private readonly IProjectFacade _ProjectFacade;
    private readonly INavigationService _navigationService;

    public ProjectDetailModel Project { get; set; } = ProjectDetailModel.Empty;

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
    private async Task GoToProjectActivityEditAsync()
    {
        await _navigationService.GoToAsync("/activities",
            new Dictionary<string, object?> { [nameof(ProjectActivitiesEditViewModel.Project)] = Project });
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        await _ProjectFacade.SaveAsync(Project with{ Activities = default! });

        MessengerService.Send(new ProjectEditMessage { ProjectId = Project.Id});

        _navigationService.SendBackButtonPressed();
    }

    public async void Receive(ProjectActivityEditMessage message)
    {
        await ReloadDataAsync();
    }

    public async void Receive(ProjectActivityAddMessage message)
    {
        await ReloadDataAsync();
    }

    public async void Receive(ProjectActivityDeleteMessage message)
    {
        await ReloadDataAsync();
    }

    private async Task ReloadDataAsync()
    {
        Project = await _ProjectFacade.GetAsync(Project.Id)
                 ?? ProjectDetailModel.Empty;
    }
}
