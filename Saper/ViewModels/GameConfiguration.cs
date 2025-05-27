namespace Saper.Services
{
    public class GameConfiguration
    {
        public string Login { get; set; } = "";
        public bool IsMuted { get; set; } = true;
        public int UserId { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public string Difficulty { get; set; } = "Easy";

        public GameConfiguration()
        {
        }

        public GameConfiguration(string login, bool isMuted, int userId, int rows, int columns, string difficulty)
        {
            Login = login;
            IsMuted = isMuted;
            UserId = userId;
            Rows = rows;
            Columns = columns;
            Difficulty = difficulty;
        }
    }
}