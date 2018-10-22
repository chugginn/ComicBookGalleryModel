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
                // the DbSet ToList method retrieves a list of entity instances.
                // with a relationship, like Comic Books to Series, we need
                // to be explicit and include the Series data. If we don't,
                // Comicbook.Series would be NULL. We can use a Linq Expression
                // to express the Series Entity
                var comicBooks = context.ComicBooks
                    .Include(cb => cb.Artists.Select(a => a.Artist))
                    .Include(cb => cb.Artists.Select(a => a.Role))
                    .Include(cb => cb.Series)
                    .ToList();

                // use foreach to loop through all instances of comic books
                // and write the series title property to the console
                foreach (var comicBook in comicBooks)
                {
                    // include a list of each comic book's artists. First, create a variable
                    // that uses LINQ to transform a collection of Artist entity objects to a
                    // list of Artist names:
                    var artistRoleNames = comicBook.Artists
                        .Select(a => $"{a.Artist.Name} - {a.Role.Name}").ToList();
                    // then use Join() to convert the collection of strings to a comma-delimited list
                    var artistRoleDisplayText = string.Join(", ", artistRoleNames);

                    Console.WriteLine(comicBook.DisplayText);
                    Console.WriteLine(artistRoleDisplayText);
                }
                Console.ReadLine();
            }


        }
    }
}
