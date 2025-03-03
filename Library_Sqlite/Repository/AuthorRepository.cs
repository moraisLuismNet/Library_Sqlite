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

        public async Task<IEnumerable<AuthorDTO>> Get()
        {
            var authors = await (from x in _context.Authors
                                 select new AuthorDTO
                                 {
                                     IdAuthor = x.IdAuthor,
                                     NameAuthor = x.Name,
                                     TotalBooks = x.Books.Count()
                                 }).ToListAsync();
            return authors;
        }

        public async Task<Author> GetById(int id)
        {
            return await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.IdAuthor == id);
        }

        public async Task<IEnumerable<AuthorBookDTO>> GetAuthorsWithDetails()
        {
            return await _context.Authors
                .Include(a => a.Books)
                .Select(a => new AuthorBookDTO
                {
                    IdAuthor = a.IdAuthor,
                    NameAuthor = a.Name,
                    TotalBooks = a.Books.Count,
                    AveragePrices = a.Books.Any() ? a.Books.Average(l => l.Price) : 0,
                    Books = a.Books.Select(l => new BookItemDTO
                    {
                        IdBook = l.IdBook,
                        Title = l.Title,
                        Price = l.Price
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
                    NameAuthor = x.Name,
                    TotalBooks = x.Books.Count(),
                    AveragePrices = x.Books.Any() ? x.Books.Average(book => (decimal?)book.Price) ?? 0 : 0,
                    Books = x.Books.Select(y => new BookItemDTO
                    {
                        IdBook = y.IdBook,
                        Title = y.Title,
                        Price = y.Price
                    }).ToList(),
                })
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AuthorInsertDTO>> GetAuthorsSortedByName(bool up)
        {
            if (up)
            {
                return await _context.Authors
                    .OrderBy(x => x.Name)
                    .Select(a => new AuthorInsertDTO { NameAuthor = a.Name })
                    .ToListAsync();
            }

            else
            {
                return await _context.Authors
                    .OrderByDescending(x => x.Name)
                    .Select(a => new AuthorInsertDTO { NameAuthor = a.Name })
                    .ToListAsync();

            }
        }

        public async Task<IEnumerable<AuthorInsertDTO>> GetAuthorsByNameContent(string text)
        {
            return await _context.Authors
                                 .Where(x => x.Name.Contains(text))
                                 .Select(a => new AuthorInsertDTO { NameAuthor = a.Name })
                                 .ToListAsync();
        }

        public async Task<IEnumerable<AuthorInsertDTO>> GetAuthorsPaginated(int from, int until)
        {
            if (from < until)
            {
                throw new ArgumentException("The maximum cannot be less than the minimum");
            }

            return await _context.Authors
                .Skip(from - 1)
                .Take(until - from + 1)
                .Select(a => new AuthorInsertDTO { NameAuthor = a.Name })
                .ToListAsync();
        }
        public async Task Add(AuthorInsertDTO authorInsertDTO)
        {
            var author = new Author
            {
                Name = authorInsertDTO.NameAuthor
            };

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
        }
        public async Task Update(AuthorUpdateDTO authorUpdateDTO)
        {
            var author = await _context.Authors
                .AsTracking()
                .FirstOrDefaultAsync(e => e.IdAuthor == authorUpdateDTO.IdAuthor);

            if (author != null)
            {
                author.Name = authorUpdateDTO.NameAuthor;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Author not found");
            }
        }

        public void Delete(Author author) =>
           _context.Authors.Remove(author);


        public async Task Save() =>
            await _context.SaveChangesAsync();


        public IEnumerable<Author> Search(Func<Author, bool> filter) =>
            _context.Authors.AsQueryable().Where(filter).ToList();

    }
}
