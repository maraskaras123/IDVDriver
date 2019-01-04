using IDVDriver.Domain;
using Microsoft.EntityFrameworkCore;

namespace IDVDriver.Utils
{
    public class IDVContext : DbContext
    {
        public DbSet<Income> Incomes { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<User> Users { get; set; }

        public string ConnectionString { get; set; }

        public IDVContext(string connString)
        {
            ConnectionString = connString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
    }
}