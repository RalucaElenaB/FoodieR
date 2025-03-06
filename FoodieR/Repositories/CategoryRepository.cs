using FoodieR.Data;
using FoodieR.Models.DbObject;

namespace FoodieR.Repositories;

public class CategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    //CREATE
    public void AddCategory(Category category)
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
    }

    //READ = filtrare
    public IEnumerable<Category> GetCategories(string searchCategory)
    {
        return _context.Categories
            .Where(category => category.Name.Contains(searchCategory))
            .ToList();
    }

    //READ all Categories
    public IEnumerable<Category> GetCategories()
    {
        return _context.Categories.ToList();
    }

    //READ one Category by id
    public Category GetCategoryById(int id)
    {
        return _context.Categories.FirstOrDefault(c => c.Id == id);
    }

    //UPDATE Category
    public void UpdateCategory(Category category)
    {
        _context.Categories.Update(category);
        _context.SaveChanges();
    }

    //DELETE Category
    public void DeleteCategory(int id)
    {
        var category = _context.Categories.FirstOrDefault(c => c.Id == id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }

}
