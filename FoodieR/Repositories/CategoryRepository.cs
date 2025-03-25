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
    public void AddCategory(Category category)//Unit test: AddCategory_ShouldAddCategory()
    {
        _context.Categories.Add(category);
        _context.SaveChanges();
    }


 
    //READ all Categories
    public IEnumerable<Category> GetCategories()//Unit test: GetCategories_ShouldReturnAllCategories()
    {
        return _context.Categories.ToList();
    }

    //READ one Category by id
    public Category GetCategoryById(int id)//unit test: GetCategoryById_ShouldReturnCorrectCategory()
    {
        return _context.Categories.FirstOrDefault(c => c.Id == id);
    }


    //READ = filtrare
    public IEnumerable<Category> GetCategories(string searchCategory)//unit test: GetCategories_Filtered_ShouldReturnMatchingCategories()
    {
        return _context.Categories
            .Where(category => category.Name.Contains(searchCategory))
            .ToList();
    }


    //UPDATE Category
    public void UpdateCategory(Category category)//unit test: UpdateCategory_ShouldModifyExistingCategory()
    {
        _context.Categories.Update(category);
        _context.SaveChanges();
    }


    //DELETE Category
    public void DeleteCategory(int id)//unit test: DeleteCategory_ShouldRemoveCategory
    {
        var category = _context.Categories.FirstOrDefault(c => c.Id == id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }

}
