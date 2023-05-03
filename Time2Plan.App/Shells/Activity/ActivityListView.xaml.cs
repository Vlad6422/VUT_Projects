using Time2Plan.App.ViewModels;

namespace Time2Plan.App.Views.Activity;

public partial class ActivityListView
{
	public ActivityListView(ActivityListViewModel viewModel)
		: base(viewModel)
	{
		InitializeComponent();
	}
}