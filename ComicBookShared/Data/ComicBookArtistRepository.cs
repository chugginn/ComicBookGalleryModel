using ComicBookShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ComicBookShared.Data
{
    public class ComicBookArtistRepository
    {
        private Context _context = null;

        public ComicBookArtistRepository(Context context)
        {
            _context = context;
        }

        public ComicBookArtist Get(int? id)
        {
            return _context.ComicBookArtists
                .Include(a => a.ComicBook.Series)
                .Include(a => a.Artist)
                .Include(a => a.Role)
                .Where(a => a.Id == (int)id)
                .SingleOrDefault();
        }

        public void Add(ComicBookArtist comicBookArtist)
        {
            _context.ComicBookArtists.Add(comicBookArtist);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var artist = new ComicBookArtist { Id = id };
            _context.Entry(artist).State = EntityState.Deleted;
            _context.SaveChanges();
        }
    }
}
