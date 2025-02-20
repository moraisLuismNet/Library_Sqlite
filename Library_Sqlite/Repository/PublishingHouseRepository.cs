using Library.DTOs;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Repository
{
    public class PublishingHouseRepository : IBaseRepository<PublishingHouse>, IPublishingHouseRepository
    {
        private LibraryContext _context;

        public PublishingHouseRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PublishingHouse>> Get() =>
            await _context.PublishingHouses.ToListAsync();

        public async Task<PublishingHouse> GetById(int id) =>
            await _context.PublishingHouses.FindAsync(id);

        public async Task<PublishingHouseBookDTO?> GetPublishingHousesBooksEager(int id)
        {
            var publishingHouse = await _context.PublishingHouses
                .Include(e => e.Books)
                .Where(e => e.IdPublishingHouse == id)
                .Select(e => new PublishingHouseBookDTO
                {
                    IdPublishingHouse = e.IdPublishingHouse,
                    Name = e.Name,
                    Books = e.Books.Select(l => new BookItemDTO
                    {
                        Title = l.Title
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            return publishingHouse;
        }

        public async Task<IEnumerable<PublishingHouse>> GetPublishingHousesSortedByName(bool up)
        {
            if (up)
            {
                return await _context.PublishingHouses.OrderBy(x => x.Name).ToListAsync();
            }
            else
            {
                return await _context.PublishingHouses.OrderByDescending(x => x.Name).ToListAsync();
            }
        }

        public async Task<IEnumerable<PublishingHouse>> GetPublishingHousesByNameContent(string text)
        {
            return await _context.PublishingHouses
                                 .Where(x => x.Name.Contains(text))
                                 .ToListAsync();
        }

        public async Task<IEnumerable<PublishingHouse>> GetPublishingHousesPaginated(int from, int until)
        {
            if (from < until)
            {
                throw new ArgumentException("The maximum cannot be less than the minimum");
            }

            return await _context.PublishingHouses
                .Skip(from - 1)
                .Take(until - from + 1)
                .ToListAsync();
        }
        public async Task Add(PublishingHouse publishingHouse) =>
            await _context.PublishingHouses.AddAsync(publishingHouse);

        public void Update(PublishingHouse publishingHouse)
        {
            _context.PublishingHouses.Attach(publishingHouse);
            _context.Entry(publishingHouse).State = EntityState.Modified;
        }

        public void Delete(PublishingHouse publishingHouse) =>
           _context.PublishingHouses.Remove(publishingHouse);

        public async Task Save() =>
            await _context.SaveChangesAsync();

        public IEnumerable<PublishingHouse> Search(Func<PublishingHouse, bool> filter) =>
        _context.PublishingHouses.Where(filter).ToList();

        public async Task<IEnumerable<PublishingHouse>> GetAuthoresWithDetails()
        {
            return await _context.PublishingHouses.Include(e => e.Books).ToListAsync();
        }

        Task<bool> IPublishingHouseRepository.PublishingHouseExists(int publishingHouseId)
        {
            throw new NotImplementedException();
        }

    }
}

