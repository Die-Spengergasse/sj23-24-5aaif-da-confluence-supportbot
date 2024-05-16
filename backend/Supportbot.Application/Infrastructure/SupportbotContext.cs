using Bogus;
using Microsoft.EntityFrameworkCore;
using Supportbot.Application.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Supportbot.Application.Infrastructure
{
    public class SupportbotContext : DbContext
    {
        public DbSet<Employee> Employees => Set<Employee>();
        public SupportbotContext(DbContextOptions<SupportbotContext> opt) : base(opt)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Generic config for all entities
                // ON DELETE RESTRICT instead of ON DELETE CASCADE
                foreach (var key in entityType.GetForeignKeys())
                    key.DeleteBehavior = DeleteBehavior.Restrict;

                foreach (var prop in entityType.GetDeclaredProperties())
                {
                    // Define Guid as alternate key. The database can create a guid fou you.
                    if (prop.Name == "Guid")
                    {
                        modelBuilder.Entity(entityType.ClrType).HasAlternateKey("Guid");
                        prop.ValueGenerated = Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAdd;
                    }
                    // Default MaxLength of string Properties is 255.
                    if (prop.ClrType == typeof(string) && prop.GetMaxLength() is null) prop.SetMaxLength(255);
                    // Seconds with 3 fractional digits.
                    if (prop.ClrType == typeof(DateTime)) prop.SetPrecision(3);
                    if (prop.ClrType == typeof(DateTime?)) prop.SetPrecision(3);
                }
            }
        }

        /// <summary>
        /// Initialize the database with some values (holidays, ...).
        /// Unlike Seed, this method is also called in production.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void Initialize()
        {
        }

        /// <summary>
        /// Generates random values for testing the application. This method is only called in development mode.
        /// </summary>
        private async Task SeedAsync()
        {
            Randomizer.Seed = new Random(1213);
            var faker = new Faker("de");

            var employees = new Faker<Employee>("de").CustomInstantiator(f =>
            {
                var lastname = f.Name.LastName();
                return new Employee(
                    username: lastname.ToLower(),
                    firstname: f.Name.FirstName(), lastname: lastname,
                    birth: f.Date.Between(new DateTime(1990, 1, 1), new DateTime(2005, 1, 1)).Date)
                { Guid = f.Random.Guid() };
            })
            .Generate(10)
            .ToList();
            Employees.AddRange(employees);
            await SaveChangesAsync();
        }

        /// <summary>
        /// Creates the database. Called once at application startup.
        /// </summary>
        public async Task CreateDatabaseAsync(bool isDevelopment)
        {
            if (isDevelopment) { Database.EnsureDeleted(); }
            // EnsureCreated only creates the model if the database does not exist or it has no
            // tables. Returns true if the schema was created.  Returns false if there are
            // existing tables in the database. This avoids initializing multiple times.
            if (Database.EnsureCreated()) { Initialize(); }
            if (isDevelopment) await SeedAsync();
        }

    }
}
