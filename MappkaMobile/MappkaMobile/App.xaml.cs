using MappkaMobile.View;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace MappkaMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            Run();
        }

        protected override void OnSleep()
        {
            Run();
        }

        protected override void OnResume()
        {
            Run();
        }

        private void Run()
        {
            Preferences.Clear();
            var id = Preferences.Get("id", "");

            if (id != string.Empty)
                App.Current.MainPage = new ServicePage();
        }
    }
}
