using ComicBookShared.Models;
using System;
using System.Data.Entity;

namespace ComicBookShared.Data
{
    /// <summary>
    /// Custom database initializer class used to populate
    /// the database with seed data.
    /// </summary>
    internal class DatabaseInitializer : DropCreateDatabaseIfModelChanges<Context>
    {
    }
}
