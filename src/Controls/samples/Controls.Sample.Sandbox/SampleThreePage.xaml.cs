using Maui.Controls.Sample.ViewModels;
using Timer = System.Timers.Timer;

namespace Maui.Controls.Sample
{
    public partial class SampleThreePage : ContentPage
    {
        private readonly Timer _updateTimer;
        private bool _timerIsRunning;

        public SampleThreePage()
            : this(new HomePageViewModel())
        {
        }

        public SampleThreePage(HomePageViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            // Timer to simulate real-time updates
            _updateTimer = new Timer
            {
                AutoReset = true,
                Interval = 100 // 500ms interval
            };
            _updateTimer.Elapsed += OnTimerElapsed;
        }

        ~SampleThreePage()
        {
            StopTimer();
            _updateTimer.Elapsed -= OnTimerElapsed;
            _updateTimer.Dispose();
        }

        private void OnTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (BindingContext is HomePageViewModel viewModel)
                {
                    viewModel.UpdateRandomCards();
                }
            });
        }

        public void StartTimer()
        {
            if (_timerIsRunning)
            {
                return;
            }
            _timerIsRunning = true;
            _updateTimer.Start();
            System.Diagnostics.Debug.WriteLine("PocPage Timer started");
        }

        public void StopTimer()
        {
            if (!_timerIsRunning)
                return;
            _timerIsRunning = false;
            _updateTimer.Stop();
            System.Diagnostics.Debug.WriteLine("PocPage Timer stopped");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            System.Diagnostics.Debug.WriteLine("PocPage OnAppearing");
            StartTimer();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            System.Diagnostics.Debug.WriteLine("PocPage OnDisappearing");
            StopTimer();
        }

        private void OnGroupHeaderTapped(object sender, TappedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Group header tapped");
        }
    }
}