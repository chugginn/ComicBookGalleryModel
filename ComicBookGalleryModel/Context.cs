using ComicBookGalleryModel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicBookGalleryModel
{ 
    public class Context : DbContext
    {
        public Context()
        {
            // Making changes to the model causes our app to be out of sync with our database
            // To fix this, we can try Code First Migrations (to be learned later), or we
            // can drop the database and create a new one every time the model changes, using
            // the code below.
            //Database.SetInitializer(new DropCreateDatabaseIfModelChanges<Context>());
            //Database.SetInitializer(new CreateDatabaseIfNotExists<Context>());
            //Database.SetInitializer(new DropCreateDatabaseAlways<Context>());
            Database.SetInitializer(new DatabaseInitializer());
        }

        public DbSet<ComicBook> ComicBooks { get; set; }

        // EF lets us override certain methods. Type 'override' and space to let
        // intellisense show you the methods.
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // this line will remove the convention of pluralizing table names automatically
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // these lines would replace the decimal property convention across all entities
            //modelBuilder.Conventions.Remove<DecimalPropertyConvention>();
            //modelBuilder.Conventions.Add(new DecimalPropertyConvention(5, 2));

            // this line replaces the decimal property precision to the AverageRating
            // property on the ComicBook entity only
            modelBuilder.Entity<ComicBook>()
                .Property(cb => cb.AverageRating)
                .HasPrecision(5, 2);
        }
    }
}
