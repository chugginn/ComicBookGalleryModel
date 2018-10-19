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
                var series1 = new Series()
                {
                    Title = "The Amazing Spiderman"
                };
                var series2 = new Series()
                {
                    Title = "The Invincible Iron Man"
                };

                var artist1 = new Artist()
                {
                    Name = "Stan Lee"
                };
                var artist2 = new Artist()
                {
                    Name = "Steve Ditko"
                };
                var artist3 = new Artist()
                {
                    Name = "Jack Kirby"
                };
                var comicBook1 = new ComicBook()
                {
                    Series = series1,
                    IssueNumber = 1,
                    PublishedOn = DateTime.Today
                };
                comicBook1.Artists.Add(artist1);
                comicBook1.Artists.Add(artist2);

                var comicBook2 = new ComicBook()
                {
                    Series = series2,
                    IssueNumber = 2,
                    PublishedOn = DateTime.Today
                };
                comicBook2.Artists.Add(artist1);
                comicBook2.Artists.Add(artist2);

                var comicBook3 = new ComicBook()
                {
                    Series = series2,
                    IssueNumber = 1,
                    PublishedOn = DateTime.Today
                };
                comicBook3.Artists.Add(artist1);
                comicBook3.Artists.Add(artist3);

                // the DbSet Add method takes an instance of the ComicBook class
                // to add an instance of this entity to the database.
                context.ComicBooks.Add(comicBook1);
                context.ComicBooks.Add(comicBook2);
                context.ComicBooks.Add(comicBook3);

                // SaveChanges needed to persist the data
                context.SaveChanges();

                // the DbSet ToList method retrieves a list of entity instances.
                // with a relationship, like Comic Books to Series, we need
                // to be explicit and include the Series data. If we don't,
                // Comicbook.Series would be NULL. We can use a Linq Expression
                // to express the Series Entity
                var comicBooks = context.ComicBooks
                    .Include(cb => cb.Artists)
                    .Include(cb => cb.Series)
                    .ToList();

                // use foreach to loop through all instances of comic books
                // and write the series title property to the console
                foreach (var comicBook in comicBooks)
                {
                    // include a list of each comic book's artists. First, create a variable
                    // that uses LINQ to transform a collection of Artist entity objects to a
                    // list of Artist names:
                    var artistNames = comicBook.Artists.Select(a => a.Name).ToList();
                    // then use Join() to convert the collection of strings to a comma-delimited list
                    var artistsDisplayText = string.Join(", ", artistNames);

                    Console.WriteLine(comicBook.DisplayText);
                    Console.WriteLine(artistsDisplayText);
                }
                Console.ReadLine();
            }


        }
    }
}
