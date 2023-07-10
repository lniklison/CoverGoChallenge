using CoverGoChallenge.src.ApplicationLayer.Services.Abstract;
using CoverGoChallenge.src.Domain.Entities;
using CoverGoChallenge.src.RepositoryLayer.Repositories.Abstract;

namespace CoverGoChallenge.src.ApplicationLayer.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IProductRepository _productRepository;
        private readonly IShoppingCartRepository _cartRepository;

        public ShoppingCartService(IProductRepository productRepository, IShoppingCartRepository cartRepository)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
        }

        public void AddProductToCart(string cartId, string productId, int quantity)
        {
            var product = _productRepository.GetById(productId);
            if (product == null)
            {
                throw new ArgumentException("Product not found.");
            }

            if (!product.IsAvailable(quantity))
            {
                throw new InvalidOperationException("Product is out of stock.");
            }

            var cart = _cartRepository.GetCart(cartId);
            if (cart == null)
            {
                var cartGuid = Guid.Parse(cartId);
                cart = new ShoppingCart(cartGuid);
            }

            cart.AddItem(product, quantity);
            product.DecreaseStock(quantity);

            _cartRepository.SaveCart(cart);
        }

        public void RemoveProductFromCart(string cartId, string productId, int quantity)
        {
            var cart = _cartRepository.GetCart(cartId);
            if (cart == null)
            {
                throw new ArgumentException("Cart not found.");
            }

            var product = _productRepository.GetById(productId);
            if (product == null)
            {
                throw new ArgumentException("Product not found.");
            }

            cart.RemoveItem(product, quantity);
            product.IncreaseStock(quantity);

            _cartRepository.SaveCart(cart);
        }

        public decimal CalculateTotalPrice(string cartId)
        {
            var cart = _cartRepository.GetCart(cartId);
            if (cart == null)
            {
                throw new ArgumentException("Cart not found.");
            }
            var cartItems = cart.GetItems();
            decimal totalPrice = 0;
            foreach (var cartItem in cartItems)
            {
                totalPrice += cartItem.Product.Price * cartItem.Quantity;
            }

            return totalPrice;
        }
    }
}