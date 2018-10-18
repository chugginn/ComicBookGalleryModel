using ComicBookGalleryModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ComicBookGalleryModel
{
    class Program
    {
        static void Main(string[] args)
        {
            // using directive takes advantage of the IDisposable interface to free
            // memory when DbSet is no longer being used.
            using (var context = new Context())
            {
                var series = new Series()
                {
                    Title = "The Amazing Spiderman"
                };
                // the DbSet Add method takes an instance of the ComicBook class
                // to add an instance of this entity to the database.
                context.ComicBooks.Add(new ComicBook()
                {
                    Series = series,
                    IssueNumber = 1,
                    PublishedOn = DateTime.Today
                });
                context.ComicBooks.Add(new ComicBook()
                {
                    Series = series,
                    IssueNumber = 2,
                    PublishedOn = DateTime.Today
                });
                // SaveChanges needed to persist the data
                context.SaveChanges();

                // the DbSet ToList method retrieves a list of entity instances
                // with a relationship, like Comic Books to Series, we need
                // to be explicit and include the Series data. If we don't,
                // Comicbook.Series would be NULL. We can use a Linq Expression
                // to express the Series Entity
                var comicBooks = context.ComicBooks
                    .Include(cb => cb.Series)
                    .ToList();

                // use foreach to loop through all instances of comic books
                // and write the series title property to the console
                foreach (var comicBook in comicBooks)
                {
                    Console.WriteLine(comicBook.DisplayText);
                }
                Console.ReadLine();
            }


        }
    }
}
