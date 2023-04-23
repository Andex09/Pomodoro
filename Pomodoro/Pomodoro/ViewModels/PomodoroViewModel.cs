using Newtonsoft.Json;
using Pomodoro.Models;
using Pomodoro.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Xamarin.Forms;

namespace Pomodoro.ViewModels
{
    public class PomodoroViewModel : BaseViewModel
    {
        #region Properties
        private Timer timer;
        private int pomodoroDuration;
        private int breakDuration;

        private TimeSpan ellapsed;

        public TimeSpan Ellapsed
        {
            get { return ellapsed; }
            set { SetProperty(ref ellapsed, value); }
        }

        private bool isRunning;

        public bool IsRunning
        {
            get { return isRunning; }
            set { SetProperty(ref isRunning,value); }
        }

        private bool isInBreak;

        public bool IsInBreak
        {
            get { return isInBreak; }
            set { SetProperty(ref isInBreak, value); }
        }

        private int duration;

        public int Duration
        {
            get { return duration; }
            set { SetProperty(ref duration, value); }
        }

        #endregion

        public PomodoroViewModel()
        {
            LoadConfiguredValues();
            InitializeTimer();
        }

        #region Commands
        public ICommand StartOrPauseCommand => new Command( () => {
            if (isRunning)
            {
                StopTimer();
            }
            else
            {
                StartTimer();
            }
        });

        public ICommand GoToConfigurationCommand => new Command(async () => {
            if (isRunning)
            {
                StopTimer();
            }

            await Shell.Current.GoToAsync($"{nameof(ConfigurationPage)}");

        });
        #endregion

        #region Methods

        private void LoadConfiguredValues()
        {
            IsInBreak = false;
            IsRunning = false;

            try
            {
                if (Application.Current.Properties.ContainsKey(Literals.PomodoroDuration))
                {
                    pomodoroDuration = (int)Application.Current.Properties[Literals.PomodoroDuration];
                }

                if (Application.Current.Properties.ContainsKey(Literals.BreakDuration))
                {
                    breakDuration = (int)Application.Current.Properties[Literals.BreakDuration];
                }

                Duration = pomodoroDuration * 60;
                Ellapsed = new TimeSpan(0, pomodoroDuration, 0);
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Debe configurar primero el pomodoro", "Close");
            }
            
        }

        private void InitializeTimer()
        {
            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;            
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Ellapsed = Ellapsed.Add(TimeSpan.FromSeconds(-1));
            if (!isInBreak && Ellapsed.TotalSeconds == 0)
            {
                IsInBreak = true;
                Ellapsed = new TimeSpan(0, breakDuration, 0);

                await SavePomodoroAsync();
            }

            if (IsInBreak && Ellapsed.TotalSeconds == 0)
            {
                IsInBreak = false;
                Ellapsed = new TimeSpan(0, pomodoroDuration, 0);
            }
        }

        private async Task SavePomodoroAsync()
        {
            List<DateTime> history;
            if (Application.Current.Properties.ContainsKey(Literals.History))
            {
                var json = Application.Current.Properties[Literals.History].ToString();
                history = JsonConvert.DeserializeObject<List<DateTime>>(json);
            }
            else
            {
                history = new List<DateTime>();
            }

            history.Add(DateTime.Now);
            var serializedObject = JsonConvert.SerializeObject(history);

            Application.Current.Properties[Literals.History] = serializedObject;
            await Application.Current.SavePropertiesAsync();
        }

        private void StartTimer()
        {
            IsRunning = true;
            timer.Start();
        }

        private void StopTimer()
        {
            IsRunning = false;
            timer.Stop();
        }

        public void OnAppearing()
        {
            LoadConfiguredValues();
            InitializeTimer();
        }
        #endregion
    }
}
