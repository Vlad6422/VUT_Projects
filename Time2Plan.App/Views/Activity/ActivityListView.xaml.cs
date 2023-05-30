using Microsoft.UI.Xaml;
using Time2Plan.App.ViewModels;
using Time2Plan.BL.Facades;

namespace Time2Plan.App.Views.Activity;

public partial class ActivityListView
{
    public ActivityListView(ActivityListViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
        DatePickerStart.DateSelected += FilterDatePicker_DateSelected;
        DatePickerEnd.DateSelected += FilterDatePicker_DateSelected;
        PickerFilter.SelectedIndexChanged += OnPickerFilterSelected;
    }

    private void DatePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
    }
    private void FilterDatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        PickerFilter.SelectedIndex = (int)IActivityFacade.Interval.Manual;
        RefreshButton.Command.Execute(RefreshButton.CommandParameter);
        
    }
    private void OnPickerFilterSelected(object sender, EventArgs e)
    {
        RefreshButton.Command.Execute(RefreshButton.CommandParameter);
    }

}


