using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Saper.Models.DB;
using Saper.Services;

namespace Saper.ViewModels
{
    public class MenuViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly IWindowService _authWindowService;
        private readonly IWindowService _mainWindowService;
        private readonly ApplicationContext _dataBase;

        private const int MinFieldSize = 10;
        private const int MaxFieldSize = 50;
        private readonly string[] AllowedDifficulties = { "Easy", "Medium", "Hard" };

        #region Fields

        private string _login = string.Empty;
        private string _password = string.Empty;
        private string _fieldSize = "10";
        private string _difficulty = "Hard";
        private string _help = string.Empty;
        private bool _isWin;
        private string _score = string.Empty;
        private TimeSpan _timeSpent;

        #endregion

        #region Properties

        public ObservableCollection<GameResult> GameResultsList { get; } = new();
        public ObservableCollection<bool> Pages { get; } = new() { true, false };
        public ObservableCollection<bool> PageVisibility { get; }

        public string Login
        {
            get => _login;
            set
            {
                if (SetProperty(ref _login, value.Trim()))
                    UpdateAuthCommands();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (SetProperty(ref _password, value.Trim()))
                    UpdateAuthCommands();
            }
        }

        public string FieldSize
        {
            get => _fieldSize;
            set
            {
                if (SetProperty(ref _fieldSize, value))
                    StartCommand?.RaiseCanExecuteChanged();
            }
        }

        public string Difficulty
        {
            get => _difficulty;
            set
            {
                if (SetProperty(ref _difficulty, value))
                    StartCommand?.RaiseCanExecuteChanged();
            }
        }

        public string Help
        {
            get => _help;
            set => SetProperty(ref _help, value);
        }

        public bool IsWin
        {
            get => _isWin;
            set => SetProperty(ref _isWin, value);
        }

        public string Score
        {
            get => _score;
            set => SetProperty(ref _score, value);
        }

        public TimeSpan TimeSpent
        {
            get => _timeSpent;
            set => SetProperty(ref _timeSpent, value);
        }

        #endregion

        #region Commands

        public RelayCommand LogInCommand { get; }
        public RelayCommand RegisterCommand { get; }
        public RelayCommand CloseWindowCommand { get; }
        public RelayCommand GoCommand { get; }
        public RelayCommand StartCommand { get; }

        #endregion

        #region Constructor

        public MenuViewModel(IWindowService authWindowService, IWindowService mainWindowService)
        {
            _authWindowService = authWindowService ?? throw new ArgumentNullException(nameof(authWindowService));
            _mainWindowService = mainWindowService ?? throw new ArgumentNullException(nameof(mainWindowService));
            _dataBase = new ApplicationContext();

            PageVisibility = new ObservableCollection<bool>(Enumerable.Repeat(false, 6));
            Mediator.PageId = 0;
            PageVisibility[0] = true;

            LogInCommand = new RelayCommand(_ => ExecuteLogIn(), _ => CanAuthenticate());
            RegisterCommand = new RelayCommand(_ => ExecuteRegister(), _ => CanAuthenticate());
            CloseWindowCommand = new RelayCommand(_ => Navigate(0));
            GoCommand = new RelayCommand(param => NavigateFromParam(param), _ => true);
            StartCommand = new RelayCommand(_ => ExecuteStart(), _ => CanStartGame());

            Login = Mediator.Login;

            if (!string.IsNullOrEmpty(Login))
                LoadGameResults();
        }

        #endregion

        #region Private Methods

        private void LoadGameResults()
        {
            GameResultsList.Clear();
            var results = _dataBase.GameResults.Where(r => r.UserId == Mediator.UserId);
            foreach (var r in results)
                GameResultsList.Add(r);

            OnPropertyChanged(nameof(GameResultsList));
        }

        private bool CanAuthenticate() =>
            !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password);

        private void ExecuteLogIn()
        {
            var user = _dataBase.Users.FirstOrDefault(u => u.Login == Login && u.Password == Password);
            if (user == null)
            {
                Help = "Такого акаунту не існує";
                return;
            }

            Mediator.Login = user.Login;
            Mediator.UserId = user.Id;
            Mediator.IsMuted = user.IsSoundMuted;

            Help = string.Empty;
            Navigate(2);
            LoadGameResults();
        }

        private void ExecuteRegister()
        {
            if (_dataBase.Users.Any(u => u.Login == Login))
            {
                Help = "Ім'я зайняте, оберіть інше";
                return;
            }

            var newUser = new User
            {
                Login = Login,
                Password = Password,
                IsSoundMuted = false
            };

            _dataBase.Users.Add(newUser);
            _dataBase.SaveChanges();

            Mediator.Login = newUser.Login;
            Mediator.UserId = newUser.Id;
            Mediator.IsMuted = false;

            Help = string.Empty;
            Navigate(2);
            GameResultsList.Clear();
        }

        private bool CanStartGame()
        {
            return int.TryParse(FieldSize, out var size)
                && size >= MinFieldSize
                && size <= MaxFieldSize
                && AllowedDifficulties.Contains(Difficulty);
        }

        private void ExecuteStart()
        {
            if (int.TryParse(FieldSize, out var size))
            {
                Mediator.Rows = size;
                Mediator.Columns = size;
                Mediator.Difficulty = Difficulty;
                _mainWindowService.OpenMainWindow();
            }
        }

        private void NavigateFromParam(object? param)
        {
            if (int.TryParse(param?.ToString(), out int idx))
                Navigate(idx);
        }

        private void Navigate(int idx)
        {
            Mediator.PageId = idx;

            for (int i = 0; i < PageVisibility.Count; i++)
                PageVisibility[i] = (i == idx);

            OnPropertyChanged(nameof(PageVisibility));
            Help = string.Empty;

            if (idx == 0 || idx == 1)
                ClearCredentials();
            else if (idx == 2)
                UpdatePages(0);
        }

        private void ClearCredentials()
        {
            Login = string.Empty;
            Password = string.Empty;
        }

        private void UpdatePages(int activeIndex)
        {
            for (int i = 0; i < Pages.Count; i++)
                Pages[i] = (i == activeIndex);

            OnPropertyChanged(nameof(Pages));
        }

        private void UpdateAuthCommands()
        {
            LogInCommand.RaiseCanExecuteChanged();
            RegisterCommand.RaiseCanExecuteChanged();
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propName);
            return true;
        }

        protected void OnPropertyChanged(string? name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        #endregion
    }
}