using CommunityToolkit.Mvvm.Input;
using Time2Plan.App.Messages;
using Time2Plan.App.Services;
using Time2Plan.BL.Facades.Interfaces;
using Time2Plan.BL.Mappers.Interfaces;
using Time2Plan.BL.Models;
using System.Collections.ObjectModel;

namespace Time2Plan.App.ViewModels;

[QueryProperty(nameof(Project), nameof(Project))]
public partial class ProjectActivitiesEditViewModel : ViewModelBase
{
    private readonly IActivityFacade _ActivityFacade;
    private readonly IActivityModelMapper _ActivityModelMapper;

    public ProjectDetailModel? Project { get; set; }
  
    public ObservableCollection<ActivityListModel> Activities { get; set; } = new();

    public ActivityListModel? ActivitySelected { get; set; }

    public ProjectActivitiesEditViewModel(
        IActivityFacade ActivityFacade,

        IMessengerService messengerService)
        : base(messengerService)
    {
        _ActivityFacade = ActivityFacade;
    }

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Activities.Clear();
        var Activities = await _ActivityFacade.GetAsync();
        foreach (var Activity in Activities)
        {
            Activities.Add(Activity);
        }
    }

    [RelayCommand]
    private async Task AddNewActivityToProjectAsync()
    {
        //if (ActivityNew is not null
        //    && ActivitySelected is not null
        //    && Project is not null)
        //{
        //    _ActivityAmountModelMapper.MapToExistingDetailModel(ActivityAmountNew, ActivitySelected);

        //    await _ActivityAmountFacade.SaveAsync(ActivityAmountNew, Project.Id);
        //    Project.Activities.Add(_ActivityAmountModelMapper.MapToListModel(ActivityAmountNew));

        //    ActivityAmountNew = GetActivityAmountNew();

            MessengerService.Send(new ProjectActivityAddMessage());
        
    }

    [RelayCommand]
    private async Task UpdateActivityAsync(ActivityDetailModel? model)
    {
        if (model is not null
            && Project is not null)
        {
            await _ActivityFacade.SaveAsync(model);

            MessengerService.Send(new ProjectActivityEditMessage());
        }
    }

    [RelayCommand]
    private async Task RemoveActivityAsync(ActivityListModel model)
    {
        if (Project is not null)
        {
            await _ActivityFacade.DeleteAsync(model.Id);
            Project.Activities.Remove(model);

            MessengerService.Send(new ProjectActivityDeleteMessage());
        }
    }

    private ActivityDetailModel GetActivityAmountNew()
    {
        var ActivityFirst = Activities.First();
        return new()
        {
            Id = Guid.NewGuid(),
            Start = ActivityFirst.Start,
            End = ActivityFirst.End,
            Tag = ActivityFirst.Tag,
            Type = ActivityFirst.Type,
        };
    }
}
