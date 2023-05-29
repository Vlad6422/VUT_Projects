using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Time2Plan.App.Messages;
using Time2Plan.App.Services;
using Time2Plan.BL.Facades;
using Time2Plan.BL.Models;
using static Time2Plan.BL.Facades.IActivityFacade;

namespace Time2Plan.App.ViewModels;

public partial class ActivityListViewModel : ViewModelBase, IRecipient<ActivityEditMessage>, IRecipient<ActivityDeleteMessage>, IRecipient<UserChangeMessage>, IRecipient<ProjectDeleteMessage>
{
    private readonly IActivityFacade _activityFacade;
    private readonly INavigationService _navigationService;

    public IEnumerable<ActivityListModel> Activities { get; set; } = null!;

    public Guid userId { get; set; }

    public string[] Filters { get; set; } = Enum.GetNames(typeof(IActivityFacade.Interval));

    public string SelectedFilter { get; set; }

    public Interval Interval { get; set; }

    public DateTime? FilterStart { get; set; }

    public DateTime? FilterEnd { get; set; }

    public ActivityListViewModel(
       IActivityFacade activityFacade,
       INavigationService navigationService,
       IMessengerService messengerService)
       : base(messengerService)
    {
        _activityFacade = activityFacade;
        _navigationService = navigationService;
        var viewModel = (AppShellViewModel)Shell.Current.BindingContext;
        userId = viewModel.UserId;
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        Activities = await _activityFacade.GetAsyncListByUser(userId);

        parseInterval(SelectedFilter);

        if(FilterEnd == null)
        {
            FilterEnd = GetMaxTime(Activities, FilterEnd);
        }
        if(FilterStart == null)
        {
            FilterStart = GetMinTime(Activities, FilterStart);
        }
        OnPropertyChanged(nameof(FilterEnd));

        Activities = await _activityFacade.GetAsyncFilter(userId, FilterStart, FilterEnd, null, null); //TODO
    }

    private void parseInterval(string selectedFilter)
    {
        if (selectedFilter == null)
            return;

        Interval = (Interval)Enum.Parse(typeof(Interval), SelectedFilter);

        DateTime now = DateTime.Now;
        switch (Interval)
        {
            case Interval.Daily:
                FilterStart = now.AddDays(-1);
                break;
            case Interval.Weekly:
                FilterStart = now.AddDays(-7);
                break;
            case Interval.Monthly:
                FilterStart = now.AddMonths(-1);
                break;
            case Interval.Yearly:
                FilterStart = now.AddYears(-1);
                break;
            case Interval.All:
                FilterStart = DateTime.MinValue;
                FilterEnd = DateTime.MaxValue;
                return;
            case Interval.Empty:
                return;
            default:
                throw new Exception("Undefined interval");
        }
        FilterEnd = now;
    }

    [RelayCommand]
    private async Task GoToCreateAsync(Guid userId)
    {
        await _navigationService.GoToAsync($"/edit?userId={userId}");
    }

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
    {
        await _navigationService.GoToAsync<ActivityDetailViewModel>(
            new Dictionary<string, object> { [nameof(ActivityDetailViewModel.Id)] = id });
    }

    public async void Receive(ActivityEditMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(ActivityDeleteMessage message)
    {
        await LoadDataAsync();
    }

    [RelayCommand]
    private async Task GoToRefreshAsync()
    {
        await LoadDataAsync();
    }

    public async void Receive(UserChangeMessage message)
    {
        userId = message.UserId;
        await LoadDataAsync();
    }

    public async void Receive(ProjectDeleteMessage message)
    {
        await LoadDataAsync();
    }

    public static DateTime? GetMinTime(IEnumerable<ActivityListModel> userActivities, DateTime? Start)
    {
        foreach (var userActivity in userActivities)
        {
            if (userActivity.Start < Start)
            {
                Start = userActivity.Start;
            }
        }
        return Start;
    }
    public static DateTime? GetMaxTime(IEnumerable<ActivityListModel> userActivities, DateTime? End)
    {
        foreach (var userActivity in userActivities)
        {
            if (userActivity.End > End)
            {
                End = userActivity.End;
            }
        }
        return End;
    }

    public async void DatePicker_PropertyChanged(object sender, SelectionChangedEventArgs e)
    {
        var datePicker = sender as DatePicker;
        await GoToRefreshAsync();
    }
}
