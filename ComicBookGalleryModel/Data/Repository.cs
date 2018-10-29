using ComicBookGalleryModel.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;

namespace ComicBookGalleryModel.Data
{
    /// <summary>
    /// Repository class that provides various database queries
    /// and CRUD operations.
    /// </summary>
    public static class Repository
    {
        /// <summary>
        /// Private method that returns a database context.
        /// </summary>
        /// <returns>An instance of the Context class.</returns>
        static Context GetContext()
        {
            var context = new Context();
            context.Database.Log = (message) => Debug.WriteLine(message);
            return context;
        }

        /// <summary>
        /// Returns a count of the comic books.
        /// </summary>
        /// <returns>An integer count of the comic books.</returns>
        public static int GetComicBookCount()
        {
            using (Context context = GetContext())
            {
                return context.ComicBooks.Count();
            }
        }

        /// <summary>
        /// Returns a list of comic books ordered by the series title 
        /// and issue number.
        /// </summary>
        /// <returns>An IList collection of ComicBook entity instances.</returns>
        public static IList<ComicBook> GetComicBooks()
        {
            using (Context context = GetContext())
            {
                return context.ComicBooks
                    .Include(cb => cb.Series)
                    .OrderBy(cb => cb.Series.Title)
                    .ThenBy(cb => cb.IssueNumber)
                    .ToList();
            }
        }

        /// <summary>
        /// Returns a single comic book.
        /// </summary>
        /// <param name="comicBookId">The comic book ID to retrieve.</param>
        /// <returns>A fully populated ComicBook entity instance.</returns>
        public static ComicBook GetComicBook(int comicBookId)
        {
            using (Context context = GetContext())
            {
                return context.ComicBooks
                    .Include(cb => cb.Series)
                    .Include(cb => cb.Artists.Select(a => a.Artist))
                    .Include(cb => cb.Artists.Select(a => a.Role))
                    .Where(cb => cb.Id == comicBookId)
                    .SingleOrDefault();
            }
        }

        /// <summary>
        /// Returns a list of series ordered by title.
        /// </summary>
        /// <returns>An IList collection of Series entity instances.</returns>
        public static IList<Series> GetSeries()
        {
            using (Context context = GetContext())
            {
                return context.Series
                    .OrderBy(s => s.Title)
                    .ToList();
            }
        }

        /// <summary>
        /// Returns a single series.
        /// </summary>
        /// <param name="seriesId">The series ID to retrieve.</param>
        /// <returns>A Series entity instance.</returns>
        public static Series GetSeries(int seriesId)
        {
            using (Context context = GetContext())
            {
                return context.Series
                    .Where(s => s.Id == seriesId)
                    .SingleOrDefault();
            }
        }

        /// <summary>
        /// Returns a list of artists ordered by name.
        /// </summary>
        /// <returns>An IList collection of Artist entity instances.</returns>
        public static IList<Artist> GetArtists()
        {
            using (Context context = GetContext())
            {
                return context.Artists
                    .OrderBy(a => a.Name)
                    .ToList();
            }
        }

        /// <summary>
        /// Returns a list of roles ordered by name.
        /// </summary>
        /// <returns>An IList collection of Role entity instances.</returns>
        public static IList<Role> GetRoles()
        {
            using (Context context = GetContext())
            {
                return context.Roles
                    .OrderBy(r => r.Name)
                    .ToList();
            }
        }

        /// <summary>
        /// Adds a comic book.
        /// </summary>
        /// <param name="comicBook">The ComicBook entity instance to add.</param>
        public static void AddComicBook(ComicBook comicBook)
        {
            // the GetContext method above does the same as
            // using (var context = new Context()) but includes debugging.
            using (Context context = GetContext())
            {
                context.ComicBooks.Add(comicBook);

                // this is defensive coding which exists in the rare case that
                // a comic book is added to the database where the series ID is explicitly
                // provided but already exists. This prevents a new series from being created ('Unchanged')
                if (comicBook.Series != null && comicBook.Series.Id > 0)
                {
                    context.Entry(comicBook.Series).State = EntityState.Unchanged;
                }
                // same defensive coding below. We are checking to make sure that the Artist and
                // Role IDs are unchanged in the database if their ID property is explicitly provided.
                foreach (ComicBookArtist artist in comicBook.Artists)
                {
                    if (artist.Artist != null && artist.Artist.Id > 0)
                    {
                        context.Entry(artist.Artist).State = EntityState.Unchanged;
                    }
                    if (artist.Role != null && artist.Role.Id > 0)
                    {
                        context.Entry(artist.Role).State = EntityState.Unchanged;
                    }
                }

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Updates a comic book.
        /// </summary>
        /// <param name="comicBook">The ComicBook entity instance to update.</param>
        public static void UpdateComicBook(ComicBook comicBook)
        {
            using (Context context = GetContext())
            {
                // Attach() will attach the passed in values to the matching pk record in the database
                context.ComicBooks.Attach(comicBook);

                // EF has nothing to compare the new values to since the record is not being tracked
                // so we have to set its state to modified to force EF to update the record.
                var comicBookEntry = context.Entry(comicBook);
                comicBookEntry.State = EntityState.Modified;

                // this optional line lets us manually choose a property we don't want to let the user change
                // comicBookEntry.Property("IssueNumber").IsModified = false;

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Deletes a comic book.
        /// </summary>
        /// <param name="comicBookId">The comic book ID to delete.</param>
        public static void DeleteComicBook(int comicBookId)
        {
            using (Context context = GetContext())
            {
                // we create a 'stub' record with the same Id as the record we'd like to delete
                var comicBook = new ComicBook() { Id = comicBookId };

                // we set the state of this entry to deleted
                // NOTE: This will also delete associated relationships in the ComicBookArtist
                // table because WillCascadeOnDelete is enabled by default. See https://teamtreehouse.com/library/deleting-entities
                // for info on how to override this.
                context.Entry(comicBook).State = EntityState.Deleted;

                context.SaveChanges();
            }
        }
    }
}
