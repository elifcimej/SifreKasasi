using Microsoft.EntityFrameworkCore;
using SifreKasasi.Data.Models;

namespace SifreKasasi.Data
{
    public class PasswordVaultDbContext : DbContext
    {
        public PasswordVaultDbContext(DbContextOptions<PasswordVaultDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AccountData> Accounts { get; set; }

    }
}
