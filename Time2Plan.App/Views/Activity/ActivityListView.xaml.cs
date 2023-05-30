using Microsoft.UI.Xaml;
using Time2Plan.App.ViewModels;

namespace Time2Plan.App.Views.Activity;

public partial class ActivityListView
{
    public ActivityListView(ActivityListViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
        DatePickerStart.DateSelected += FilterDatePicker_DateSelected;
        DatePickerEnd.DateSelected += FilterDatePicker_DateSelected;
    }

    private void DatePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
    }
    private void FilterDatePicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        PickerFilter.SelectedIndex = 5;
        RefreshButton.Command.Execute(RefreshButton.CommandParameter);
        
    }
    
}


