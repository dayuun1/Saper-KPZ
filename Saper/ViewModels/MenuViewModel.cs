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

        private readonly IWindowService _windowService;
        private readonly IWindowService _mainWindowService;
        private readonly ApplicationContext _dataBase;
        public RelayCommand LogInCommand { get; }
        public RelayCommand RegisterCommand { get; }
        public RelayCommand CloseWindowCommand { get; }
        public RelayCommand GoCommand { get; }
        public RelayCommand StartCommand { get; }
        public ObservableCollection<GameResult> GameResultsList { get; } = new();
        public ObservableCollection<bool> Pages { get; } = new ObservableCollection<bool> { true, false };
        public ObservableCollection<bool> PageVisibility { get; }
        private string _help = string.Empty;
        public string Help
        {
            get => _help;
            set => SetProperty(ref _help, value);
        }

        private string _login = string.Empty;
        public string Login
        {
            get => _login;
            set
            {
                if (SetProperty(ref _login, value.Trim()))
                    LogInCommand.RaiseCanExecuteChanged();
            }
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set
            {
                if (SetProperty(ref _password, value.Trim()))
                    LogInCommand.RaiseCanExecuteChanged();
            }
        }

        private string _fieldSize = "10";
        public string FieldSize
        {
            get => _fieldSize;
            set
            {
                if (SetProperty(ref _fieldSize, value))
                    StartCommand.RaiseCanExecuteChanged();
            }
        }

        private string _difficulty = "Hard";
        public string Difficulty
        {
            get => _difficulty;
            set
            {
                if (SetProperty(ref _difficulty, value))
                    StartCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _isWin;
        public bool IsWin
        {
            get => _isWin;
            set => SetProperty(ref _isWin, value);
        }

        private string _score = string.Empty;
        public string Score
        {
            get => _score;
            set => SetProperty(ref _score, value);
        }

        private TimeSpan _timeSpent;
        public TimeSpan TimeSpent
        {
            get => _timeSpent;
            set => SetProperty(ref _timeSpent, value);
        }

        public MenuViewModel(IWindowService windowService, IWindowService mainWindowService)
        {
            _windowService = windowService;
            _mainWindowService = mainWindowService;
            _dataBase = new ApplicationContext();
            PageVisibility = new ObservableCollection<bool>(Enumerable.Repeat(false, 6));
            Mediator.PageId = 0;
            PageVisibility[0] = true;
            Login = Mediator.Login;
            if (!string.IsNullOrEmpty(Login))
                LoadGameResults();
            LogInCommand = new RelayCommand(_ => ExecuteLogIn(), _ => CanAuthenticate());
            RegisterCommand = new RelayCommand(_ => ExecuteRegister(), _ => CanAuthenticate());
            CloseWindowCommand = new RelayCommand(_ => _windowService.CloseWindow());
            GoCommand = new RelayCommand(param =>
            {
                if (int.TryParse(param?.ToString(), out int idx))
                {
                    Navigate(idx);
                }
            }, _ => true);
            StartCommand = new RelayCommand(_ => ExecuteStart(), _ => CanStartGame());
        }

        private void LoadGameResults()
        {
            GameResultsList.Clear();
            var results = _dataBase.GameResults.Where(r => r.UserId == Mediator.UserId);
            foreach (var r in results)
                GameResultsList.Add(r);
            OnPropertyChanged(nameof(GameResultsList));
        }

        private bool CanAuthenticate()
            => !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password);

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
            _dataBase.SaveChanges();

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
            var newUser = new User { Login = Login, Password = Password, IsSoundMuted = false };
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
            => int.TryParse(FieldSize, out var size)
               && size >= 10 && size <= 50
               && new[] { "Easy", "Medium", "Hard" }.Contains(Difficulty);

        private void ExecuteStart()
        {
            if (int.TryParse(FieldSize, out var size))
            {
                Mediator.Rows = size;
                Mediator.Columns = size;
                Mediator.Difficulty = Difficulty;
                _dataBase.SaveChanges();
                _mainWindowService.OpenWindow();
            }
        }

        private void Navigate(int idx)
        {
            Mediator.PageId = idx;
            for (int i = 0; i < PageVisibility.Count; i++)
                PageVisibility[i] = i == idx;
            OnPropertyChanged(nameof(PageVisibility));
            Help = string.Empty;

            if (idx == 0 || idx == 1)
            {
                Login = string.Empty;
                Password = string.Empty;
            }
            else if (idx == 2)
            {
                Pages[0] = true;
                Pages[1] = false;
                OnPropertyChanged(nameof(Pages));
            }
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propName);
            return true;
        }

        protected void OnPropertyChanged(string? name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}