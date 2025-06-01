using System.Linq;
using System.Windows;
using Saper.Services;
using Saper;
using Saper.Views;

public class WindowService : IWindowService
{
    public void OpenMainWindow()
    {
        var window = new MainWindow();
        window.Show();
        Application.Current.MainWindow = window;

        CloseCurrentWindow();
    }

    public void OpenMenuWindow()
    {
        var window = new MenuWindow();
        window.Show();
        Application.Current.MainWindow = window;

        CloseCurrentWindow();
    }

    public void CloseCurrentWindow()
    {
        var currentWindow = Application.Current.Windows
            .OfType<Window>()
            .FirstOrDefault(w => w.IsActive);

        if (currentWindow != null && currentWindow != Application.Current.MainWindow)
        {
            currentWindow.Close();
        }
    }
}
