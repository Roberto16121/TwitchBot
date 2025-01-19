using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (started == DateTime.MinValue)
                return;
            GetUptime(DateTime.Now - started);
        }

        public void UpdateInfo(string title, int count, DateTime startedAt)
        {
            TitleText.Text = title;
            ViewerCountText.Text = count.ToString();
            started = startedAt;
        }


        private void GetUptime(TimeSpan time) =>
            UptimeText.Text = string.Format("Inca nu merge");
           //((int)time.TotalHours), time.Minutes, time.Seconds);

    }
}
