using CommunityToolkit.Mvvm.Input;
using Time2Plan.App.Messages;
using Time2Plan.App.Services;
using Time2Plan.BL.Facades;
using Time2Plan.BL.Models;

namespace Time2Plan.App.ViewModels;

[QueryProperty(nameof(Project), nameof(Project))]
public partial class ProjectEditViewModel : ViewModelBase
{
    private readonly IProjectFacade _projectFacade;
    private readonly INavigationService _navigationService;

    public ProjectDetailModel Project { get; set; } = ProjectDetailModel.Empty;
    public Guid UserId { get; set; }
    public ProjectEditViewModel(
        IProjectFacade ProjectFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        _projectFacade = ProjectFacade;
        _navigationService = navigationService;
        var viewModel = (AppShellViewModel)Shell.Current.BindingContext;
        UserId = viewModel.UserId;
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        Project = await _projectFacade.GetAsync(Guid.Parse(Project.Id.ToString())) ?? ProjectDetailModel.Empty;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        await _projectFacade.SaveAsync(Project with { UserProjects = default! });
        MessengerService.Send(new ProjectEditMessage { ProjectId = Project.Id });
        _navigationService.SendBackButtonPressed();
    }
}