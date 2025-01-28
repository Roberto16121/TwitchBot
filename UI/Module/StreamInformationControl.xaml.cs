
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TwitchBot.UI_Parts
{
    /// <summary>
    /// Interaction logic for StreamInformationControl.xaml
    /// </summary>
    public partial class StreamInformationControl : UserControl
    {

        public static StreamInformationControl Instance { get; private set; } 

        DispatcherTimer timer = new();
        public StreamInformationControl()
        {
            InitializeComponent();
            if(Instance == null)
                Instance = this;

            SetupTimer();
        }

        private void SetupTimer()
        {
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        DateTime started = DateTime.MinValue;

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (started == DateTime.MinValue)
                return;
            GetUptime(DateTime.Now - started);
        }

        public void UpdateInfo(string title, int count, DateTime startedAt)
        {
            ViewerCountText.Text = count.ToString();
            started = new DateTime(startedAt.Year, startedAt.Month, startedAt.Day, startedAt.Hour+2, startedAt.Minute, startedAt.Second);
        }


        private void GetUptime(TimeSpan time)
        {
            string text = $"{time.Hours:00}:{time.Minutes:00}:{time.Seconds:00}";
            UptimeText.Text = text;
            
        }
        //((int)time.TotalHours), time.Minutes, time.Seconds);

    }
}
