using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper.Models.DB
{
    public class MenuState
    {
        public bool MenuVisibility { get; set; }
        public bool MainMenuVisible { get; set; } = true;
        public bool SettingsVisible { get; set; } = false;

        public void ShowMainMenu()
        {
            MenuVisibility = true;
            MainMenuVisible = true;
            SettingsVisible = false;
        }

        public void HideMenu()
        {
            MenuVisibility = false;
        }

        public void ShowSettings()
        {
            MainMenuVisible = false;
            SettingsVisible = true;
        }

        public void ToggleMenu()
        {
            if (MenuVisibility && MainMenuVisible)
            {
                HideMenu();
            }
            else
            {
                ShowMainMenu();
            }
        }
    }
}
