using System.Linq;
using System.Windows;
using Saper.Views;

namespace Saper.Services
{
    public class WindowService : IWindowService
    {
        public void OpenMainWindow()
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }

        public void OpenMenuWindow()
        {
            var menuWindow = new MenuWindow();
            menuWindow.Show();
        }

        public void CloseCurrentWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            currentWindow?.Close();
        }
    }
}
