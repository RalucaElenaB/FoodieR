using FoodieR.Data;
using FoodieR.Models.DbObject;
using Microsoft.EntityFrameworkCore;

namespace FoodieR.Repositories;

public class ReviewRepository
{
    private readonly ApplicationDbContext _context;
  

    public ReviewRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Review> GetReviewsByProductId(int productId)
    {
        return _context.Reviews.Where(review => review.Product.Id == productId);
    }


    //READ all Categories
    public async Task<IEnumerable<Review>> GetReviews()
    {
        return await _context.Reviews
            .ToListAsync();
    } 

    public async Task<IEnumerable<Review>> GetReviews(int pageSize, int pageNumber)
    {
        return await _context.Reviews
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    // GET: Reviews/Details/5
    public async Task<Review> GetReviewById(Guid id)
    {
        var review = await _context.Reviews
                .Include(r => r.CreatedBy)//aduce si include din baza de date detaliile despre autorul recenziei(dupa id din Users extrage toate info)
                .Include(r => r.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
        return review;
    }

    //CREATE
    public void AddReview(Review review)
    {
        _context.Reviews.Add(review);
        _context.SaveChanges();
    }

    //READ = filtrare
    public IEnumerable<Review> GetReviews(string searchReview)
    {
        return _context.Reviews
            .Where(review => review.Subject.Contains(searchReview))
            .ToList();
    }

    //UPDATE Category
    public void UpdateReview(Review review)
    {
        _context.Reviews.Update(review);
        _context.SaveChanges();
    }

    //DELETE Review
    public void DeleteReview(Guid id)
    {
        var review = _context.Reviews.FirstOrDefault(c => c.Id == id);
        if (review != null)
        {
            _context.Reviews.Remove(review);
            _context.SaveChanges();
        }
    }

    public  bool ReviewExists(Guid id)
    {
        return _context.Reviews.Any(e => e.Id == id);
    }

    //PAGINATION
    public async Task<int> GetAllReviewsCountAsync()
    {
        var count = await _context.Reviews.CountAsync();
        return count;
    }

    public async Task<IEnumerable<Review>> GetReviewsPagedAsync(int? pageNumber, int pageSize)
    {
        IQueryable<Review> reviews = _context.Reviews;

        pageNumber ??= 1; 
        reviews = reviews.Skip((pageNumber.Value-1) * pageSize).Take(pageSize);
        return await reviews.AsNoTracking().ToListAsync();
    
    }
}

