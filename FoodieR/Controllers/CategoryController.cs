﻿using FoodieR.Models;
using FoodieR.Models.DbObject;
using FoodieR.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FoodieR.Controllers
{

    public class CategoryController : Controller
    {
        private readonly CategoryRepository _categoryRepository;


        public CategoryController(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

      
        // GET: CategoryController
        public async Task<ActionResult> Index()
        {
            var categories = _categoryRepository.GetCategories();
            var categoryViewModel = new CategoryViewModel
            {
                Categories = categories
            };
      
            return View(categoryViewModel);
        }

        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            return View(category);
        }


        //GET: ProductController/Filter
        public async Task<ActionResult> Filter(string searchCategory)
        {
            if (string.IsNullOrEmpty(searchCategory))
            {
                var allCategories = _categoryRepository.GetCategories();//Apelează metoda fără parametru pentru a obține toate categoriile
                return View("Index", allCategories); 
            }

            // Filtrare produse în funcție de termenul de căutare: Apelează metoda cu parametru pentru a obține categoriile filtrate
            var filteredCategories = _categoryRepository.GetCategories(searchCategory);
            ViewData["CurrentFilter"] = searchCategory; // Păstrează termenul de căutare

            var categoryViewModel = new CategoryViewModel
            {
                Categories = filteredCategories
            };
            return View("Index", categoryViewModel); 
        }


        // GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                Category category = new Category
                {
                    //Id= int.Parse(collection["Id"]),
                    Name = collection["Name"],
                    Description = collection["Description"]//add de prop noua
                };
                _categoryRepository.AddCategory(category);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            return View(category);
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                var category = _categoryRepository.GetCategoryById(id);
      
                category.Name = collection["Name"];
                category.Description = collection["Description"];
                _categoryRepository.UpdateCategory(category);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            return View(category);
        }

        // POST: CategoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                _categoryRepository.DeleteCategory(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
