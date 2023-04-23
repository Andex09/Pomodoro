using Pomodoro.Models;
using Pomodoro.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Pomodoro.ViewModels
{
    public class ConfigurationViewModel : BaseViewModel
    {
        #region Properties

        //private ObservableCollection<int> breakDurations;

        //public ObservableCollection<int> BreakDurations
        //{
        //    get { return breakDurations; }
        //    set { SetProperty(ref breakDurations, value); }
        //}

        private int selectedPomodoroDuration;

        public int SelectedPomodoroDurations
        {
            get { return selectedPomodoroDuration; }
            set { SetProperty(ref selectedPomodoroDuration, value); }
        }

        private int selectedBreakDuration;

        public int SelectedBreakDurations
        {
            get { return selectedBreakDuration; }
            set { SetProperty(ref selectedBreakDuration, value); }
        }

        #endregion

        public ConfigurationViewModel()
        {
            LoadConfiguration();
        }

        public ICommand SaveCommand => new Command(async (o) =>
        {
            if (selectedPomodoroDuration <1 || selectedBreakDuration < 1)
            {
                await Application.Current.MainPage.DisplayAlert("Error", 
                                                        "Value must be more than cero", 
                                                        "Ok");
                return;
            }

            Application.Current.Properties[Literals.PomodoroDuration] = SelectedPomodoroDurations;
            Application.Current.Properties[Literals.BreakDuration] = SelectedBreakDurations;

            await Application.Current.SavePropertiesAsync();

            await Shell.Current.GoToAsync($"//{nameof(PomodoroPage)}");
        });

        #region Methods

        private void LoadConfiguration()
        {
            if (Application.Current.Properties.ContainsKey(Literals.PomodoroDuration))
            {
                SelectedPomodoroDurations = (int)Application.Current.Properties[Literals.PomodoroDuration];
            }

            if (Application.Current.Properties.ContainsKey(Literals.BreakDuration))
            {
                SelectedBreakDurations = (int)Application.Current.Properties[Literals.BreakDuration];
            }
        }

        #endregion

    }
}
