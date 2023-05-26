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

    public string UserId { get; set; }
    public ActivityDetailModel Activity { get; init; } = ActivityDetailModel.Empty;
    public DateTime EndDate { get; set; } = DateTime.Today.AddHours(1);
    public TimeSpan EndTime { get; set; }

    public DateTime StartDate { get; set; } = DateTime.Today; 
    public TimeSpan StartTime { get; set; }

    public ActivityEditViewModel(
        IActivityFacade activityFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        _activityFacade = activityFacade;
        _navigationService = navigationService;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        Activity.Start = StartDate.Add(StartTime);
        Activity.End = EndDate.Add(EndTime);
        await _activityFacade.SaveAsync(Activity);
        MessengerService.Send(new ActivityEditMessage { ActivityId = Activity.Id });
        _navigationService.SendBackButtonPressed();
    }

    
}