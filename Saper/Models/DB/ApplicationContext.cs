using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Saper.Models.DB
{
    public class ApplicationContext : DbContext
    {
        private const string _connectionString = "Data Source=Saper1.db";

        public DbSet<User> Users => Set<User>();
        public DbSet<GameResult> GameResults => Set<GameResult>();
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connectionString);
        }
    }
}
