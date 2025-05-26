using Saper.Models.DB;
using System.Collections.Generic;
using System.Linq;

namespace Saper.Services
{
    public interface IGameDatabaseService
    {
        void SaveGameResult(GameResult gameResult);
        void SaveUserSession(int userId);
        List<GameResult> GetUserGameResults(int userId);
        User GetUser(int userId);
    }

    public class GameDatabaseService : IGameDatabaseService
    {
        private readonly ApplicationContext _database;

        public GameDatabaseService()
        {
            _database = new ApplicationContext();
        }

        public void SaveGameResult(GameResult gameResult)
        {
            _database.GameResults.Add(gameResult);
            _database.SaveChanges();
        }

        public void SaveUserSession(int userId)
        {
            var user = _database.Users.Find(userId);
            if (user != null)
            {
                _database.SaveChanges();
            }
        }

        public List<GameResult> GetUserGameResults(int userId)
        {
            return _database.GameResults.Where(u => u.UserId == userId).ToList();
        }

        public User GetUser(int userId)
        {
            return _database.Users.Find(userId);
        }

        public void Dispose()
        {
            _database?.Dispose();
        }
    }
}