using Time2Plan.App.Converters;
using Time2Plan.App.ViewModels;

namespace Time2Plan.App.Views.Activity;

public partial class ActivityEditView
{
    public ActivityEditView(ActivityEditViewModel viewModel) : base(viewModel)
    {
        Resources.Add("NotEmptyConverter", new NotEmptyConverter());
        InitializeComponent();
    }
}