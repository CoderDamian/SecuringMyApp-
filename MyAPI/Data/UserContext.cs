using Microsoft.EntityFrameworkCore;
using MyAPI.Mappings;
using MyAPI.Models;
using Oracle.ManagedDataAccess.Client;

namespace MyAPI.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {

        }

        public DbSet<LoginModel> LoginModels { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder dbContext)
        {
            if (OracleConfiguration.TnsAdmin is null)
            {
                OracleConfiguration.TnsAdmin = @"C:\Users\Fmla\Documents\OracleWallet\MyERP\";
                OracleConfiguration.WalletLocation = OracleConfiguration.TnsAdmin;
            }
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LoginModelMap());
        }
    }
}
