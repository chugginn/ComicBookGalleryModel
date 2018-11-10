using ComicBookShared.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComicBookLibraryManagerWebApp.Controllers
{
    public abstract class BaseController : Controller
    {
        private Context _context = null;

        //  private field to hold whether or not disposal has been called to safeguard against
        // Dispose() being called more than once.
        private bool _disposed = false;

        // use protected property so that only this class or descendants of this class have access
        // private setter because only being set from within this class
        protected Repository Repository { get; private set; }

        public BaseController()
        {
            _context = new Context();
            Repository = new Repository(_context);
        }

        // we will override the Controller Dispose method so that we can dispose of the context
        // once it has been used.

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