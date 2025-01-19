using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows;

namespace TwitchBot
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, eventArgs) =>
            {
                var assemblyName = new AssemblyName(eventArgs.Name).Name + ".dll";
                var libPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lib", assemblyName);
                return File.Exists(libPath) ? Assembly.LoadFrom(libPath) : null;
            };
        }
    }

}
