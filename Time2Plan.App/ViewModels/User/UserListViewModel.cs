using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Time2Plan.App.Services;
using Time2Plan.BL.Facades;
using Time2Plan.BL.Facades.Interfaces;
using Time2Plan.BL.Models;


namespace Time2Plan.App.ViewModels;

    public partial class UserListViewModel : ViewModelBase
    {
    private readonly IUserFacade _userFacade;
    private readonly INavigationService _navigationService;
    public IEnumerable<UserListModel> Users { get; set; } = null!;
    public UserListViewModel(
        IUserFacade userFacade,
        INavigationService navigationService,
        IMessengerService messengerService) 
        : base(messengerService)
    {
        _userFacade= userFacade;
        _navigationService= navigationService;
    }
    protected override async Task LoadDataAsync()
    {
        await base.LoadDataAsync();

        Users = await _userFacade.GetAsync();
    }
    

}

