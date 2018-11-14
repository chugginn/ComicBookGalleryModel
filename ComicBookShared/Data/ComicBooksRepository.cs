using ComicBookShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ComicBookShared.Data
{
    public class ComicBooksRepository
    {
        // this field used only within this class
        private Context _context = null;

        public ComicBooksRepository(Context context)
        {
            _context = context;
        }

        public IList<ComicBook> GetList()
        {
            return _context.ComicBooks
                .Include(cb => cb.Series)
                .OrderBy(cb => cb.Series.Title)
                .ThenBy(cb => cb.IssueNumber)
                .ToList();
        }

        public ComicBook Get(int? id, bool includeRelatedEntities = true)
        {
            var comicBooks = _context.ComicBooks.AsQueryable();

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

        public void Edit(ComicBook comicBook)
        {
            _context.Entry(comicBook).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var comicBook = new ComicBook() { Id = id };
            _context.Entry(comicBook).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public void Add(ComicBook comicBook)
        {
            _context.ComicBooks.Add(comicBook);
            _context.SaveChanges();
        }

        public bool ValidateComicBook(ComicBook comicBook)
        {
            return _context.ComicBooks
                    .Any(cb => cb.Id != comicBook.Id &&
                               cb.SeriesId == comicBook.SeriesId &&
                               cb.IssueNumber == comicBook.IssueNumber);
        }

        public bool ValidateArtist(int roleId, int artistId, int comicBookId)
        {
            return _context.ComicBookArtists
                    .Any(a => a.RoleId == roleId &&
                              a.ArtistId == artistId &&
                              a.ComicBookId == comicBookId);
        }
    }
}
