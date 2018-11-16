using ComicBookShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ComicBookShared.Data
{
    public class ArtistsRepository : BaseRepository<Artist>
    {
        public ArtistsRepository(Context context)
            : base(context)
        {
        }

        public override Artist Get(int id, bool includeRelatedEntities = true)
        {
            var artist = Context.Artists.AsQueryable();

            if (includeRelatedEntities)
            {
                artist = artist
                    // from the Artist model, we are eagerly loading ComicBooks (a collection of 
                    // ComicBookArtist entities) and from there, the Series and Role entities
                    .Include(s => s.ComicBooks.Select(a => a.ComicBook.Series))
                    .Include(s => s.ComicBooks.Select(a => a.Role));
            }

            return artist.Where(a => a.Id == id).SingleOrDefault();
        }

        public override IList<Artist> GetList()
        {
            return Context.Artists
                .OrderBy(a => a.Name)
                .ToList();
        }

        public bool ValidateArtist(Artist artist)
        {
            return Context.Artists
                .Any(a => a.Id != artist.Id && 
                          a.Name == artist.Name);
        }
    }
}
