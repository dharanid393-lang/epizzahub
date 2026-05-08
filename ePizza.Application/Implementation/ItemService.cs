using AutoMapper;
using ePizza.Application.Contracts;
using ePizza.Application.DTOs.Response;
using ePizza.Application.Exceptions;
using ePizza.Domain.Interfaces;

namespace ePizza.Application.Implementation
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;
         
        public ItemService(
            IItemRepository itemRepository,
            IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        public async Task<ItemResponseDto> GetItemByIdAsync(int itemId)
        {
            var itemResponse = await _itemRepository.GetByIdAsync(itemId);

            if (itemResponse == null)
                throw new RecordNotFoundException($"Item with Id {itemId} doesn't exist in database");
            ///same section has been implemented in middleware for global exception handling

            var result = _mapper.Map<ItemResponseDto>(itemResponse);

            return result;
        }

        public async Task<IEnumerable<ItemResponseDto>> GetItemsAsync()
        {
            var itemResponse = await _itemRepository.GetAllAsync();

            var result = _mapper.Map<IEnumerable<ItemResponseDto>>(itemResponse);

            return result;
        }
    }
}
