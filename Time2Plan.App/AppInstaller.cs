using CommunityToolkit.Mvvm.Messaging;
using Time2Plan.App.Services;
using Time2Plan.App.ViewModels;
using Time2Plan.App.Views;

namespace Time2Plan.App;

public static class AppInstaller
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<AppShell>();

        services.AddSingleton<IMessenger>(_ => StrongReferenceMessenger.Default);
        services.AddSingleton<IMessengerService, MessengerService>();

        services.AddSingleton<IAlertService, AlertService>();

        services.Scan(selector => selector
            .FromAssemblyOf<App>()
            .AddClasses(filter => filter.AssignableTo<ContentPageBase>())
            .AsSelf()
            .WithTransientLifetime());

        services.Scan(selector => selector
            .FromAssemblyOf<App>()
            .AddClasses(filter => filter.AssignableTo<IViewModel>())
            .AsSelfWithInterfaces()
            .WithTransientLifetime());

        services.AddTransient<INavigationService,NavigationService> ();

        return services;
    }
}
