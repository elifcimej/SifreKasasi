using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SifreKasasi.Data
{
    public class PasswordVaultDbContextFactory : IDesignTimeDbContextFactory<PasswordVaultDbContext>
    {
        public PasswordVaultDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PasswordVaultDbContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-N9JSA6E\\SQLEXPRESS;Database=SifreKasasiDb;Trusted_Connection=True;TrustServerCertificate=True");

            return new PasswordVaultDbContext(optionsBuilder.Options);
        }
    }
}
