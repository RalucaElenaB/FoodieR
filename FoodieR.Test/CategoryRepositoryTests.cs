using FoodieR.Data;
using FoodieR.Models.DbObject;
using FoodieR.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FoodieR.Test;
//xUnit- framework-ul de testare folosit
//testez metodele din CategoryRepository

public class CategoryRepositoryTests
{
    private DbContextOptions<ApplicationDbContext> GetDbOptions() //metoda auxiliara care configurează DbContextOptions<ApplicationDbContext> pentru a folosi o bază de date în memorie (TestDatabase).
    {
        return new DbContextOptionsBuilder<ApplicationDbContext>()
             //.UseInMemoryDatabase(databaseName: "TestDatabase")
             //.UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Nume unic pentru fiecare test
            .Options;// Fiecare test creează o instanță nouă a bazei de date pentru a asigura independența testelor.
    }

    [Fact]//rularea de catre runnerul de testare; indică faptul că metoda de mai jos este un test unit și trebuie executată de runner-ul de testare.
    //prepare(arrange) - act - assert
    //AddCategory()
    public void AddCategory_ShouldAddCategory()//semnătura metodei: modificator de acces- public, tip de return- void, nume metoda(urmează convenția Actiune_Expectativa)
    {
        var options = GetDbOptions();//Aici se obține configurația bazei de date în memorie folosind GetDbOptions()= fiecare test rulează într-o bază de date separată, prevenind interferențele între teste.
        using (var context = new ApplicationDbContext(options))//Creează un nou context de bază de date (instanță a ApplicationDbContext). using → asigură că context este eliberat automat după ce blocul de cod se termină.
        {
            var repository = new CategoryRepository(context);//Creează un CategoryRepository folosind context.Acest repository este responsabil de operațiile asupra entității Category în baza de date.
            
            var category = new Category { Id = 1, Name = "TestCategory" }; //Creează un obiect Category cu Id = 1 și Name = "TestCategory".

            repository.AddCategory(category);//Apelează metoda AddCategory() din CategoryRepository, care:Adaugă categoria în baza de date.Salvează modificările.

            Assert.NotEmpty(context.Categories);//Verifică dacă lista de categorii NU este goală, ceea ce înseamnă că s-a adăugat ceva în baza de date.
            Assert.Equal("TestCategory", context.Categories.First().Name);//verifica daca categoria adăugată are numele "TestCategory";context.Categories.First() → ia prima categorie din baza de date.
        }
    }

    //GetCategories()
    [Fact]
    public void GetCategories_ShouldReturnAllCategories()
    {
        var options = GetDbOptions();
        using (var context = new ApplicationDbContext(options))
        {
            context.Categories.Add(new Category { Id = 1, Name = "Category1" });
            context.Categories.Add(new Category { Id = 2, Name = "Category2" });
            context.SaveChanges();

            var repository = new CategoryRepository(context);
            var categories = repository.GetCategories();

            Assert.Equal(2, categories.Count());
        }
    }

    //GetCategories() cu filtrare
    [Fact]
    public void GetCategories_Filtered_ShouldReturnMatchingCategories()
    {
        var options = GetDbOptions();
        using (var context = new ApplicationDbContext(options))
        {
            context.Categories.Add(new Category { Id = 1, Name = "Tech" });
            context.Categories.Add(new Category { Id = 2, Name = "Science" });
            context.SaveChanges();

            var repository = new CategoryRepository(context);
            var categories = repository.GetCategories("Tech");

            Assert.Single(categories);
            Assert.Equal("Tech", categories.First().Name);
        }
    }

    //GetCategoryById()
    [Fact]
    public void GetCategoryById_ShouldReturnCorrectCategory()
    {
        var options = GetDbOptions();
        using (var context = new ApplicationDbContext(options))
        {
            var category = new Category { Id = 1, Name = "TestCategory" };
            context.Categories.Add(category);
            context.SaveChanges();

            var repository = new CategoryRepository(context);
            var result = repository.GetCategoryById(1);

            Assert.NotNull(result);
            Assert.Equal("TestCategory", result.Name);
        }
    }

    //UpdateCategory()
    [Fact]
    public void UpdateCategory_ShouldModifyExistingCategory()
    {
        var options = GetDbOptions();
        using (var context = new ApplicationDbContext(options))
        {
            var category = new Category { Id = 1, Name = "OldName" };
            context.Categories.Add(category);
            context.SaveChanges();

            var repository = new CategoryRepository(context);
            category.Name = "NewName";
            repository.UpdateCategory(category);

            var updatedCategory = context.Categories.Find(1);
            Assert.Equal("NewName", updatedCategory.Name);
        }
    }

    //DeleteCategory()
    [Fact]
    public void DeleteCategory_ShouldRemoveCategory()
    {
        var options = GetDbOptions();
        using (var context = new ApplicationDbContext(options))
        {
            var category = new Category { Id = 1, Name = "ToDelete" };
            context.Categories.Add(category);
            context.SaveChanges();

            var repository = new CategoryRepository(context);
            repository.DeleteCategory(1);

            Assert.Empty(context.Categories);
        }
    }
}
