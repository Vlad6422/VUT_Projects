﻿using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Time2Plan.BL;

namespace Time2Plan.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            ConfigureAppSettings(builder);

            builder.Services
                .AddDALServices(builder.Configuration)
                //   .AddAppServices()
                .AddBLServices();

            var app = builder.Build();

            app.Services.GetRequiredService<IDbMigrator>().Migrate();
            RegisterRouting(app.Services.GetRequiredService<INavigationService>());

            return app;
        }

        private static void ConfigureAppSettings(MauiAppBuilder builder)
        {
            var configurationBuilder = new ConfigurationBuilder();

            var assembly = Assembly.GetExecutingAssembly();
            const string appSettingsFilePath = "CookBook.App.appsettings.json";
            using var appSettingsStream = assembly.GetManifestResourceStream(appSettingsFilePath);
            if (appSettingsStream is not null)
            {
                configurationBuilder.AddJsonStream(appSettingsStream);
            }

            var configuration = configurationBuilder.Build();
            builder.Configuration.AddConfiguration(configuration);
        }

        private static void RegisterRouting(INavigationService navigationService)
        {
            foreach (var route in navigationService.Routes)
            {
                Routing.RegisterRoute(route.Route, route.ViewType);
            }
        }
    }
