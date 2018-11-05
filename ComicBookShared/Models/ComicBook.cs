using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicBookShared.Models
{
    public class ComicBook
    {
        public ComicBook()
        {
            Artists = new List<ComicBookArtist>();
            AverageRatings = new List<ComicBookAverageRating>();
        }

        public int Id { get; set; }
        [Display(Name = "Issue Number")]
        public int IssueNumber { get; set; }
        // explicitly naming this property 'Series' with 'Id' is a convention that EF uses to
        // reference this ID field with the Series navigation property below. Doing
        // this ensures that Series is non-nullable, since int is non-nullable.
        // Also, this allows us to associate a Comicbook with a Series just by providing
        // the Series ID.
        [Display(Name = "Series")]
        public int SeriesId { get; set; }
        public string Description { get; set; }
        [Display(Name = "Published On")]
        public DateTime PublishedOn { get; set; }

        // many-to-one relationship created by the following 'navigation' property:
        public Series Series { get; set; }
        // many-to-many because Artist entity also has an ICollection navigation property:
        public ICollection<ComicBookArtist> Artists { get; set; }
        [Display(Name = "Average Rating")]
        public ICollection<ComicBookAverageRating> AverageRatings { get; set; }

        public string DisplayText
        {
            get
            {
                // The ? operator will allow to check if the property is null and
                // return null. Without it, we'd get a NullReferenceException
                return $"{Series?.Title} #{IssueNumber}";
            }
        }

        public void AddArtist(Artist artist, Role role)
        {
            Artists.Add(new ComicBookArtist()
            {
                Artist = artist,
                Role = role
            });
        }

        /// <summary>
        /// Adds an artist to the comic book.
        /// </summary>
        /// <param name="artistId">The artist ID to add.</param>
        /// <param name="roleId">The role ID that the artist had on this comic book.</param>
        public void AddArtist(int artistId, int roleId)
        {
            Artists.Add(new ComicBookArtist()
            {
                ArtistId = artistId,
                RoleId = roleId
            });
        }
    }
}
