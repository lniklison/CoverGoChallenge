using CoverGoChallenge.src.Domain.Entities.Base;

namespace CoverGoChallenge.src.Domain.Entities
{
    public class ShoppingCart : BaseEntity
    {
        private readonly List<CartItem> _items;

        public ShoppingCart(Guid id) : base(id)
        {
            Id = id;
            _items = new List<CartItem>();
        }

        public void AddItem(Product product, int quantity)
        {
            var existingItem = _items.FirstOrDefault(item => item.Product.Id == product.Id);
            if (existingItem != null)
            {
                existingItem.IncreaseQuantity(quantity);
            }
            else
            {
                var newItem = new CartItem(product, quantity);
                _items.Add(newItem);
            }
        }

        public void RemoveItem(Product product, int quantity)
        {
            var existingItem = _items.FirstOrDefault(item => item.Product.Id == product.Id);
            if (existingItem != null)
            {
                existingItem.DecreaseQuantity(quantity);
                if (existingItem.Quantity == 0)
                {
                    _items.Remove(existingItem);
                }
            }
        }

        public List<CartItem> GetItems()
        {
            return _items;
        }
    }
}
