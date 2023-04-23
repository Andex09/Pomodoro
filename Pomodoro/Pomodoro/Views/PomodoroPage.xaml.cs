using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Pomodoro.ViewModels;

namespace Pomodoro.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PomodoroPage : ContentPage
    {
        private readonly PomodoroViewModel _viewModel;
        public PomodoroPage()
        {
            InitializeComponent();
            _viewModel = BindingContext as PomodoroViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}