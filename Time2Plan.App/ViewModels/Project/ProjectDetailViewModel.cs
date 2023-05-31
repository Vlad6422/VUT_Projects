using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Time2Plan.App.Messages;
using Time2Plan.App.Services;
using Time2Plan.BL.Facades;
using Time2Plan.BL.Models;

namespace Time2Plan.App.ViewModels;

[QueryProperty(nameof(Id), nameof(Id))]

public partial class ProjectDetailViewModel : ViewModelBase, IRecipient<ProjectEditMessage>, IRecipient<ProjectJoinMessage>
{
    private readonly IProjectFacade _projectFacade;
    private readonly INavigationService _navigationService;
    private readonly IUserProjectFacade _userProjectFacade;
    private readonly IAlertService _alertService;
    private readonly IUserFacade _userFacade;
    public Guid Id { get; set; }
    public ProjectDetailModel Project { get; set; }
    public Guid UserId { get; set; }
    public UserDetailModel User { get; set; }

    public bool IsMember { get; set; }
    public bool IsNotMember { get; set; }

    public ProjectDetailViewModel(
        IProjectFacade projectFacade,
        INavigationService navigationService,
        IMessengerService messengerService,
        IUserProjectFacade userProjectFacade,
        IAlertService alertService,
        IUserFacade userFacade)
        : base(messengerService)
    {
        _projectFacade = projectFacade;
        _navigationService = navigationService;
        var viewModel = (AppShellViewModel)Shell.Current.BindingContext;
        UserId = viewModel.UserId;
        _userProjectFacade = userProjectFacade;
        _alertService = alertService;
        _userFacade = userFacade;
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Project = await _projectFacade.GetAsync(Id);
        User = await _userFacade.GetAsync(UserId);
        if(Project is not null)
        {
            IsMember = (Project.UserProjects.Any(up => up.UserId == UserId));
            IsNotMember = !IsMember;
        }
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (Project is not null)
        {
            try
            {
                await _projectFacade.DeleteAsync(Project.Id);
                MessengerService.Send(new ProjectDeleteMessage());
                _navigationService.SendBackButtonPressed();
            }
            catch
            {
                await _alertService.DisplayAsync("Project delete failed", "Failed to delete " + Project.Name + " because other users are joined.");
            }
        }
    }

    [RelayCommand]
    private async Task GoToEditAsync()
    {
        if (Project is not null)
        {
            await _navigationService.GoToAsync("/edit",
                new Dictionary<string, object> { [nameof(ProjectEditViewModel.Project)] = Project with { } });
        }
    }

    [RelayCommand]
    private async Task JoinProjectAsync()
    {
        var model = new UserProjectDetailModel()
        {
            UserId = UserId,
            ProjectId = Guid.Parse(Project.Id.ToString()),
            UserName = User.Name ?? string.Empty,
            UserSurname = User.Surname ?? string.Empty,
            UserNickname = User.NickName ?? string.Empty,
            Photo = User.Photo,
            ProjectName = Project.Name
        };
        await _userProjectFacade.SaveAsync(model);
        var listmodel = new UserProjectListModel()
        {
            UserId = UserId,
            ProjectId = Guid.Parse(Project.Id.ToString()),
            UserName = User.Name ?? string.Empty,
            Surname = User.Surname ?? string.Empty,
            Nickname = User.NickName ?? string.Empty,
            Photo = User.Photo,
            ProjectName = Project.Name
        };

        Project.UserProjects.Add(listmodel);
        IsMember = true;
        IsNotMember = false;

        MessengerService.Send(new ProjectJoinMessage());
        await _alertService.DisplayAsync("Project joined", "Successfully joined to " + Project.Name + ".");
    }
    [RelayCommand]
    private async Task LeaveProjectAsync()
    {
        bool result = await _alertService.DisplayConfirmAsync("Leave project", "Are you sure you want to leave '" + Project.Name + "' and delete all activities in this project?");
        if (result)
        {
            try
            {
                var model = Project.UserProjects.Single(up => up.UserId == UserId);
                await _userProjectFacade.DeleteAsync(model.Id);
                Project.UserProjects.Remove(model);
                IsMember = false;
                IsNotMember = true;
                MessengerService.Send(new ProjectLeaveMessage { ProjectId = Id });
                await _alertService.DisplayAsync("Project left", "Successfully left " + Project.Name + ".");
            }
            catch
            {
                await _alertService.DisplayAsync("Error Leave", "Error while trying to leave project.");
            }
        }
    }
    public async void Receive(ProjectEditMessage message)
    {
        if (message.ProjectId == Project?.Id)
        {
            await LoadDataAsync();
        }
    }

    public async void Receive(ProjectJoinMessage message)
    {
        await LoadDataAsync();
    }

}
