using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time2Plan.App.Models;
using Time2Plan.App.ViewModels;

namespace Time2Plan.App.Services;

    public class NavigationService : INavigationService
    {
        public IEnumerable<RouteModel> Routes { get; } = new List<RouteModel>
    {
       /* new("//ingredients", typeof(IngredientListView), typeof(IngredientListViewModel)),
        new("//ingredients/detail", typeof(IngredientDetailView), typeof(IngredientDetailViewModel)),

        new("//ingredients/edit", typeof(IngredientEditView), typeof(IngredientEditViewModel)),
        new("//ingredients/detail/edit", typeof(IngredientEditView), typeof(IngredientEditViewModel)),

        new("//recipes", typeof(RecipeListView), typeof(RecipeListViewModel)),
        new("//recipes/detail", typeof(RecipeDetailView), typeof(RecipeDetailViewModel)),

        new("//recipes/edit", typeof(RecipeEditView), typeof(RecipeEditViewModel)),
        new("//recipes/detail/edit", typeof(RecipeEditView), typeof(RecipeEditViewModel)),

        new("//recipes/detail/edit/ingredients", typeof(RecipeIngredientsEditView), typeof(RecipeIngredientsEditViewModel)),*/
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

