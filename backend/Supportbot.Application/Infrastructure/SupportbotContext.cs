//using Bogus;
using Microsoft.EntityFrameworkCore;
using Supportbot.Application.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using BCrypt.Net;

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
    var employees = new List<Employee>
    {
        new Employee
        {
            Guid = Guid.NewGuid(),
            Username = "aksoy",
            Firstname = "Can",
            Lastname = "Aksoy",
            Birth = new DateTime(2002, 1, 25),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("1234")
        },
        new Employee
        {
            Guid = Guid.NewGuid(),
            Username = "flach",
            Firstname = "Julian",
            Lastname = "Flach",
            Birth = new DateTime(2002, 2, 27),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("1234")
        },
        new Employee
        {
            Guid = Guid.NewGuid(),
            Username = "posavec",
            Firstname = "Luka",
            Lastname = "Posavec",
            Birth = new DateTime(1995, 3, 3),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("1234")
        },
        new Employee
        {
            Guid = Guid.NewGuid(),
            Username = "birkel",
            Firstname = "Noah",
            Lastname = "Birkel",
            Birth = new DateTime(1995, 3, 3),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("1234")
        },
        // Weitere Mitarbeiter können hier hinzugefügt werden
    };

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
