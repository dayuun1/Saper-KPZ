using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saper.ViewModels
{
    public static class Mediator
    {
        public static string Login { get; set; } = "";
        public static bool IsMuted { get; set; } = true;
        public static int UserId { get; set; }
        public static int PageId { get; set; }
        public static int Rows { get; set; }
        public static int Columns { get; set; }
        public static string Difficulty { get; set; } = "Easy";
    }
}
