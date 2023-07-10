using CoverGoChallenge.src.Domain.Entities;

namespace CoverGoChallenge.src.RepositoryLayer.Repositories.Abstract
{
    public interface IShoppingCartRepository
    {
        ShoppingCart GetCart(string cartId);
        void SaveCart(ShoppingCart cart);
        void AddCartItem(string cartId, CartItem item);
    }
}
