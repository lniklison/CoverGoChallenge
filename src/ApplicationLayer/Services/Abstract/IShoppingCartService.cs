namespace CoverGoChallenge.src.ApplicationLayer.Services.Abstract
{
    public interface IShoppingCartService
    {
        void AddProductToCart(string cartId, string productId, int quantity);
        void RemoveProductFromCart(string cartId, string productId, int quantity);
        decimal CalculateTotalPrice(string cartId);
    }
}
