using Saper.Models;
using Saper.Services;
using Saper.ViewModels;
using System.Windows;

namespace Saper
{
    public partial class MainWindow : Window
    {
        private readonly IWindowService windowService;  // зробити поле класу

        public MainWindow()
        {
            InitializeComponent();

            // Ініціалізуємо поле
            windowService = new WindowService();

            var gameManager = new GameManager();
            var databaseService = new GameDatabaseService();
            var menuStateService = new MenuStateService();

            var gameConfiguration = new GameConfiguration
            {
                Login = GetLoginFromPreviousWindow(),
                UserId = GetUserIdFromPreviousWindow(),
                Rows = GetRowsFromPreviousWindow(),
                Columns = GetColumnsFromPreviousWindow(),
                Difficulty = GetDifficultyFromPreviousWindow(),
                IsMuted = GetMutedStateFromPreviousWindow()
            };

            // Передаємо windowService у ViewModel
            var gameViewModel = new GameViewModel(
                gameManager,
                windowService,
                windowService,      // Залишаємо так, як було
                databaseService,
                menuStateService,
                gameConfiguration);

            DataContext = gameViewModel;
        }

        private string GetLoginFromPreviousWindow() => Mediator.Login;
        private int GetUserIdFromPreviousWindow() => Mediator.UserId;
        private int GetRowsFromPreviousWindow() => Mediator.Rows;
        private int GetColumnsFromPreviousWindow() => Mediator.Columns;
        private string GetDifficultyFromPreviousWindow() => Mediator.Difficulty;
        private bool GetMutedStateFromPreviousWindow() => Mediator.IsMuted;

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            windowService.OpenMenuWindow();

        }
    }
}
