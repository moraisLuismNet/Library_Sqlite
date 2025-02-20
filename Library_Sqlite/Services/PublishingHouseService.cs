using AutoMapper;
using Library.DTOs;
using Library.Models;
using Library.Repository;

namespace Library.Services
{
    public class PublishingHouseService : IPublishingHouseService
    {
        private IPublishingHouseRepository _publishingHouseRepository;
        private IMapper _mapper;
        public List<string> Errors { get; }

        public PublishingHouseService(IPublishingHouseRepository publishingHouseRepository,
            IMapper mapper)
        {
            _publishingHouseRepository = publishingHouseRepository;
            _mapper = mapper;
            Errors = new List<string>();

        }

        public async Task<IEnumerable<PublishingHouseDTO>> Get()
        {
            var publishingHouses = await _publishingHouseRepository.Get();
            return publishingHouses.Select(publishingHouse => _mapper.Map<PublishingHouseDTO>(publishingHouse));
        }

        public async Task<PublishingHouseDTO> GetById(int id)
        {
            var publishingHouse = await _publishingHouseRepository.GetById(id);

            if (publishingHouse != null)
            {
                var publishingHouseDTO = _mapper.Map<PublishingHouseDTO>(publishingHouse);
                return publishingHouseDTO;
            }

            return null;
        }

        public async Task<PublishingHouseBookDTO?> GetPublishingHousesBooksEager(int id)
        {
            var publishingHouse = await _publishingHouseRepository.GetPublishingHousesBooksEager(id);
            return publishingHouse;
        }

        public async Task<IEnumerable<PublishingHouse>> GetPublishingHousesSortedByName(bool up)
        {
            return await _publishingHouseRepository.GetPublishingHousesSortedByName(up);
        }

        public async Task<IEnumerable<PublishingHouse>> GetPublishingHousesByNameContent(string text)
        {
            return await _publishingHouseRepository.GetPublishingHousesByNameContent(text);
        }

        public async Task<IEnumerable<PublishingHouse>> GetPublishingHousesPaginated(int from, int until)
        {
            return await _publishingHouseRepository.GetPublishingHousesPaginated(from, until);
        }
        public async Task<PublishingHouseDTO> Add(PublishingHouseInsertDTO publishingHouseInsertDTO)
        {
            var publishingHouse = _mapper.Map<PublishingHouse>(publishingHouseInsertDTO);

            await _publishingHouseRepository.Add(publishingHouse);
            await _publishingHouseRepository.Save();

            var publishingHouseDTO = _mapper.Map<PublishingHouseDTO>(publishingHouse);

            return publishingHouseDTO;
        }
        public async Task<PublishingHouseDTO> Update(int id, PublishingHouseUpdateDTO publishingHouseUpdateDTO)
        {
            var publishingHouse = await _publishingHouseRepository.GetById(id);

            if (publishingHouse != null)
            {
                publishingHouse = _mapper.Map<PublishingHouseUpdateDTO, PublishingHouse>(publishingHouseUpdateDTO, publishingHouse);

                _publishingHouseRepository.Update(publishingHouse);
                await _publishingHouseRepository.Save();

                var publishingHouseDTO = _mapper.Map<PublishingHouseDTO>(publishingHouse);

                return publishingHouseDTO;
            }
            return null;
        }

        public async Task<PublishingHouseDTO> Delete(int id)
        {
            var publishingHouse = await _publishingHouseRepository.GetById(id);

            if (publishingHouse != null)
            {
                var publishingHouseDTO = _mapper.Map<PublishingHouseDTO>(publishingHouse);

                _publishingHouseRepository.Delete(publishingHouse);
                await _publishingHouseRepository.Save();

                return publishingHouseDTO;
            }
            return null;
        }
        public bool Validate(PublishingHouseInsertDTO publishingHouseInsertDTO)
        {
            if (_publishingHouseRepository.Search(b => b.Name == publishingHouseInsertDTO.Name).Count() > 0)
            {
                Errors.Add("There is already a publishing house with that name");
                return false;
            }
            return true;
        }

        public bool Validate(PublishingHouseUpdateDTO publishingHouseUpdateDTO)
        {
            if (_publishingHouseRepository.Search(b => b.Name == publishingHouseUpdateDTO.Name && publishingHouseUpdateDTO.IdPublishingHouse !=
            b.IdPublishingHouse).Count() > 0)
            {
                Errors.Add("There is already a publishing house with that name");
                return false;
            }
            return true;
        }

    }
}
