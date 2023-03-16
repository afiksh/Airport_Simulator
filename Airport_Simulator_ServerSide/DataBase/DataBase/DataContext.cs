#region using
using Models.Models;
using Microsoft.EntityFrameworkCore;
#endregion

namespace DataBase
{
    public class DataContext : DbContext
    {
        #region Tabels
        public virtual DbSet<Airplane> Airplanes { get; set; }
        public virtual DbSet<Leg> Legs { get; set; }
        public virtual DbSet<Logger> Loggers { get; set; }
        #endregion

        #region Consturctors
        public DataContext() : base() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        #endregion

        #region Methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MyAirportDB");
        }
        #endregion
    }
}



