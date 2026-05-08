namespace ePizza.UI.Services.Contracts
{
    public interface ICartService
    {
        Task<int> UpdateQuantity(Guid CartId, int itemId, int quantity);
    }
}
