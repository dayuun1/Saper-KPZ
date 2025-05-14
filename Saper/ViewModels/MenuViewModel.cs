using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saper.Services;

namespace Saper.ViewModels
{
    class MenuViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private IWindowService _windowService;
        private IWindowService _windowServiceTwoPlayers;

        public RelayCommand StartCommand { get; set; }
        public RelayCommand CloseWindowCommand { get; set; }
        public RelayCommand GoCommand { get; set; }
        public RelayCommand LogInCommand { get; set; }
        public RelayCommand RegisterCommand { get; set; }
        public RelayCommand NextPageCommand { get; set; }
        public RelayCommand PreviousPageCommand { get; set; }
        public RelayCommand NextRuleCommand { get; set; }
        public RelayCommand PreviousRuleCommand { get; set; }

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
        private string _firstPlayerName;
        public string FirstPlayerName
        {
            get => _firstPlayerName;
            set
            {
                _firstPlayerName = value;
                OnPropertyChanged(nameof(FirstPlayerName));
            }
        }

        private string _secondPlayerName;
        public string SecondPlayerName
        {
            get => _secondPlayerName;
            set
            {
                _secondPlayerName = value;
                OnPropertyChanged(nameof(SecondPlayerName));
            }
        }

        private string _name = "";
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(_name));
            }
        }

        private string _password = "";
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private bool _isMuted = true;
        public bool IsMuted
        {
            get => _isMuted;
            set
            {
                _isMuted = value;
                OnPropertyChanged(nameof(IsMuted));
            }
        }

        private int _pageNum;
        private int _ruleNum;
        public bool[] Pages { get; set; }
        public bool[] RuleVisibility { get; set; }
        public bool[] PageVisibility { get; set; }

        public MenuViewModel(IWindowService windowService, IWindowService mainWindowService)
        {
            FirstPlayerName = "Cartographer1";
            SecondPlayerName = "Cartographer2";
            _pageNum = 0;
            Pages = [true, false, false];
            _ruleNum = 0;
            RuleVisibility = [true, false, false, false];
            PageVisibility = [false, false, false, false, false, false, false, false, false];
            PageVisibility[Mediator.PageId] = true;
            IsMuted = Mediator.IsMuted;
            Mediator.PageId = 0;
            Name = Mediator.Login;

            _windowService = windowService;
            _windowServiceTwoPlayers = mainWindowService;


            StartCommand = new RelayCommand(obj => StartCommandExecuted(), obj => StartCommandCanExecute());
            CloseWindowCommand = new RelayCommand(obj => _windowService.CloseWindow());
            GoCommand = new RelayCommand(obj =>
            {
                int idx = int.Parse((string)obj);
                GoCommandExecuted(idx);
            });
            StartCommand = new RelayCommand(obj =>
            {
                if (int.Parse((string)obj) == 0)
                {
                    _windowService.OpenWindow();
                }
                else
                {
                    _windowServiceTwoPlayers.OpenWindow();
                }
            });
            CloseWindowCommand = new RelayCommand(obj => _windowService.CloseWindow());
            NextPageCommand = new RelayCommand(obj =>
            {
                Pages[_pageNum] = false;
                _pageNum++;
                if (_pageNum >= Pages.Length)
                {
                    _pageNum = 0;
                }
                Pages[_pageNum] = true;
                OnPropertyChanged(nameof(Pages));
            });
            PreviousPageCommand = new RelayCommand(obj =>
            {
                Pages[_pageNum] = false;
                _pageNum--;
                if (_pageNum < 0)
                {
                    _pageNum = Pages.Length - 1;
                }
                Pages[_pageNum] = true;
                OnPropertyChanged(nameof(Pages));
            });
            NextRuleCommand = new RelayCommand(obj =>
            {
                RuleVisibility[_ruleNum] = false;
                _ruleNum++;
                if (_ruleNum >= RuleVisibility.Length)
                {
                    _ruleNum = 0;
                }
                RuleVisibility[_ruleNum] = true;
                OnPropertyChanged(nameof(RuleVisibility));
            });
            PreviousRuleCommand = new RelayCommand(obj =>
            {
                RuleVisibility[_ruleNum] = false;
                _ruleNum--;
                if (_ruleNum < 0)
                {
                    _ruleNum = RuleVisibility.Length - 1;
                }
                RuleVisibility[_ruleNum] = true;
                OnPropertyChanged(nameof(RuleVisibility));
            });
        }


        public bool StartCommandCanExecute()
        {
            return FirstPlayerName.Trim() != string.Empty && SecondPlayerName.Trim() != string.Empty && FirstPlayerName != SecondPlayerName
                && FirstPlayerName.Trim().Length <= 10 && SecondPlayerName.Trim().Length <= 10;
        }
        public void StartCommandExecuted()
        {
            _windowService.OpenWindow();
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
                Name = "";
                Password = "";
            }
            else if (idx == 2)
            {
                FirstPlayerName = "Player1";
                SecondPlayerName = "Player2";
                Pages[_pageNum] = false;
                RuleVisibility[_ruleNum] = false;
                _ruleNum = 0;
                _pageNum = 0;
                Pages[_pageNum] = true;
                RuleVisibility[_ruleNum] = true;
                OnPropertyChanged(nameof(Pages));
                OnPropertyChanged(nameof(RuleVisibility));
            }
            OnPropertyChanged(nameof(PageVisibility));
        }
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
