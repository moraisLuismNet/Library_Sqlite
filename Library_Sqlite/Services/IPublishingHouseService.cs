using Library.DTOs;
using Library.Models;

namespace Library.Services
{
    public interface IPublishingHouseService : ICommonServiceBase<PublishingHouseDTO, PublishingHouseInsertDTO, PublishingHouseUpdateDTO>
    {
        Task<PublishingHouseBookDTO> GetPublishingHouseBooksSelect(int id);
        Task<IEnumerable<PublishingHouseInsertDTO>> GetPublishingHousesSortedByName(bool up);
        Task<IEnumerable<PublishingHouseInsertDTO>> GetPublishingHousesByNameContent(string text);
        Task<IEnumerable<PublishingHouseInsertDTO>> GetPublishingHousesPaginated(int start, int end);
        Task<PublishingHouseInsertDTO> Add(PublishingHouseInsertDTO publishingHouseInsertDTO);
    }
}

