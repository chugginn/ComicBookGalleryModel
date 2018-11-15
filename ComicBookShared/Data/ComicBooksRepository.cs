using ComicBookShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ComicBookShared.Data
{
    public class ComicBooksRepository : BaseRepository<ComicBook>
    {
        public ComicBooksRepository(Context context)
            // base class contructor requires a Context instance
            : base(context)
        {
        }

        public override IList<ComicBook> GetList()
        {
            return Context.ComicBooks
                .Include(cb => cb.Series)
                .OrderBy(cb => cb.Series.Title)
                .ThenBy(cb => cb.IssueNumber)
                .ToList();
        }

        public override ComicBook Get(int id, bool includeRelatedEntities = true)
        {
            var comicBooks = Context.ComicBooks.AsQueryable();

            if (includeRelatedEntities)
            {
                comicBooks = comicBooks
                    .Include(cb => cb.Series)
                    .Include(cb => cb.Artists.Select(a => a.Artist))
                    .Include(cb => cb.Artists.Select(a => a.Role));
            }

            return comicBooks
                    .Where(cb => cb.Id == id)
                    .SingleOrDefault();
        }

        public bool ValidateComicBook(ComicBook comicBook)
        {
            return Context.ComicBooks
                    .Any(cb => cb.Id != comicBook.Id &&
                               cb.SeriesId == comicBook.SeriesId &&
                               cb.IssueNumber == comicBook.IssueNumber);
        }

        public bool ValidateArtist(int roleId, int artistId, int comicBookId)
        {
            return Context.ComicBookArtists
                    .Any(a => a.RoleId == roleId &&
                              a.ArtistId == artistId &&
                              a.ComicBookId == comicBookId);
        }
    }
}
