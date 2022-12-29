using GuessTheGameApi.Converter.Models.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace GuessTheGameApi.DataAccess.Context
{
    public class GuessTheGameDBContext : DbContext
    {
        public GuessTheGameDBContext(DbContextOptions<GuessTheGameDBContext> options)
            : base(options){ }

        public DbSet<Game> Game { get; set; }
        public DbSet<CurrentLevel> CurrentLevel { get; set; }
        public DbSet<Credentials> Credentials { get; set; }
    }
}
