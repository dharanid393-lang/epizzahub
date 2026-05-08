using ePizza.Domain.Models;


namespace ePizza.Domain.Interfaces
{
    public interface ICartItemRepository : IGenericRepository<CartItemDomain>
    {
        Task<CartItemDomain> GetCartItemsAsync(Guid cartId, int itemId);
    }
}
