using Saper.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Saper.Services
{
    public class MainWindowService : IWindowService
    {
        public void OpenWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            Window window;
            if (currentWindow is MenuWindow)
            {
                window = new MainWindow();
            }
            else
            {
                window = new MenuWindow();
            }

            window.Show();
            currentWindow.Close();
        }
        public void CloseWindow()
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

            if (window != null)
            {
                window.Close();
            }
        }
    }

}
