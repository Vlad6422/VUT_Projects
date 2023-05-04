using Time2Plan.App.Models;
using Time2Plan.App.ViewModels;
//using Time2Plan.App.Views.Activity;
using Time2Plan.App.Views.User;
//using Time2Plan.App.Views.Project;

namespace Time2Plan.App.Services;

    public class NavigationService : INavigationService
    {
        public IEnumerable<RouteModel> Routes { get; } = new List<RouteModel>
    {
        new("//Users", typeof(UserListView), typeof(UserListViewModel)),
        //new("//Activities", typeof(ActivityListView), typeof(ActivityListViewModel)),
        //new("//Activities/detail", typeof(ActivityDetailView), typeof(ActivityDetailViewModel)),

        //new("//Activities/edit", typeof(ActivityEditView), typeof(ActivityEditViewModel)),
        //new("//Activities/detail/edit", typeof(ActivityEditView), typeof(ActivityEditViewModel)),

        //new("//Users", typeof(ProjectListView), typeof(ProjectListViewModel)),
        //new("//Users/detail", typeof(ProjectDetailView), typeof(ProjectDetailViewModel)),

        //new("//Users/edit", typeof(UserEditView), typeof(UserEditViewModel)),
        //new("//Users/detail/edit", typeof(UserEditView), typeof(UserEditViewModel)),

        //new("//Users/detail/edit/Activities", typeof(UserEditView), typeof(UserEditViewModel)),
    };

        public async Task GoToAsync<TViewModel>()
            where TViewModel : IViewModel
        {
            var route = GetRouteByViewModel<TViewModel>();
            await Shell.Current.GoToAsync(route);
        }
        public async Task GoToAsync<TViewModel>(IDictionary<string, object?> parameters)
            where TViewModel : IViewModel
        {
            var route = GetRouteByViewModel<TViewModel>();
            await Shell.Current.GoToAsync(route, parameters);
        }

        public async Task GoToAsync(string route)
            => await Shell.Current.GoToAsync(route);

        public async Task GoToAsync(string route, IDictionary<string, object?> parameters)
            => await Shell.Current.GoToAsync(route, parameters);

        public bool SendBackButtonPressed()
            => Shell.Current.SendBackButtonPressed();

        private string GetRouteByViewModel<TViewModel>()
            where TViewModel : IViewModel
            => Routes.First(route => route.ViewModelType == typeof(TViewModel)).Route;
    }

