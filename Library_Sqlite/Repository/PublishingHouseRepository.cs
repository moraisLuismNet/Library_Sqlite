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

        public async Task<IEnumerable<PublishingHouseDTO>> Get()
        {
            var publishingHouses = await (from x in _context.PublishingHouses
                                          select new PublishingHouseDTO
                                          {
                                            IdPublishingHouse = x.IdPublishingHouse,
                                            NamePublishingHouse = x.Name,
                                            TotalBooks = x.Books.Count()
                                          }).ToListAsync();
            return publishingHouses;
        }

        public async Task<PublishingHouse> GetById(int id)
        {
            return await _context.PublishingHouses
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.IdPublishingHouse == id);
        }

        public async Task<IEnumerable<PublishingHouseInsertDTO>> GetPublishingHousesSortedByName(bool up)
        {
            if (up)
            {
                return await _context.PublishingHouses
                    .OrderBy(x => x.Name)
                    .Select(a => new PublishingHouseInsertDTO { NamePublishingHouse = a.Name })
                    .ToListAsync();
            }

            else
            {
                return await _context.PublishingHouses
                    .OrderByDescending(x => x.Name)
                    .Select(a => new PublishingHouseInsertDTO { NamePublishingHouse = a.Name })
                    .ToListAsync();

            }
        }

        public async Task<PublishingHouseBookDTO?> GetPublishingHouseBooksSelect(int id)
        {
            return await _context.PublishingHouses
                .Where(x => x.IdPublishingHouse == id)
                .Select(x => new PublishingHouseBookDTO
                {
                    IdPublishingHouse = x.IdPublishingHouse,
                    NamePublishingHouse = x.Name,
                    TotalBooks = x.Books.Count(),
                    Books = x.Books.Select(y => new BookItemDTO
                    {
                        IdBook = y.IdBook,
                        Title = y.Title,
                        Price = y.Price
                    }).ToList(),
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PublishingHouseInsertDTO>> GetPublishingHousesByNameContent(string text)
        {
            return await _context.PublishingHouses
                                 .Where(x => x.Name.Contains(text))
                                 .Select(a => new PublishingHouseInsertDTO { NamePublishingHouse = a.Name })
                                 .ToListAsync();
        }

        public async Task<IEnumerable<PublishingHouseInsertDTO>> GetPublishingHousesPaginated(int from, int until)
        {
            if (from < until)
            {
                throw new ArgumentException("The maximum cannot be less than the minimum");
            }

            return await _context.PublishingHouses
                .Skip(from - 1)
                .Take(until - from + 1)
                .Select(a => new PublishingHouseInsertDTO { NamePublishingHouse = a.Name })
                .ToListAsync();
        }

        public async Task Add(PublishingHouseInsertDTO publishingHouseInsertDTO)
        {
            var publishingHouse = new PublishingHouse
            {
                Name = publishingHouseInsertDTO.NamePublishingHouse
            };

            _context.PublishingHouses.Add(publishingHouse);
            await _context.SaveChangesAsync();
        }

        public async Task Update(PublishingHouseUpdateDTO publishingHouseUpdateDTO)
        {
            var publishingHouse = await _context.PublishingHouses
                 .AsTracking()
                 .FirstOrDefaultAsync(e => e.IdPublishingHouse == publishingHouseUpdateDTO.IdPublishingHouse);

            if (publishingHouse != null)
            {
                publishingHouse.Name = publishingHouseUpdateDTO.NamePublishingHouse;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Publishing House not found");
            }
        }

        public void Delete(PublishingHouse publishingHouse) =>
           _context.PublishingHouses.Remove(publishingHouse);

        public async Task Save() =>
            await _context.SaveChangesAsync();

        public IEnumerable<PublishingHouse> Search(Func<PublishingHouse, bool> filter) =>
        _context.PublishingHouses.Where(filter).ToList();

        public async Task<IEnumerable<PublishingHouse>> GetAuthorsWithDetails()
        {
            return await _context.PublishingHouses.Include(e => e.Books).ToListAsync();
        }

    }
}

