using FoodieR.Models;
using FoodieR.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FoodieR.Controllers;

//Un controller care gestionează coșul de cumpărături, folosind dependență injectată (ProductRepository și ShoppingCart).
//Primește cereri pentru a adăuga/elimina produse și returnează View-ul ShoppingCart- Index.cshtml
public class ShoppingCartController : Controller
{
    private readonly ProductRepository _productRepository;
    private readonly ShoppingCart _shoppingCart;

    public ShoppingCartController(ProductRepository productRepository, ShoppingCart shoppingCart)
    {
        _productRepository = productRepository;
        _shoppingCart = shoppingCart;
    }

    //Index-ul cosului- unde vad toate articolele
    public ViewResult Index()
    {
        var items = _shoppingCart.GetShoppingCartItems();
        _shoppingCart.ShoppingCartItems = items;//toate articolele din cos

        var shoppingCartViewModel = new ShoppingCartViewModel(_shoppingCart, 
            _shoppingCart.GetShoppingCartTotal());//totalul cosului

        return View(shoppingCartViewModel);//returnez shoppingCartViewModel
    }

    public async Task<RedirectToActionResult> AddToShoppingCart(int productId)//metoda primeste id-ul cu care vreau sa interactionez
    {
        var selectedProduct = await _productRepository.GetProductById(productId);//caut in ProductRepository un ProductId existent

        if (selectedProduct != null)//daca nu e nul apelez AddToCart, trecand acel produs 
        {
            _shoppingCart.AddToCart(selectedProduct);//AddToCart are ogica in ShoppingCart
        }
        return RedirectToAction("Index");//fac redirect la RedirectToAction; nu returnez un View ci un apel la metoda RedirectToAction- care ne va redirectiona la index-ul cosului de cumparaturi
    }

    public async Task<RedirectToActionResult> RemoveFromShoppingCart(int productId)//metoda primeste id-ul cu care vreau sa interactionez
    {
        var selectedProduct = await _productRepository.GetProductById(productId);

        if (selectedProduct != null)
        {
            _shoppingCart.RemoveFromCart(selectedProduct);
        }
        return RedirectToAction("Index");
    }
}
