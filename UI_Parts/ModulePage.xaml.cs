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

namespace TwitchBot.UI_Parts
{
    /// <summary>
    /// Interaction logic for ModulePage.xaml
    /// </summary>
    public partial class ModulePage : Page
    {
        MainWindow window;
        public ModulePage(MainWindow window)
        {
            InitializeComponent();
            this.window = window;
        }
        public void PageIsClosing(object sender, EventArgs e)
        {
            window.ClosePage(this);
        }

            
    }
}
