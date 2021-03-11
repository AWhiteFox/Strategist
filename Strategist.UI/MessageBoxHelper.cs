using System.Windows;

namespace Strategist.UI
{
    internal static class MessageBoxHelper
    {
        public static void Info(string caption, string content) => MessageBox.Show(content, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        
        public static void Error(string content) => MessageBox.Show(content, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}