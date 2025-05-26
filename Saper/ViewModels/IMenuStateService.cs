namespace Saper.Services
{
    public interface IMenuStateService
    {
        bool MenuVisibility { get; }
        bool MainMenuVisible { get; }
        bool SettingsVisible { get; }

        void ToggleMenu();
        void ShowSettings();
        void HideMenu();
    }

    public class MenuStateService : IMenuStateService
    {
        public bool MenuVisibility { get; private set; }
        public bool MainMenuVisible { get; private set; }
        public bool SettingsVisible { get; private set; }

        public MenuStateService()
        {
            MenuVisibility = false;
            MainMenuVisible = true;
            SettingsVisible = false;
        }

        public void ToggleMenu()
        {
            MenuVisibility = !MenuVisibility;
            if (MenuVisibility)
            {
                MainMenuVisible = true;
                SettingsVisible = false;
            }
        }

        public void ShowSettings()
        {
            MenuVisibility = true;
            MainMenuVisible = false;
            SettingsVisible = true;
        }

        public void HideMenu()
        {
            MenuVisibility = false;
            MainMenuVisible = true;
            SettingsVisible = false;
        }
    }
}