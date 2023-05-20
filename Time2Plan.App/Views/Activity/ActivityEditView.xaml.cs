using Time2Plan.App.Converters;
using Time2Plan.App.ViewModels;

namespace Time2Plan.App.Views.Activity;

[QueryProperty(nameof(UserId), "UserId")]
public partial class ActivityEditView
{
    public string UserId { get; set; }
    public ActivityEditView(ActivityEditViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}