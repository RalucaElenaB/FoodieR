namespace FoodieR.Models;

public class ShoppingCartViewModel
{
    //Un view model care ține informațiile despre coșul de cumpărături: Ține datele necesare pentru afișarea coșului(produse + total).
    public decimal ShoppingCartTotal { get; }
    public ShoppingCart ShoppingCart { get; }

    public ShoppingCartViewModel(ShoppingCart shoppingCart, decimal shoppingCartTotal)
    {
        ShoppingCart = shoppingCart;
        ShoppingCartTotal = shoppingCartTotal;
    }

}
