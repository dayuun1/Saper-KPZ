using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper.Models.DB
{
    public class GameResult
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public bool IsWin { get; set; }
        public TimeSpan TimeSpent { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
