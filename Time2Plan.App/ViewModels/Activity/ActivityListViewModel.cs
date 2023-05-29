using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Time2Plan.App.Messages;
using Time2Plan.App.Services;
using Time2Plan.BL.Facades;
using Time2Plan.BL.Models;

namespace Time2Plan.App.ViewModels;

public partial class ActivityListViewModel : ViewModelBase, IRecipient<ActivityEditMessage>, IRecipient<ActivityDeleteMessage>, IRecipient<UserChangeMessage>, IRecipient<ProjectDeleteMessage>
{
    private readonly IActivityFacade _activityFacade;
    private readonly INavigationService _navigationService;

    public IEnumerable<ActivityListModel> Activities { get; set; } = null!;

    public Guid userId { get; set; }

    public string[] Filters { get; set; }

    public string SelectedFilter { get; set; }

    public DateTime FilterStart { get; set; } = DateTime.Now;

    public DateTime FilterEnd { get; set; } = DateTime.Now;

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
        SelectedFilter = Enum.GetName(IActivityFacade.Interval.All);
        Filters = Enum.GetNames(typeof(IActivityFacade.Interval));
        FilterEnd = GetMaxTime(Activities, FilterEnd);
        OnPropertyChanged(nameof(FilterEnd));
        FilterStart = GetMinTime(Activities, FilterStart);
        Activities = await _activityFacade.GetAsyncFilter(userId, FilterStart, FilterEnd, null, null, IActivityFacade.Interval.All); //TODO
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

    public static DateTime GetMinTime(IEnumerable<ActivityListModel> userActivities, DateTime Start)
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
    public static DateTime GetMaxTime(IEnumerable<ActivityListModel> userActivities, DateTime End)
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
