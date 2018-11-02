using ComicBookGalleryModel.Models;
using System;
using System.Data.Entity;

namespace ComicBookGalleryModel.Data
{
    /// <summary>
    /// Custom database initializer class used to populate
    /// the database with seed data.
    /// </summary>
    internal class DatabaseInitializer : DropCreateDatabaseIfModelChanges<Context>
    {
    }
}
