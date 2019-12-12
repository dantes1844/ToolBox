using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ToolBox.Services;
using ToolBox.Views;
using ToolBox.Services.Bus;

namespace ToolBox
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            DependencyService.Register<BusAppService>();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
