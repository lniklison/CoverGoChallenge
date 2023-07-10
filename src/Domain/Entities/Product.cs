using CoverGoChallenge.src.Domain.Entities.Base;

namespace CoverGoChallenge.src.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; }
        public string Description {get;}
        public int Stock { get; private set; }
        public int Price {get;}

        public Product(Guid id, string name, string description, int stock, int price) : base(id)
        {
            Name = name;
            Description = description;
            Stock = stock;
            Price = price;
        }

        public bool IsAvailable(int quantity)
        {
            return Stock >= quantity;
        }

        public void DecreaseStock(int quantity)
        {
            if (IsAvailable(quantity))
            {
                Stock -= quantity;
            }
        }

        public void IncreaseStock(int quantity)
        {
            Stock += quantity;
        }

    }
}
