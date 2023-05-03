using Time2Plan.App.ViewModels;

namespace Time2Plan.App.Views.User;

public partial class UserListView
{
    public UserListView(UserListViewModel viewModel) 
        : base(viewModel)        
    {
        InitializeComponent();
    }
}