using ComicBookGalleryModel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            Database.SetInitializer(new DropCreateDatabaseAlways<Context>());
        }

        public DbSet<ComicBook> ComicBooks { get; set; }
        public DbSet<Series> Series { get; set; }
    }
}
