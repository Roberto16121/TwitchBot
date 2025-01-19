using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TwitchBot.UI_Parts
{
    /// <summary>
    /// Interaction logic for CurrentViewer.xaml
    /// </summary>
    public partial class CurrentViewer : UserControl
    {
        public CurrentViewer()
        {
            InitializeComponent();
        }
        public static readonly DependencyProperty UserProperty =
            DependencyProperty.Register(nameof(ActiveViewer), typeof(ActiveViewer), typeof(CurrentViewer), new PropertyMetadata(null, OnActiveViewerChanged));

        public CurrentViewer ActiveViewer
        {
            get => (CurrentViewer)GetValue(UserProperty);
            set => SetValue(UserProperty, value);
        }

        private static void OnActiveViewerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CurrentViewer control && e.NewValue is ActiveViewer viewer)
            {
                control.UsernameText.Text = viewer.username;
                
                control.UsernameText.Foreground = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString(viewer.userColor == "" ? "#FF0000" : viewer.userColor));
                
                switch(viewer.viewerType)
                {
                    case ViewerType.Broadcaster:
                    case ViewerType.Moderator:
                        {
                            var uri = new Uri(@"/Images/Moderator.png", UriKind.Relative);
                            control.Icon.Source = new BitmapImage(uriSource: uri);
                        }break;
                    case ViewerType.Subscriber:
                        {
                            var uri = new Uri(@"/Images/SUB.png", UriKind.Relative);
                            control.Icon.Source = new BitmapImage(uriSource: uri);
                        }break;
                    case ViewerType.VIP:
                        {
                            var uri = new Uri(@"/Images/VIP.png", UriKind.Relative);
                            control.Icon.Source = new BitmapImage(uriSource: uri);
                        }break;
                }

            }
        }
    }
}
