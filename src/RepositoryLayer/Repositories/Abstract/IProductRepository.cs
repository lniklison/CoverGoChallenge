using CoverGoChallenge.src.Domain.Entities;

namespace CoverGoChallenge.src.RepositoryLayer.Repositories.Abstract
{
    public interface IProductRepository
    {
        Product GetById(string productId);
        List<Product> GetAvailableProducts();
        void UpdateProduct(Product product);
    }
}
