using ComicBookShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ComicBookShared.Data
{
    public class SeriesRepository : BaseRepository<Series>
    {
        public SeriesRepository(Context context)
            : base(context)
        {
        }

        public override Series Get(int id, bool includeRelatedEntities = true)
        {
            var seriesList = Context.Series.AsQueryable();

            if (includeRelatedEntities)
            {
                seriesList = seriesList
                    .Include(s => s.ComicBooks);
            }

            return seriesList.Where(s => s.Id == id)
                .SingleOrDefault();
        }

        public override IList<Series> GetList()
        {
            return Context.Series
                .OrderBy(s => s.Title)
                .ToList();
        }

        public IList<ComicBook> GetComicBooks(int id)
        {
            return Context.ComicBooks
                .Include(cb => cb.Series)
                .Where(cb => cb.SeriesId == id)
                .ToList();
        }

        public bool ValidateSeries(Series series)
        {
            return Context.Series
                .Any(s => s.Id != series.Id &&
                          s.Title == series.Title);
        }
    }
}
