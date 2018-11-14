using ComicBookShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ComicBookShared.Data
{
    public class ComicBookArtistRepository : BaseRepository<ComicBookArtist>
    {

        public ComicBookArtistRepository(Context context) : base(context)
        {
        }

        public override ComicBookArtist Get(int id, bool includeRelatedEntities = true)
        {
            var comicBookArtists = Context.ComicBookArtists.AsQueryable();

            if (includeRelatedEntities)
            {
                comicBookArtists = comicBookArtists
                    .Include(a => a.ComicBook.Series)
                    .Include(a => a.Artist)
                    .Include(a => a.Role);
            }

            return comicBookArtists
                .Where(a => a.Id == (int)id)
                .SingleOrDefault();
        }

        public override IList<ComicBookArtist> GetList()
        {
            throw new NotImplementedException();
        }
    }
}
