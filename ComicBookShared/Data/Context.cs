using ComicBookShared.Migrations;
using ComicBookShared.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ComicBookShared.Data
{
    /// <summary>
    /// Entity Framework context class.
    /// </summary>
    public class Context : DbContext
    {
        public DbSet<ComicBook> ComicBooks { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Role> Roles { get; set; }

        public Context()
        {
            // This call to the SetInitializer method is used 
            // to configure EF to use our custom database initializer class
            // which contains our app's database seed data.
            // Database.SetInitializer(new DatabaseInitializer());

#if DEBUG
            // This disabled database initialization within EF. This is used
            // to enable migrations.
            Database.SetInitializer<Context>(null);
#else
            // When in production mode, our initializer will use MigrateDatabaseToLatestVersion
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Configuration>());
#endif
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Removing the pluralizing table name convention 
            // so our table names will use our entity class singular names.
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Using the fluent API to configure the precision and scale
            // for the ComicBook.AverageRating property.
            modelBuilder.Entity<ComicBookAverageRating>()
                .Property(cb => cb.AverageRating)
                .HasPrecision(5, 2);
        }
    }
}
