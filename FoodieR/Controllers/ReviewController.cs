using FoodieR.Data;
using FoodieR.Models;
using FoodieR.Models.DbObject;
using FoodieR.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FoodieR.Controllers
{
    public class ReviewController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;// a lega recenzia de un user
        private readonly ReviewRepository _reviewRepository;
        private readonly ProductRepository _productRepository;

        public ReviewController(ApplicationDbContext context, 
                                UserManager<IdentityUser> userManager, 
                                ReviewRepository reviewRepository,
                                ProductRepository productRepository)
        {
            //_context = context;
            _userManager = userManager;
            _reviewRepository = reviewRepository;
            _productRepository = productRepository;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()//actiune Index=view Index
        {
            var reviews = await _reviewRepository.GetReviews();
            var reviewViewModel = reviews.Select(review => ReviewViewModel.FromEntity(review));
            return View(reviewViewModel);
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(Guid id)//actiunea Details=view Details
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _reviewRepository.GetReviewById(id);

            var user = await _userManager.GetUserAsync(User);//extrag userul logat

            var reviewViewModel = ReviewViewModel.FromEntity(review);//convertim de la entitate la ViewModel si il pasam in View

            if (user != null)//verificam daca userul are acces la Edit/Delete Review(daca user e diferit de null)
            {
                reviewViewModel.HasEditAndDeletePermissions = string.Equals(user.Id, reviewViewModel.CreatedById);//compar cele 2 stringuri user.Id si CreatedById 
            }
            

            if (review == null)
            {
                return NotFound();
            }

            return View(reviewViewModel);
        }

        //GET: ProductController/Filter
        public async Task<ActionResult> Filter(string searchReview)
        {
            if (string.IsNullOrEmpty(searchReview))
            {
                var allReviews = await _reviewRepository.GetReviews();//Apelează metoda fără parametru pentru a obține toate categoriile
                return View("Index", allReviews.Select(review => ReviewViewModel.FromEntity(review)));
            }

            // Filtrare produse în funcție de termenul de căutare: Apelează metoda cu parametru pentru a obține categoriile filtrate
            var filteredReviews = _reviewRepository.GetReviews(searchReview);
            var reviewViewModel = filteredReviews.Select(review => ReviewViewModel.FromEntity(review));
            ViewData["CurrentFilter"] = searchReview; // Păstrează termenul de căutare

            return View("Index", reviewViewModel);
        }


        //Se apeleaza cand se deschide pagina de creare a unei noi recenzii
        // GET: Reviews/Create= PAS 1 afisam pagina de creare a unei recenzii
        [Authorize]
        public async Task<IActionResult> Create(int id)
        {
            var product = await _productRepository.GetProductById(id);

            var reviewViewModel = new ReviewViewModel { ProductId = id };
            reviewViewModel.Subject = product.Name;
            return View(reviewViewModel);
        }

        // POST: Reviews/Create = PAS 2 cream recenzia cand apsam pe butonul Create si salvam datele
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReviewViewModel reviewViewModel)
        {
            //if (ModelState.IsValid)
            //{

            var user = await _userManager.GetUserAsync(User);//extrag userul = obiect ce contine toate info despre el

            var review = reviewViewModel.ToEntity();
            var product = await _productRepository.GetProductById(reviewViewModel.ProductId);//_context.Products.FirstOrDefaultAsync(product => product.Id == reviewViewModel.ProductId);
            review.Product = product;
            review.Id = Guid.NewGuid(); 
            review.Created = DateTime.Now;

            review.CreatedBy = user;//legatura dintre review si user logat

            _reviewRepository.AddReview(review);
         
            return RedirectToAction("Details", "Product", new { id = product.Id });
            //}
            //return View(reviewViewModel);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _reviewRepository.GetReviewById(id);// _context.Reviews.FindAsync(id);//extragem recenzia din baza de date
            if (review == null)
            {
                return NotFound();
            }

            var reviewViewModel = ReviewViewModel.FromEntity(review);//populam ReviewViewModel cu datele corecte

            return View(reviewViewModel);//trimitem datele in View
        }

        // POST: Reviews/Edit/5 DUPA CE EDITAM, CAND DAM 'Save' se ajunge aici si noile date se trimit in baza de date
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, ReviewViewModel reviewViewModel)
        {
            if (id != reviewViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var review = reviewViewModel.ToEntity();

                    review.Modified = DateTime.Now;

                    _reviewRepository.UpdateReview(review);   
                 
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_reviewRepository.ReviewExists(reviewViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Product", new { id = reviewViewModel.ProductId });
            }
            return View(reviewViewModel);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _reviewRepository.GetReviewById(id);
            if (review == null)
            {
                return NotFound();
            }

            var reviewViewModel = ReviewViewModel.FromEntity(review);

            return View(reviewViewModel);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirme(Guid id)
        {
            var review = await _reviewRepository.GetReviewById(id);

            if (review != null)
            {
                _reviewRepository.DeleteReview(id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
