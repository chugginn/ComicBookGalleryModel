using ComicBookGalleryModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                // the DbSet Add method takes an instance of the ComicBook class
                // to add an instance of this entity to the database.
                context.ComicBooks.Add(new ComicBook()
                {
                    SeriesTitle = "The Amazing Spiderman",
                    IssueNumber = 1,
                    PublishedOn = DateTime.Today
                });
                // SaveChanges needed to persist the data
                context.SaveChanges();

                // the DbSet ToList method retrieves a list of entity instances
                var comicBooks = context.ComicBooks.ToList();

                // use foreach to loop through all instances of comic books
                // and write the series title property to the console
                foreach (var comicBook in comicBooks)
                {
                    Console.WriteLine(comicBook.SeriesTitle);
                }
                Console.ReadLine();
            }


        }
    }
}
