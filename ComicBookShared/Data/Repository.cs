using ComicBookShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ComicBookShared.Data
{
    public class Repository
    {
        private Context _context = null;

        public Repository(Context context)
        {
            _context = context;
        }

        public IList<ComicBook> GetComicBooks()
        {
            return _context.ComicBooks
                .Include(cb => cb.Series)
                .OrderBy(cb => cb.Series.Title)
                .ThenBy(cb => cb.IssueNumber)
                .ToList();
        }

        public ComicBook GetComicBook(int? id, bool includeRelatedEntities = true)
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

        public IList<Series> GetSeriesList()
        {
            return _context.Series.OrderBy(s => s.Title).ToList();
        }

        public IList<Artist> GetArtists()
        {
            return _context.Artists.OrderBy(a => a.Name).ToList();
        }

        public ComicBookArtist GetArtist(int? id)
        {
            return _context.ComicBookArtists
                .Include(a => a.ComicBook.Series)
                .Include(a => a.Artist)
                .Include(a => a.Role)
                .Where(a => a.Id == (int)id)
                .SingleOrDefault();
        }

        public IList<Role> GetRoles()
        {
            return _context.Roles.OrderBy(r => r.Name).ToList();
        }

        public void AddComicBook(ComicBook comicBook)
        {
            _context.ComicBooks.Add(comicBook);
            _context.SaveChanges();
        }

        public void EditComicBook(ComicBook comicBook)
        {
            _context.Entry(comicBook).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteComicBook(int id)
        {
            var comicBook = new ComicBook() { Id = id };
            _context.Entry(comicBook).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public void DeleteArtist(int id)
        {
            var artist = new ComicBookArtist { Id = id };
            _context.Entry(artist).State = EntityState.Deleted;
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

        public void AddArtist(ComicBookArtist comicBookArtist)
        {
            _context.ComicBookArtists.Add(comicBookArtist);
            _context.SaveChanges();
        }
    }
}
