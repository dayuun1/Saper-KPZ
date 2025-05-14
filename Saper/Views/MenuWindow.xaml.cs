using Saper.Services;
using Saper.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Saper.Views
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        private MediaPlayer _mediaPlayer;
        public MenuWindow()
        {
            var windowService = new WindowService();
            var mainWindowService = new MainWindowService();
            DataContext = new MenuViewModel(windowService, mainWindowService);

            string executableFilePath = Assembly.GetExecutingAssembly().Location;
            string executableDirectoryPath = System.IO.Path.GetDirectoryName(executableFilePath);
            string audioFilePath = System.IO.Path.Combine(executableDirectoryPath, "Music/videoplayback.m4a");

            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.MediaEnded += Media_Ended;
            _mediaPlayer.Close();
            _mediaPlayer.Open(new Uri(audioFilePath));
            _mediaPlayer.Volume = 0.10;

            InitializeComponent();

            _mediaPlayer.Play();
        }
        private void Media_Ended(object sender, EventArgs e)
        {
            _mediaPlayer.Position = TimeSpan.FromMilliseconds(1);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _mediaPlayer.Close();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _mediaPlayer.IsMuted = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            _mediaPlayer.IsMuted = false;
        }

    }
}
