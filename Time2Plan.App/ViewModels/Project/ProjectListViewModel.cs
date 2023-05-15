using CommunityToolkit.Mvvm.Input;
using Time2Plan.App.Messages;
using Time2Plan.App.Services;
using Time2Plan.BL.Facades;
using Time2Plan.BL.Models;

namespace Time2Plan.App.ViewModels;

public partial class ProjectListViewModel : ViewModelBase
{
    private readonly IProjectFacade _projectFacade;
    private readonly INavigationService _navigationService;

    public IEnumerable<ProjectListModel> Projects { get; set; } = null!;

    public ProjectListViewModel(
        IProjectFacade projectFacade,
        INavigationService navigationService,
        IMessengerService messengerService)
        : base(messengerService)
    {
        _projectFacade = projectFacade;
        _navigationService = navigationService;
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

    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();
        Projects = await _projectFacade.GetAsync();
    }
    public async void Receive(UserEditMessage message)
    {
        await LoadDataAsync();
    }

    public async void Receive(UserDeleteMessage message)
    {
        await LoadDataAsync();
    }
}
