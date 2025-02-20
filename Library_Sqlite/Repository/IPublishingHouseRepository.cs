using Library.DTOs;
using Library.Models;

namespace Library.Repository
{
    public interface IPublishingHouseRepository : IBaseRepository<PublishingHouse>
    {
        Task<PublishingHouseBookDTO?> GetPublishingHousesBooksEager(int id);
        Task<bool> PublishingHouseExists(int publishingHouseId);
        Task<IEnumerable<PublishingHouse>> GetPublishingHousesSortedByName(bool up);
        Task<IEnumerable<PublishingHouse>> GetPublishingHousesByNameContent(string text);
        Task<IEnumerable<PublishingHouse>> GetPublishingHousesPaginated(int from, int until);
    }
}

