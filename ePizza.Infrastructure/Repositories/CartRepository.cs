using AutoMapper;
using ePizza.Domain.Interfaces;
using ePizza.Domain.Models;
using ePizza.Infrastructure.Entities;
using ePizza.Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;

namespace ePizza.Infrastructure.Repositories
{
    public class CartRepository : GenericRepository<CartDomain, Cart>, ICartRepository
    {
        public CartRepository(
            ePizzaDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<bool> DeleteItemFromCartAsync(Guid cartId, int itemId)
        {
            var cartItems = await _dbContext.CartItems.FirstOrDefaultAsync(x => x.CartId == cartId && x.ItemId == itemId);

            if(cartItems != null) {
                _dbContext.CartItems.Remove(cartItems);

                return await CommitAsync() > 0;
            }

            return false;
        }

        public async Task<CartDomain> GetCartDetailAsync(Guid cartId)
        {
            var result = await _dbContext.Carts
                .Where(x => x.Id == cartId && x.IsActive)
                .Include(x => x.CartItems)
                .ThenInclude(ci => ci.Item)
                .FirstOrDefaultAsync();

            return result.ToDomain();
        }

        public async Task<int> GetCartItemsCountAsnc(Guid cartId)
        {
            var itemCount = await _dbContext.CartItems.Where(x => x.CartId == cartId).CountAsync();
            return itemCount;
        }

        public async Task<int> UpdateCartUserAsync(Guid cartId, int userId)
        {
            var cart =
                await _dbContext.Carts.FirstOrDefaultAsync(x => x.Id == cartId);

            if (cart is not null)
                cart.UserId = userId;

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateItemQuantity(Guid cartId, int itemId, int quantity)
        {
            var currentItems = await _dbContext
                                            .CartItems
                                                .Where(x => x.CartId == cartId
                                                       && x.ItemId == itemId)
                                                .FirstOrDefaultAsync();

            currentItems.Quantity = quantity;
            _dbContext.Entry(currentItems).State = EntityState.Modified;
            return await CommitAsync();
        }
    }
}