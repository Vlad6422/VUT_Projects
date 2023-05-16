using Time2Plan.App.ViewModels;
using Time2Plan.App.Converters;
namespace Time2Plan.App.Views.User;

public partial class UserEditView
{
    public UserEditView(UserEditViewModel viewModel)
        : base(viewModel)
    {
        Resources.Add("NotEmptyConverter", new NotEmptyConverter());

        InitializeComponent();
    }
}