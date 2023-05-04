using Time2Plan.App.ViewModels;

namespace Time2Plan.App.Views.User;

public partial class UserDetailView
{
	public UserDetailView(UserDetailViewModel viewModel)
        : base(viewModel)
    {
		InitializeComponent();
	}
}