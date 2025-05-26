using Saper.Models;
using Saper.Services;
using Saper.ViewModels;
using System.Windows;

namespace Saper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Створення сервісів
            var windowService = new WindowService();
            var gameManager = new GameManager();
            var databaseService = new GameDatabaseService();
            var menuStateService = new MenuStateService();

            // Створення конфігурації гри (замість використання статичного Mediator)
            var gameConfiguration = new GameConfiguration
            {
                Login = GetLoginFromPreviousWindow(), // Потрібно передати з попереднього вікна
                UserId = GetUserIdFromPreviousWindow(), // Потрібно передати з попереднього вікна
                Rows = GetRowsFromPreviousWindow(), // Потрібно передати з попереднього вікна
                Columns = GetColumnsFromPreviousWindow(), // Потрібно передати з попереднього вікна
                Difficulty = GetDifficultyFromPreviousWindow(), // Потрібно передати з попереднього вікна
                IsMuted = GetMutedStateFromPreviousWindow() // Потрібно передати з попереднього вікна
            };

            // Створення ViewModel з усіма залежностями
            var gameViewModel = new GameViewModel(
                gameManager,
                windowService,
                databaseService,
                menuStateService,
                gameConfiguration);

            DataContext = gameViewModel;
        }

        // Методи для отримання даних з попереднього вікна
        // Ці методи потрібно реалізувати залежно від того, як ви передаєте дані між вікнами
        private string GetLoginFromPreviousWindow()
        {
            // Тут може бути логіка отримання логіну з конструктора або з параметрів
            return Mediator.Login; // Тимчасово, поки не реалізуєте передачу параметрів
        }

        private int GetUserIdFromPreviousWindow()
        {
            return Mediator.UserId; // Тимчасово
        }

        private int GetRowsFromPreviousWindow()
        {
            return Mediator.Rows; // Тимчасово
        }

        private int GetColumnsFromPreviousWindow()
        {
            return Mediator.Columns; // Тимчасово
        }

        private string GetDifficultyFromPreviousWindow()
        {
            return Mediator.Difficulty; // Тимчасово
        }

        private bool GetMutedStateFromPreviousWindow()
        {
            return Mediator.IsMuted; // Тимчасово
        }
    }
}