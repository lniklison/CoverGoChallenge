namespace CoverGoChallenge.src.Domain.Entities
{
    public class CartItem
    {
        public Product Product { get; }
        public int Quantity { get; private set; }

        public CartItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public void IncreaseQuantity(int quantity)
        {
            Quantity += quantity;
        }

        public void DecreaseQuantity(int quantity)
        {
            Quantity -= quantity;
        }
    }
}
