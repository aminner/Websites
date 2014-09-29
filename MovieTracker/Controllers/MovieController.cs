using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using MovieTracker.Data;
using MovieTracker.Models;
using MovieTracker.Services;

namespace MovieTracker.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {
        private readonly IRepository<Genre> _genreRepository;
        private readonly IMembershipService _membershipService;
        private readonly IRepository<Movie> _movieRepository;
        
        public MovieController(IRepository<Movie> movieRepository, IRepository<Genre> genreRepository, IMembershipService membershipService)
        {
            _movieRepository = movieRepository;
            _genreRepository = genreRepository;
            _membershipService = membershipService;
        }

        [OutputCache(Duration = 15, VaryByParam="none")]
        public ActionResult Index()
        {
            MembershipUser user = _membershipService.GetUser(User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Register", "Account");
            }
            var userKey = (Guid) user.ProviderUserKey;
            IEnumerable<Movie> movies = _movieRepository.Find(m => m.aspnet_UsersUserId == userKey);
            return View(movies);
        }

        [HttpPost]
        public ActionResult Filter(string filter)
        {
            MembershipUser user = _membershipService.GetUser(User.Identity.Name);
            var userKey = (Guid)user.ProviderUserKey;
            if (filter == null || filter == "")
                return View("MovieList", _movieRepository.Find(m => m.aspnet_UsersUserId == userKey));
            IEnumerable<Movie> movies = _movieRepository.Find(m => m.Name.Contains(filter) && m.aspnet_UsersUserId == userKey);
            return View("MovieList", movies);
        }

        public ActionResult Detail(int? id)
        {
            if (id == null)
                return Redirect("Index");
            MembershipUser user = _membershipService.GetUser(User.Identity.Name);
            if (user == null)
                return RedirectToAction("Register", "Account");
            var userKey = (Guid)user.ProviderUserKey;
            Movie movie = _movieRepository.Find(m => m.Id == id&&m.aspnet_UsersUserId==userKey).FirstOrDefault();
            if (movie == null)
            {
                return RedirectToAction("MovieNotInCollection");
            }
            return View(movie);
        }

