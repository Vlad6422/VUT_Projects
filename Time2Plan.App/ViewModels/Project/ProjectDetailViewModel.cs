using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Time2Plan.App.Messages;
using Time2Plan.App.Services;
using Time2Plan.BL.Facades.Interfaces;
using Time2Plan.BL.Models;

namespace Time2Plan.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]
public partial class ProjectDetailViewModel : ViewModelBase, IRecipient<ProjectEditMessage>, IRecipient<ProjectActivityAddMessage>, IRecipient<ProjectActivityDeleteMessage>
{
    private readonly IProjectFacade _ProjectFacade;
    private readonly INavigationService _navigationService;

    public Guid Id { get; set; }
    public ProjectDetailModel? Project { get; set; }

    public ProjectDetailViewModel(
        IProjectFacade ProjectFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        _ProjectFacade = ProjectFacade;
        _navigationService = navigationService;
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Project = await _ProjectFacade.GetAsync(Id);
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Project is not null)
        {
            await _ProjectFacade.DeleteAsync(Project.Id);

            MessengerService.Send(new ProjectDeleteMessage());

            _navigationService.SendBackButtonPressed();
        }
    }


    [RelayCommand]
    private async Task GoToEditAsync()
    {
        if (Project is not null)
        {
            await _navigationService.GoToAsync("/edit",
                new Dictionary<string, object?> { [nameof(ProjectEditViewModel.Project)] = Project with { } });
        }
    }

    public async void Receive(ProjectEditMessage message)
    {
        if (message.ProjectId == Project?.Id)
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
