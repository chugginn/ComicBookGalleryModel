﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComicBookGalleryModel.Models
{
    public class ComicBook
    {
        public int Id { get; set; }
        public int IssueNumber { get; set; }
        // explicitly naming this property 'Series' with 'Id' is a convention that EF uses to
        // reference this ID field with the Series navigation property below. Doing
        // this ensures that Series is non-nullable, since int is non-nullable.
        // Also, this allows us to associate a Comicbook with a Series just by providing
        // the Series ID.
        public int SeriesId { get; set; }
        public string Description { get; set; }
        public DateTime PublishedOn { get; set; }
        public decimal? AverageRating { get; set; }

        // many-to-one relationship created by the following 'navigation' property:
        public Series Series { get; set; }

        public string DisplayText
        {
            get
            {
                // The ? operator will allow to check if the property is null and
                // return null. Without it, we'd get a NullReferenceException
                return $"{Series?.Title} #{IssueNumber}";
            }
        }
    }
}
