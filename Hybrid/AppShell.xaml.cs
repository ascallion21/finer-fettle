using CommunityToolkit.Maui.Alerts;
using Core.Models.Options;
using Microsoft.Extensions.Options;
using System.Windows.Input;

namespace Hybrid
{
    public partial class AppShell : Shell
    {
        /// <param name="serviceProvider">https://github.com/dotnet/maui/issues/11485</param>
        public AppShell(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            //Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            //Routing.RegisterRoute(nameof(NewsletterPage), typeof(NewsletterPage));

            BindingContext = serviceProvider.GetRequiredService<AppShellViewModel>();
        }
    }

    public class AppShellViewModel
    {
        public IOptions<SiteSettings> SiteSettings { get; set; }

        public ICommand SourceCommand { private set; get; }
        public ICommand LogoutCommand { private set; get; }

        public AppShellViewModel(IOptions<SiteSettings> siteSettings, IServiceProvider serviceProvider)
        {
            SiteSettings = siteSettings;
            SourceCommand = new Command<string>(async (string arg) =>
            {
                await Browser.Default.OpenAsync(arg, BrowserLaunchMode.SystemPreferred);
            });
            LogoutCommand = new Command(() =>
            {
                Preferences.Default.Clear(nameof(PreferenceKeys.Email));
                Preferences.Default.Clear(nameof(PreferenceKeys.Token));

                if (Application.Current != null)
                {
                    _ = Toast.Make("Logged out.").Show();
                    Application.Current.MainPage = serviceProvider.GetRequiredService<AuthShell>();
                }
            });
        }
    }
}