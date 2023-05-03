using Time2Plan.App.ViewModels;

namespace Time2Plan.App.Views.Project;

public partial class ProjectListView
{
	public ProjectListView(ProjectListViewModel viewModel)
		: base(viewModel)
	{
		InitializeComponent();
	}
}