using Library.DTOs;
using Library.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Repository
{
    public class AuthorRepository : IBaseRepository<Author>, IAuthorRepository
    {
        private LibraryContext _context;

        public AuthorRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Author>> Get() =>
            await _context.Authors.ToListAsync();

        public async Task<Author> GetById(int id) =>
            await _context.Authors.FindAsync(id);

        public async Task<IEnumerable<AuthorBookDTO>> GetAuthorsWithDetail()
        {
            return await _context.Authors
                .Include(a => a.Books)
                .Select(a => new AuthorBookDTO
                {
                    IdAuthor = a.IdAuthor,
                    Name = a.Name,
                    TotalBooks = a.Books.Count,
                    AveragePrices = a.Books.Any() ? a.Books.Average(l => l.Price) : 0,
                    Books = a.Books.Select(l => new BookItemDTO
                    {
                        Title = l.Title
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<AuthorBookDTO?> GetAuthorBooksSelect(int id)
        {
            return await _context.Authors
                .Where(x => x.IdAuthor == id)
                .Select(x => new AuthorBookDTO
                {
                    IdAuthor = x.IdAuthor,
                    Name = x.Name,
                    TotalBooks = x.Books.Count(),
                    AveragePrices = x.Books.Any() ? x.Books.Average(book => (decimal?)book.Price) ?? 0 : 0,
                    Books = x.Books.Select(y => new BookItemDTO
                    {
                        Title = y.Title
                    }).ToList(),
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Author>> GetAuthorsSortedByName(bool up)
        {
            if (up)
            {
                return await _context.Authors.OrderBy(x => x.Name).ToListAsync();
            }
            else
            {
                return await _context.Authors.OrderByDescending(x => x.Name).ToListAsync();
            }
        }

        public async Task<IEnumerable<Author>> GetAuthorsByNameContent(string text)
        {
            return await _context.Authors
                                 .Where(x => x.Name.Contains(text))
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Author>> GetAuthorsPaginated(int from, int until)
        {
            if (from < until)
            {
                throw new ArgumentException("The maximum cannot be less than the minimum");
            }

            return await _context.Authors
                .Skip(from - 1)
                .Take(until - from + 1)
                .ToListAsync();
        }
        public async Task Add(Author author) =>
            await _context.Authors.AddAsync(author);

        public void Update(Author author)
        {
            _context.Authors.Attach(author);
            _context.Entry(author).State = EntityState.Modified;
        }

        public void Delete(Author author) =>
           _context.Authors.Remove(author);

        public async Task Save() =>
            await _context.SaveChangesAsync();

        public IEnumerable<Author> Search(Func<Author, bool> filter) =>
        _context.Authors.AsQueryable().Where(filter).ToList();

        Task<IEnumerable<Book>> IAuthorRepository.GetAuthorsWithDetails()
        {
            throw new NotImplementedException();
        }

        public Task<bool> AuthorExists(int authorId)
        {
            throw new NotImplementedException();
        }

    }
}
