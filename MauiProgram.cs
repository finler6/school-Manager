using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using SchoolManage.App.Services;
using SchoolManage.BL;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using SchoolManage.DAL;
using SchoolManage.DAL.Migrator;
using SchoolManage.DAL.Options;
using SchoolManage.App.ViewModels;
using SchoolManage.App.Views;
using SchoolManage.App.Views.Subject;
using SchoolManage.App.Views.Student;
using System.Diagnostics;
using SchoolManage.App.Shells;
using SchoolManage.BL.Facades;
using Microsoft.EntityFrameworkCore;

[assembly: System.Resources.NeutralResourcesLanguage("en")]
namespace SchoolManage.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        Debug.WriteLine("Starting CreateMauiApp");

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        Debug.WriteLine("Configuring app settings");

        ConfigureAppSettings(builder);

        Debug.WriteLine("Registering services");

        builder.Services
            .AddDALServices(GetDALOptions(builder.Configuration))
            .AddAppServices()
            .AddBLServices()
            .AddTransient<LoginPage>()
            .AddTransient<SubjectTableViewModel>()
            .AddTransient<MainPageView>()
            .AddTransient<SubjectListView>()
            .AddTransient<SubjectTableView>()
            .AddSingleton<SubjectListViewModel>()
            .AddSingleton<MainPageViewModel>()
            .AddTransient<StudentEditViewModel>()
            .AddTransient<StudentSubjectEditViewModel>()
            .AddTransient<StudentSubjectEditView>()
            .AddTransient<StudentEditView>()
            .AddTransient<StudentSubjectEditView>()
            .AddSingleton<INavigationService, NavigationService>()
            .AddSingleton<ISubjectFacade, SubjectFacade>();
            //
        builder.Services.AddDbContext<SchoolManageDbContext>(options =>
        {
            options.UseSqlite("Data Source=SchoolManage.db")
                   .LogTo(Console.WriteLine, LogLevel.Information); 
        });

        var app = builder.Build();

        Debug.WriteLine("Migrating database and registering routes");

        try
        {
            MigrateDb(app.Services.GetRequiredService<IDbMigrator>());
            RegisterRouting(app.Services.GetRequiredService<INavigationService>());
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error occurred: {ex.Message}");
            throw;
        }

        Debug.WriteLine("Finished CreateMauiApp");

        return app;
    }

    private static void ConfigureAppSettings(MauiAppBuilder builder)
    {
        var configurationBuilder = new ConfigurationBuilder();

        var assembly = Assembly.GetExecutingAssembly();
        const string appSettingsFilePath = "SchoolManage.App.appsettings.json";
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

    private static DALOptions GetDALOptions(IConfiguration configuration)
    {
        DALOptions dalOptions = new()
        {
            DatabaseDirectory = FileSystem.AppDataDirectory
        };
        configuration.GetSection("SchoolManage:DAL").Bind(dalOptions);
        return dalOptions;
    }

    private static void MigrateDb(IDbMigrator migrator) => migrator.Migrate();
}
