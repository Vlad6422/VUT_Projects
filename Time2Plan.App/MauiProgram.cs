using CommunityToolkit.Maui;
using Time2Plan.BL;
using Time2Plan.App.Services;

namespace Time2Plan.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

             builder.Services
                 .AddDALServices(builder.Configuration)
                 .AddAppServices()
                 .AddBLServices();

            var app = builder.Build();

            app.Services.GetRequiredService<IDbMigrator>().Migrate();
            RegisterRouting(app.Services.GetRequiredService<INavigationService>());

            return app;
        }

        private static void RegisterRouting(INavigationService navigationService)
        {
            foreach (var route in navigationService.Routes)
            {
                Routing.RegisterRoute(route.Route, route.ViewType);
            }
        }
    }
}