        public ActionResult Edit(int? id)
        {
            if (User == null)
                return RedirectToAction("Register", "Action");
            if (id == null)
                return Redirect("Index");
            MembershipUser user = _membershipService.GetUser(User.Identity.Name);
            if (user == null)
                return RedirectToAction("Register", "Account");
            
            var userKey = (Guid)user.ProviderUserKey;
            IEnumerable<Movie> movies = _movieRepository.Find(m => m.Id == id);
            Movie movie = movies.Where(m => m.aspnet_UsersUserId == userKey).SingleOrDefault();
            if (movies.Count() <= 0)
                return RedirectToAction("MoveNotFound");
            if (movie == null)
            {
                return RedirectToAction("MovieNotInCollection");
            }
          
            IEnumerable<Genre> genres = _genreRepository.GetAll();
            var editMovieViewModel = new EditMovieViewModel
                                         {
                                             Directors = movie.Directors,
                                             Genres = new SelectList(genres, "Id", "Name"),
                                             Name = movie.Name,
                                             Rating = movie.Rating,
                                             Stars = movie.Stars,
                                             Writers = movie.Writers,
                                             Id = (int)id,
                                             GenreId = movie.GenreId
                                         };
            return View(editMovieViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditMovieViewModel editMovieViewModel)
        {
            if (ModelState.IsValid)
            {
                Movie movie = _movieRepository.Find(m => m.Id == editMovieViewModel.Id).FirstOrDefault();

                if (movie == null)
                {
                    return RedirectToAction("MoveNotFound");
                }

                movie.Directors = editMovieViewModel.Directors;
                movie.GenreId = editMovieViewModel.GenreId;
                movie.Name = editMovieViewModel.Name;
                movie.Rating = editMovieViewModel.Rating;
                movie.Stars = editMovieViewModel.Stars;
                movie.Writers = editMovieViewModel.Writers;

                _movieRepository.Save();
                return RedirectToAction("Index");
            }
            IEnumerable<Genre> genres = _genreRepository.GetAll();
            editMovieViewModel.Genres = new SelectList(genres, "Id", "Name");

            return View(editMovieViewModel);
        }

        public ActionResult Add()
        {
            IEnumerable<Genre> genres = _genreRepository.GetAll();
            var editMovieViewModel = new EditMovieViewModel
                                         {
                                             Genres = new SelectList(genres, "Id", "Name"),
                                         };
            return View(editMovieViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(EditMovieViewModel editMovieViewModel)
        {
            if (ModelState.IsValid)
            {
                MembershipUser user = _membershipService.GetUser(User.Identity.Name);
                if (user == null)
                {
                    return RedirectToAction("Register", "Account");
                }
                var userKey = (Guid) user.ProviderUserKey;
                var movie = new Movie
                                {
                                    Directors = editMovieViewModel.Directors,
                                    GenreId = editMovieViewModel.GenreId,
                                    Name = editMovieViewModel.Name,
                                    Rating = editMovieViewModel.Rating,
                                    Stars = editMovieViewModel.Stars,
                                    Writers = editMovieViewModel.Writers,
                                    aspnet_UsersUserId = userKey
                                };

                _movieRepository.Add(movie);
                _movieRepository.Save();
                return RedirectToAction("Index");
            }
            IEnumerable<Genre> genres = _genreRepository.GetAll();
            editMovieViewModel.Genres = new SelectList(genres, "Id", "Name");

            return View(editMovieViewModel);
        }

        public ActionResult Delete(int? id)
        {
            if (User == null)
                return RedirectToAction("Register", "Action");
            if (id == null)
                return Redirect("Index");
            MembershipUser user = _membershipService.GetUser(User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Register", "Account");
            }

            var userKey = (Guid)user.ProviderUserKey;
            IEnumerable<Movie> movies = _movieRepository.Find(m => m.Id == id);
            Movie movie = movies.Where(m => m.aspnet_UsersUserId == userKey).SingleOrDefault();
            if (movies.Count() <= 0)
                return RedirectToAction("MoveNotFound");
            if (movie == null)
            {
                return RedirectToAction("MovieNotInCollection");
            }
            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Movie movie)
        {
            Movie movieToDelete = _movieRepository.Find(m => m.Id == movie.Id).FirstOrDefault();
            _movieRepository.Delete(movieToDelete);
            _movieRepository.Save();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ClearRating(int movieId)
        {
            Movie movie = _movieRepository.Find(m => m.Id == movieId).Single();
            movie.ClearRating();

            _movieRepository.Save();

            return View("MovieRatingControl", movie);
        }

        [HttpPost]
        public ActionResult AddRating(int movieId, short rating)
        {
            Movie movie = _movieRepository.Find(m => m.Id == movieId).Single();
            movie.Rating = rating;
            _movieRepository.Save();
            return View("MovieRatingControl", movie);
        }

        public ActionResult MoveNotFound()
        {
            return View();
        }

        public ActionResult MovieNotInCollection()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LentOut(int movieId, String borrower, String borrowedDate)
        {
            MembershipUser user = _membershipService.GetUser(User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Register", "Account");
            }
            
            var userKey = (Guid)user.ProviderUserKey; 
            Movie movie = _movieRepository.Find(m=> m.Id== movieId).FirstOrDefault();
            if (movie == null)
                return Redirect("Index");
            if (borrower == null || borrowedDate == null)
                return RedirectToAction("Index","Movie");
            movie.BorrowerName = borrower;
            movie.BorrowedDate = Convert.ToDateTime(borrowedDate);
            _movieRepository.Save();
            return View("MovieDetailLentStatus", movie);
        }
        [HttpPost]
        public ActionResult Returned(int movieId, string reloadLoc)
        {
            Movie movie = _movieRepository.Find(m => m.Id == movieId).FirstOrDefault();
            if (movie == null)
                return Redirect("Index");
            movie.Returned();
            _movieRepository.Save();
            return View(reloadLoc, movie);
        }
    }
}