using Library.DTOs;
using Library.Models;

namespace Library.Repository
{
    public interface IPublishingHouseRepository : IBaseRepository<PublishingHouse>
    {
        Task<PublishingHouseBookDTO?> GetPublishingHouseBooksSelect(int id);
        Task<IEnumerable<PublishingHouseInsertDTO>> GetPublishingHousesSortedByName(bool up);
        Task<IEnumerable<PublishingHouseInsertDTO>> GetPublishingHousesByNameContent(string text);
        Task<IEnumerable<PublishingHouseInsertDTO>> GetPublishingHousesPaginated(int from, int until);
        Task<IEnumerable<PublishingHouseDTO>> Get();
        Task Add(PublishingHouseInsertDTO publishingHouseInsertDTO);
        Task Update(PublishingHouseUpdateDTO publishingHouseUpdateDTO);
        Task<PublishingHouse> GetById(int id);
        void Delete(PublishingHouse publishingHouse);
    }
}

