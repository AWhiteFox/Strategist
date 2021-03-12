using System;
using System.Windows;

namespace Strategist.UI
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App
    {
        private void OnExit(object sender, ExitEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
    }
}
