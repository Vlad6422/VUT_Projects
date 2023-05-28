using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Web;
using Time2Plan.App.Messages;
using Time2Plan.App.Services;
using Time2Plan.BL.Facades;
using Time2Plan.BL.Models;
using Time2Plan.DAL.Migrations;

namespace Time2Plan.App.ViewModels;

[QueryProperty(nameof(Activity), nameof(Activity))]
public partial class ActivityEditViewModel : ViewModelBase, INotifyPropertyChanged
{
    private readonly IActivityFacade _activityFacade;
    private readonly INavigationService _navigationService;
    private readonly IAlertService _alertService;
    private readonly IUserFacade _userFacade;
    private readonly IProjectFacade _projectFacade;
    //public Guid _userID { get; set; }

    public Guid UserId { get; set; }
    public ActivityDetailModel Activity { get; init; } = ActivityDetailModel.Empty;
    public DateTime EndDate { get; set; } = DateTime.Now;
    public TimeSpan EndTime { get; set; } = DateTime.Now.TimeOfDay;

    public DateTime StartDate { get; set; } = DateTime.Now; 
    public TimeSpan StartTime { get; set; } = DateTime.Now.TimeOfDay;

    public UserDetailModel User { get; set; }
    public string[] Projects { get; set; }

    public ProjectDetailModel Project { get; set; }
    public string SelectedProject { get; set; }

    public string UserName { get; set; }

    public ActivityEditViewModel(
        IActivityFacade activityFacade,
        INavigationService navigationService,
        IMessengerService messengerService,
        IAlertService alertService,
        IUserFacade userFacade,
        IProjectFacade projectFacade)
        : base(messengerService)
    {
        _activityFacade = activityFacade;
        _navigationService = navigationService;
        _alertService = alertService;
        var viewModel = (AppShellViewModel)Shell.Current.BindingContext;
        UserId = viewModel.UserId;
        _userFacade = userFacade;
        _projectFacade = projectFacade;
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        User = await _userFacade.GetAsync(UserId);
        UserName = User.Surname;
        Project = await _projectFacade.GetAsync(Activity.ProjectId); // nefunguje pri pridani aktivity - projekt je null - je treba zjisti pres uzivatele
        //Projects = User.UserProjects.Select(up => up.ProjectName).ToArray();
        //Projects = User.UserProjects.Select(_ => _.ProjectName).ToArray();
        //Projects = User.UserProjects.Where(up => up.UserId == User.Id).Select(p => p.ProjectName).ToArray();
        Projects = new string[] { Project.Name }; //User.UserProjects.Select(p => p.ProjectName).ToArray();
        SelectedProject = User.UserProjects.Where(up => up.ProjectId == Activity.ProjectId).Select(up => up.ProjectName).FirstOrDefault();
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        Activity.Start = StartDate.Date.Add(StartTime);
        Activity.End = EndDate.Date.Add(EndTime);
        if (Activity.Start >= Activity.End)
        {
            await _alertService.DisplayAsync("Invalid Time", "Activity cannot be add because start time is same or greater then end time.");
            return;
        }

        var user_Activities = await _activityFacade.GetAsyncListByUser(UserId);

        if (CheckActivitiesTime(user_Activities, Activity))
        {
            await _alertService.DisplayAsync("Activities Overlap", "Activity cannot be add because different activity is planned for this time.");
            return;
        }

        await _activityFacade.SaveAsync(Activity);
        MessengerService.Send(new ActivityEditMessage { ActivityId = Activity.Id });
        _navigationService.SendBackButtonPressed();
    }

    public static bool CheckActivitiesTime(IEnumerable<ActivityListModel> userActivities, ActivityDetailModel newActivity)
    {
        foreach (var userActivity in userActivities)
        {
            if (userActivity.Id == newActivity.Id) continue;

            if (newActivity.Start < userActivity.End && userActivity.Start < newActivity.End)
            {
                return true;
            }
        }
        return false;
    }


}