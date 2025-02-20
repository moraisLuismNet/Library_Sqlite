using Library.DTOs;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Repository
{
    public class BookRepository : IBaseRepository<Book>, IBookRepository
    {
        private LibraryContext _context;

        public BookRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> Get() =>
            await _context.Books.ToListAsync();

        public async Task<Book> GetById(int id) =>
            await _context.Books.FindAsync(id);

        public async Task<IEnumerable<BookSaleDTO>> GetBooksAndPrices()
        {
            return await _context.Books
                .Select(l => new BookSaleDTO
                {
                    Title = l.Title,
                    Price = l.Price
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<BookGroupDTO>> GetBooksGroupedByDiscontinued()
        {
            return await _context.Books
                .GroupBy(l => l.Discontinued)
                .Select(g => new BookGroupDTO
                {
                    Discontinued = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<Book>> GetBooksPaginated(int from, int until)
        {
            if (from < until)
            {
                throw new ArgumentException("The maximum cannot be less than the minimum");
            }

            return await _context.Books
                .Skip(from - 1)
                .Take(until - from + 1)
                .ToListAsync();
        }
        public async Task<IEnumerable<Book>> GetBooksByPrice(decimal priceMin, decimal priceMax)
        {
            return await _context.Books
                .Where(book => book.Price >= priceMin && book.Price <= priceMax)
                .ToListAsync();
        }
        public async Task<IEnumerable<Book>> GetBooksSortedByTitle(bool up)
        {
            if (up)
            {
                return await _context.Books.OrderBy(x => x.Title).ToListAsync();
            }
            else
            {
                return await _context.Books.OrderByDescending(x => x.Title).ToListAsync();
            }
        }
        public async Task<IEnumerable<Book>> GetBooksByTitleContent(string text)
        {
            return await _context.Books
                                 .Where(x => x.Title.Contains(text))
                                 .ToListAsync();
        }
        public async Task Add(Book book) =>
            await _context.Books.AddAsync(book);

        public void Delete(Book book) =>
           _context.Books.Remove(book);

        public async Task<Book> GetBookById(int id)
        {
            return await _context.Books.FindAsync(id);
        }
        public async Task DeleteBook(Book book)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task Save() =>
            await _context.SaveChangesAsync();

        public async Task<bool> BookExists(int id) =>
        await _context.Books.AnyAsync(l => l.IdBook == id);

        public async Task<bool> AuthorExists(int authorId) =>
            await _context.Authors.AnyAsync(a => a.IdAuthor == authorId);

        public async Task<bool> PublishingHouseExists(int publishingHouseId) =>
            await _context.PublishingHouses.AnyAsync(e => e.IdPublishingHouse == publishingHouseId);

        public IEnumerable<Book> Search(Func<Book, bool> filter) =>
            _context.Books.Where(filter).ToList();

        public void Update(Book book)
        {
            _context.Books.Attach(book);
            _context.Entry(book).State = EntityState.Modified;
        }

    }
}

