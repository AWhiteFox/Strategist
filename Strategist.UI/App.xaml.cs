using System;
using System.Windows;

namespace Strategist.UI
{
    public partial class App
    {
        private void OnExit(object sender, ExitEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
    }
}
