using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Time2Plan.App.Messages;
using Time2Plan.App.Services;
using Time2Plan.BL.Facades.Interfaces;
using Time2Plan.BL.Models;

namespace Time2Plan.App.ViewModels;

public partial class ProjectListViewModel : ViewModelBase, IRecipient<ProjectEditMessage>, IRecipient<ProjectDeleteMessage>
{
    private readonly IProjectFacade _ProjectFacade;
    private readonly INavigationService _navigationService;

    public IEnumerable<ProjectListModel> Projects { get; set; } = null!;

    public ProjectListViewModel(
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

        Projects = await _ProjectFacade.GetAsync();
    }

    [RelayCommand]
    private async Task GoToDetailAsync(Guid id)
        => await _navigationService.GoToAsync<ProjectDetailViewModel>(
            new Dictionary<string, object?> { [nameof(ProjectDetailViewModel.Id)] = id });

    [RelayCommand]
    private async Task GoToCreateAsync()
    {
        await _navigationService.GoToAsync("/edit");
    }

    public async void Receive(ProjectEditMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(ProjectDeleteMessage message)
    {
        await LoadDataAsync();
    }
}
