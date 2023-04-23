using Pomodoro.ViewModels;
using Pomodoro.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Pomodoro
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            Routing.RegisterRoute(nameof(PomodoroPage), typeof(PomodoroPage));
            Routing.RegisterRoute(nameof(ConfigurationPage), typeof(ConfigurationPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
