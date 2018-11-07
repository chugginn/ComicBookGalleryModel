using ComicBookLibraryManagerWebApp.ViewModels;
using ComicBookShared.Data;
using ComicBookShared.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ComicBookLibraryManagerWebApp.Controllers
{
    /// <summary>
    /// Controller for adding/deleting comic book artists.
    /// </summary>
    public class ComicBookArtistsController : Controller
    {
        // create private field to hold reference to a context object
        private Context _context = null;

        public ComicBookArtistsController()
        {
            _context = new Context();
        }

        public ActionResult Add(int comicBookId)
        {
            var comicBook = _context.ComicBooks
                .Include(cb => cb.Series)
                .Where(cb => cb.Id == comicBookId)
                .SingleOrDefault();

            if (comicBook == null)
            {
                return HttpNotFound();
            }

            var viewModel = new ComicBookArtistsAddViewModel()
            {
                ComicBook = comicBook
            };

            viewModel.Init(_context);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Add(ComicBookArtistsAddViewModel viewModel)
        {
            ValidateComicBookArtist(viewModel);

            if (ModelState.IsValid)
            {
                var comicBookArtist = new ComicBookArtist()
                {
                    ComicBookId = viewModel.ComicBookId,
                    ArtistId = viewModel.ArtistId,
                    RoleId = viewModel.RoleId
                };

                _context.ComicBookArtists.Add(comicBookArtist);
                _context.SaveChanges();

                TempData["Message"] = "Your artist was successfully added!";

                return RedirectToAction("Detail", "ComicBooks", new { id = viewModel.ComicBookId });
            }

            // TODO Prepare the view model for the view.
            // TODO Get the comic book.
            // Include the "Series" navigation property.
            viewModel.ComicBook = _context.ComicBooks
                .Include(cb => cb.Series)
                .Where(cb => cb.Id == viewModel.ComicBookId)
                .SingleOrDefault();
            viewModel.Init(_context);

            return View(viewModel);
        }

        public ActionResult Delete(int comicBookId, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // TODO Get the comic book artist.
            // Include the "ComicBook.Series", "Artist", and "Role" navigation properties.
            var comicBookArtist = _context.ComicBookArtists
                .Include(a => a.ComicBook.Series)
                .Include(a => a.Artist)
                .Include(a => a.Role)
                .Where(a => a.Id == (int)id)
                .SingleOrDefault();

            if (comicBookArtist == null)
            {
                return HttpNotFound();
            }

            return View(comicBookArtist);
        }

        [HttpPost]
        public ActionResult Delete(int comicBookId, int id)
        {
            var artist = new ComicBookArtist { Id = id };
            _context.Entry(artist).State = EntityState.Deleted;
            _context.SaveChanges();

            TempData["Message"] = "Your artist was successfully deleted!";

            return RedirectToAction("Detail", "ComicBooks", new { id = comicBookId });
        }

        /// <summary>
        /// Validates a comic book artist on the server
        /// before adding a new record.
        /// </summary>
        /// <param name="viewModel">The view model containing the values to validate.</param>
        private void ValidateComicBookArtist(ComicBookArtistsAddViewModel viewModel)
        {
            // If there aren't any "ArtistId" and "RoleId" field validation errors...
            if (ModelState.IsValidField("ArtistId") &&
                ModelState.IsValidField("RoleId"))
            {
                // Then make sure that this artist and role combination 
                // doesn't already exist for this comic book.
                // TODO Call method to check if this artist and role combination
                // already exists for this comic book.
                if (_context.ComicBookArtists
                    .Any(a => a.RoleId == viewModel.RoleId &&
                              a.ArtistId == viewModel.ArtistId &&
                              a.ComicBookId == viewModel.ComicBookId))
                {
                    ModelState.AddModelError("ArtistId",
                        "This artist and role combination already exists for this comic book.");
                }
            }
        }

        //  private field to hold whether or not disposal has been called to safeguard against
        // Dispose() being called more than once.
        private bool _disposed = false;

        // type 'override' to get a list of methods that can be overridden
        protected override void Dispose(bool disposing)
        {
            // check if disposal has been called and if so, short circuit the method by returning
            if (_disposed)
                return;

            // only dispose of the context if disposing parameter is true
            // this will prevent us from releasing managed resources that have already been reclaimed
            if (disposing)
            {
                _context.Dispose();
            }

            _disposed = true;

            base.Dispose(disposing);
        }
    }
}