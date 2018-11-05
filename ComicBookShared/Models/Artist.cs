using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComicBookShared.Models
{
    public class Artist
    {
        public Artist()
        {
            ComicBooks = new List<ComicBookArtist>();
        }

        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; }
        [Required]
        public string Bio { get; set; }

        public ICollection<ComicBookArtist> ComicBooks { get; set; }
    }
}
