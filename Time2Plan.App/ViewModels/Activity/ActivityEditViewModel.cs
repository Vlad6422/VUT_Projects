using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Web;
using Time2Plan.App.Messages;
using Time2Plan.App.Services;
using Time2Plan.BL.Facades;
using Time2Plan.BL.Models;

namespace Time2Plan.App.ViewModels;

[QueryProperty(nameof(Activity), nameof(Activity))]
public partial class ActivityEditViewModel : ViewModelBase, INotifyPropertyChanged
{
    private readonly IActivityFacade _activityFacade;
    private readonly INavigationService _navigationService;
    private readonly IAlertService _alertService;

    public string UserId { get; set; }
    public ActivityDetailModel Activity { get; init; } = ActivityDetailModel.Empty;
    public DateTime EndDate { get; set; } = DateTime.UtcNow;
    public TimeSpan EndTime { get; set; } = DateTime.UtcNow.TimeOfDay;

    public DateTime StartDate { get; set; } = DateTime.UtcNow; 
    public TimeSpan StartTime { get; set; } = DateTime.UtcNow.TimeOfDay;

    public ActivityEditViewModel(
        IActivityFacade activityFacade,
        INavigationService navigationService,
        IMessengerService messengerService,
        IAlertService alertService)
        : base(messengerService)
    {
        _activityFacade = activityFacade;
        _navigationService = navigationService;
        _alertService = alertService;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        Activity.Start = StartDate.Add(StartTime);
        Activity.End = EndDate.Add(EndTime);
        if (Activity.Start >= Activity.End)
        {
            await _alertService.DisplayAsync("Invalid Time","Start time is same or greater then end time.");
            return;
        }
        await _activityFacade.SaveAsync(Activity);
        MessengerService.Send(new ActivityEditMessage { ActivityId = Activity.Id });
        _navigationService.SendBackButtonPressed();
    }

    
}