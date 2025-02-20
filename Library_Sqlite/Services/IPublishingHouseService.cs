using Library.DTOs;
using Library.Models;

namespace Library.Services
{
    public interface IPublishingHouseService : ICommonServiceBase<PublishingHouseDTO, PublishingHouseInsertDTO, PublishingHouseUpdateDTO>
    {
        Task<PublishingHouseBookDTO> GetPublishingHousesBooksEager(int id);
        Task<IEnumerable<PublishingHouse>> GetPublishingHousesSortedByName(bool up);
        Task<IEnumerable<PublishingHouse>> GetPublishingHousesByNameContent(string text);
        Task<IEnumerable<PublishingHouse>> GetPublishingHousesPaginated(int from, int until);
    }
}

