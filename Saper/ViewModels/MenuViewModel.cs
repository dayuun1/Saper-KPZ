using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saper.Services;
using Saper.Models.DB;

namespace Saper.ViewModels
{
    class MenuViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private IWindowService _windowService;
        private IWindowService _mainWindowService;
        private ApplicationContext _dataBase;

        public RelayCommand StartCommand { get; set; }
        public RelayCommand CloseWindowCommand { get; set; }
        public RelayCommand GoCommand { get; set; }
        public RelayCommand LogInCommand { get; set; }
        public RelayCommand RegisterCommand { get; set; }
        public RelayCommand NextPageCommand { get; set; }
        public RelayCommand PreviousPageCommand { get; set; }

        private string _help;
        public string Help
        {
            get => _help;
            set
            {
                _help = value;
                OnPropertyChanged(nameof(Help));
            }
        }

        private string _login = "";
        public string Login
        {
            get => _login;
            set
            {
                if (_login != value)
                {
                    _login = value;
                    OnPropertyChanged(nameof(Login));
                    (LogInCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (RegisterCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        private string _password = "";
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                    (LogInCommand as RelayCommand)?.RaiseCanExecuteChanged();
                    (RegisterCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }
        private string _fieldSize = "10";
        public string FieldSize
        {
            get => _fieldSize;
            set
            {
                _fieldSize = value;
                OnPropertyChanged(nameof(FieldSize));
                (StartCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        private string _difficulty = "Hard";
        public string Difficulty
        {
            get => _difficulty;
            set
            {
                _difficulty = value;
                OnPropertyChanged(nameof(Difficulty));
                (StartCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        private bool _isMuted;
        public bool IsMuted
        {
            get => _isMuted;
            set
            {
                if (_isMuted)
                {
                    MutedIcon = "../Images/notmuted.jpg";
                }
                else
                {
                    MutedIcon = "../Images/muted.jpg";
                }
                _isMuted = value;
                OnPropertyChanged(nameof(IsMuted));
            }
        }

        private string _mutedIcon = "../Images/notmuted";
        public string MutedIcon
        {
            get => _mutedIcon;
            set
            {
                _mutedIcon = value;
                OnPropertyChanged(nameof(MutedIcon));
            }
        }
        
        private int _pageNum;
        public List<GameResult> GameResultsList { get; set; } = new();
        public bool[] Pages { get; set; }
        public bool[] PageVisibility { get; set; }

        public MenuViewModel(IWindowService windowService, IWindowService mainWindowService)
        {
            _pageNum = 0;
            Pages = [true, false];
            PageVisibility = [false, false, false, false, false, false];
            PageVisibility[Mediator.PageId] = true;
            IsMuted = Mediator.IsMuted;
            Mediator.PageId = 0;
            Login = Mediator.Login;
            GameResultsList = new();


            _windowService = windowService;
            _mainWindowService = mainWindowService;
            _dataBase = new ApplicationContext();

            if (!string.IsNullOrEmpty(Login))
            {
                GameResultsList = _dataBase.GameResults.Where(u => u.UserId == Mediator.UserId).ToList();
            }
            LogInCommand = new RelayCommand(obj => LogInCommandExecuted(), obj => LogAndRegisterCommandCanExecute());
            RegisterCommand = new RelayCommand(obj => RegisterCommandExecuted(), obj => LogAndRegisterCommandCanExecute());
            CloseWindowCommand = new RelayCommand(obj => _windowService.CloseWindow());

            GoCommand = new RelayCommand(obj =>
            {
                int idx = int.Parse((string)obj);
                GoCommandExecuted(idx);
            });

            StartCommand = new RelayCommand(obj =>
            {
                if (int.TryParse(FieldSize, out int size))
                {
                    Mediator.Rows = size;
                    Mediator.Columns = size;
                    Mediator.Difficulty = Difficulty;

                    Mediator.IsMuted = IsMuted;
                    var user = _dataBase.Users.Find(Mediator.UserId);
                    user.IsSoundMuted = IsMuted;
                    _dataBase.SaveChanges();

                    _mainWindowService.OpenWindow(); 
                }
            }, obj => StartCommandCanExecute());
            CloseWindowCommand = new RelayCommand(obj => _windowService.CloseWindow());
        }
            
        public void LogInCommandExecuted()
        {
            User user = _dataBase.Users.FirstOrDefault(u => u.Password == Password.Trim() && u.Login == Login.Trim());
            if (user == null)
            {
                Help = "Такого акаунту не існує";
            }
            else
            {
                PageVisibility[0] = false;
                PageVisibility[2] = true;
                Help = "";
                Mediator.Login = user.Login;
                Mediator.UserId = user.Id;
                Mediator.IsMuted = user.IsSoundMuted;
                IsMuted = user.IsSoundMuted;
                GameResultsList = _dataBase.GameResults.Where(u => u.UserId == Mediator.UserId).ToList();
                OnPropertyChanged(nameof(GameResultsList));
                OnPropertyChanged(nameof(PageVisibility));
            }
        }

        public bool LogAndRegisterCommandCanExecute()
        {
            return !string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password);
        }

        public void RegisterCommandExecuted()
        {
            if (_dataBase.Users.FirstOrDefault(u => u.Login == Login.Trim()) != null)
            {
                Help = "Ім'я зайняте, оберіть інше";
            }
            else
            {
                _dataBase.Users.Add(new User { Login = Login.Trim(), Password = Password.Trim(), IsSoundMuted = false });
                _dataBase.SaveChanges();
                PageVisibility[1] = false;
                PageVisibility[2] = true;
                Help = "";
                GameResultsList = new();
                IsMuted = false;
                Mediator.Login = _dataBase.Users.FirstOrDefault(u => u.Login == Login.Trim()).Login;
                Mediator.IsMuted = false;
                Mediator.UserId = _dataBase.Users.FirstOrDefault(u => u.Login == Login.Trim()).Id;
                OnPropertyChanged(nameof(GameResultsList));
                OnPropertyChanged(nameof(PageVisibility));
            }
        }
        public bool StartCommandCanExecute()
        {
            return int.TryParse(FieldSize, out int size) && size >= 10 && size <= 50 &&
          (Difficulty == "Easy" || Difficulty == "Medium" || Difficulty == "Hard");
        }
        public void GoCommandExecuted(int idx)
        {
            for (int i = 0; i < PageVisibility.Length; i++)
            {
                PageVisibility[i] = false;
            }
            PageVisibility[idx] = true;
            Help = "";
            if (idx == 0 || idx == 1)
            {
                Login = "";
                Password = "";
            }
            else if (idx == 2)
            {
                Pages[_pageNum] = false;
                _pageNum = 0;
                Pages[_pageNum] = true;
                OnPropertyChanged(nameof(Pages));
            }
            OnPropertyChanged(nameof(PageVisibility));
        }
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
