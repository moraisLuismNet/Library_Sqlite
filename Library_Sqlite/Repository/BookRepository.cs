using Library.DTOs;
using Library.Models;
using Library.Services;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Library.Repository
{
    public class BookRepository : IBaseRepository<Book>, IBookRepository
    {
        private LibraryContext _context;
        private readonly IManagerFiles _managerFiles;

        public BookRepository(LibraryContext context,IManagerFiles managerFiles)
        {
            _context = context;
            _managerFiles = managerFiles;
        }

        public async Task<IEnumerable<BookDTO>> Get()
        {
            var books = await (from x in _context.Books
                                select new BookDTO
                                {
                                    Isbn = x.IdBook,
                                    Title = x.Title,
                                    Pages = x.Pages,
                                    Price = x.Price,
                                    PhotoCover = x.PhotoCover,
                                    Discontinued = x.Discontinued,
                                    NameAuthor = x.Author.Name,
                                    NamePublishingHouse = x.PublishingHouse.Name
                                }).ToListAsync();
            return books;
        }

        public async Task<BookDTO?> GetById(int id)
        {
            var book = await _context.Books
                .Include(l => l.Author)
                .Include(l => l.PublishingHouse)
                .FirstOrDefaultAsync(l => l.IdBook == id);

            if (book == null)
            {
                return null;
            }

            return new BookDTO
            {
                Isbn = book.IdBook,
                Title = book.Title,
                Pages = book.Pages,
                Price = book.Price,
                PhotoCover = book.PhotoCover,
                Discontinued = book.Discontinued,
                NameAuthor = book.Author.Name,
                NamePublishingHouse = book.PublishingHouse.Name
            };
        }

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

        public async Task<IEnumerable<BookDTO>> GetBooksPaginated(int start, int end)
        {
            if (end < start)
            {
                throw new ArgumentException("The maximum cannot be less than the minimum");
            }

            var books = await (from x in _context.Books
                .Skip(start - 1)
                .Take(end - start + 1)
                select new BookDTO
                {
                    Isbn = x.IdBook,
                    Title = x.Title,
                    Pages = x.Pages,
                    Price = x.Price,
                    PhotoCover = x.PhotoCover,
                    Discontinued = x.Discontinued,
                    NameAuthor = x.Author.Name,
                    NamePublishingHouse = x.PublishingHouse.Name
                })
                .ToListAsync();
            return books;
        }
        public async Task<IEnumerable<BookDTO>> GetBooksByPrice(decimal priceMin, decimal priceMax)
        {
            var books = await (from x in _context.Books
               .Where(book => book.Price >= priceMin && book.Price <= priceMax)
               select new BookDTO
               {
                    Isbn = x.IdBook,
                    Title = x.Title,
                    Pages = x.Pages,
                    Price = x.Price,
                    PhotoCover = x.PhotoCover,
                    Discontinued = x.Discontinued,
                    NameAuthor = x.Author.Name,
                    NamePublishingHouse = x.PublishingHouse.Name
               })
                .ToListAsync();
            return books;
        }
        public async Task<IEnumerable<BookDTO>> GetBooksSortedByTitle(bool up)
        {
            IQueryable<Book> query = _context.Books.Include(x => x.Author).Include(x => x.PublishingHouse);
            if (up)
            {
                query = query.OrderBy(x => x.Title);
            }
            else
            {
                query = query.OrderByDescending(x => x.Title);
            }

            var books = await query
                .Select(x => new BookDTO
                {
                    Isbn = x.IdBook,
                    Title = x.Title,
                    Pages = x.Pages,
                    Price = x.Price,
                    PhotoCover = x.PhotoCover,
                    Discontinued = x.Discontinued,
                    NameAuthor = x.Author.Name,
                    NamePublishingHouse = x.PublishingHouse.Name
                })
                .ToListAsync();

            return books;
        }
        public async Task<IEnumerable<BookDTO>> GetBooksByTitleContent(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new List<BookDTO>();
            }

            var books = await (from x in _context.Books
                                where x.Title.Contains(text)
                                select new BookDTO
                                {
                                    Isbn = x.IdBook,
                                    Title = x.Title,
                                    Pages = x.Pages,
                                    Price = x.Price,
                                    PhotoCover = x.PhotoCover,
                                    Discontinued = x.Discontinued,
                                    NameAuthor = x.Author.Name,
                                    NamePublishingHouse = x.PublishingHouse.Name
                                }).ToListAsync();

            return books;
        }
        public async Task Add(BookInsertDTO bookInsertDTO)
        {
            var book = new Book
            {
                Title = bookInsertDTO.Title,
                Pages = bookInsertDTO.Pages,
                Price = bookInsertDTO.Price,
                Discontinued = bookInsertDTO.Discontinued,
                PhotoCover = "",
                PublishingHouseId = (int)bookInsertDTO.PublishingHouseId,
                AuthorId = (int)bookInsertDTO.AuthorId,
            };

            if (bookInsertDTO.Photo != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await bookInsertDTO.Photo.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(bookInsertDTO.Photo.FileName);
                    book.PhotoCover = await _managerFiles.SaveFile(content, extension, "img", bookInsertDTO.Photo.ContentType);
                }
            }

            await _context.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task Update(BookUpdateDTO bookUpdateDTO)
        {

            var book = await _context.Books
                .AsTracking()
                .FirstOrDefaultAsync(e => e.IdBook == bookUpdateDTO.IdBook);

            if (book == null)
            {
                throw new KeyNotFoundException("The book was not found");
            }

            book.Title = bookUpdateDTO.Title;
            book.Pages = bookUpdateDTO.Pages;
            book.Price = bookUpdateDTO.Price;
            book.Discontinued = bookUpdateDTO.Discontinued;
            book.PublishingHouseId = (int)bookUpdateDTO.PublishingHouseId;
            book.AuthorId = (int)bookUpdateDTO.AuthorId;

            if (bookUpdateDTO.Photo != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await bookUpdateDTO.Photo.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(bookUpdateDTO.Photo.FileName);
                    book.PhotoCover = await _managerFiles.SaveFile(content, extension, "img", bookUpdateDTO.Photo.ContentType);
                }
            }

            await _context.SaveChangesAsync();
        }
        
        public async Task<Book> GetBookById(int id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task DeleteBook(BookDTO bookDTO)
        {
            var book = await _context.Books.FindAsync(bookDTO.Isbn);
            if (book == null)
            {
                throw new KeyNotFoundException("The book was not found");
            }
            _context.Books.Remove(book);
            await _managerFiles.DeleteFile(book.PhotoCover, "img");
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

        public async Task<IEnumerable<Book>> GetBooks()
        {
            return await _context.Books.ToListAsync();
        }

    }
}

