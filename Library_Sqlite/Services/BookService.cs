using AutoMapper;
using Library.DTOs;
using Library.Models;
using Library.Repository;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class BookService : IBookService
    {
        private IBookRepository _bookRepository;
        private IMapper _mapper;
        private readonly IManagerFiles _fileManager;
        public List<string> Errors { get; }

        public BookService(IBookRepository bookRepository,
            IMapper mapper, IManagerFiles fileManager)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            Errors = new List<string>();
            _fileManager = fileManager;
        }

        public async Task<IEnumerable<BookDTO>> Get()
        {
            var books = await _bookRepository.Get();
            return books.Select(book => _mapper.Map<BookDTO>(book));
        }

        public async Task<BookDTO> GetById(int id)
        {
            var book = await _bookRepository.GetById(id);

            if (book != null)
            {
                var bookDTO = _mapper.Map<BookDTO>(book);
                return bookDTO;
            }

            return null;
        }

        public async Task<IEnumerable<BookSaleDTO>> GetBooksAndPrices()
        {
            return await _bookRepository.GetBooksAndPrices();
        }

        public async Task<IEnumerable<BookGroupDTO>> GetBooksGroupedByDiscontinued()
        {
            return await _bookRepository.GetBooksGroupedByDiscontinued();
        }

        public async Task<IEnumerable<Book>> GetBooksPaginated(int from, int until)
        {
            return await _bookRepository.GetBooksPaginated(from, until);
        }

        public async Task<IEnumerable<Book>> GetBooksByPrice(decimal priceMin, decimal priceMax)
        {
            return await _bookRepository.GetBooksByPrice(priceMin, priceMax);
        }

        public async Task<IEnumerable<Book>> GetBooksSortedByTitle(bool up)
        {
            return await _bookRepository.GetBooksSortedByTitle(up);
        }

        public async Task<IEnumerable<Book>> GetBooksByTitleContent(string text)
        {
            return await _bookRepository.GetBooksByTitleContent(text);
        }

        public async Task<BookDTO> Add(BookInsertDTO bookInsertDTO)
        {
            var book = _mapper.Map<Book>(bookInsertDTO);

            if (bookInsertDTO.Photo != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await bookInsertDTO.Photo.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(bookInsertDTO.Photo.FileName);
                    book.PhotoCover = await _fileManager.SaveFile(content, extension, "img", bookInsertDTO.Photo.ContentType);
                }
            }

            await _bookRepository.Add(book);
            await _bookRepository.Save();

            return _mapper.Map<BookDTO>(book);
        }

        public async Task<BookDTO> Update(int id, BookUpdateDTO bookUpdateDTO)
        {
            var book = await _bookRepository.GetById(id);



            if (book == null)
            {
                Errors.Add($"Book with ISBN {id} not found");
                return null;
            }

            if (!await _bookRepository.AuthorExists(bookUpdateDTO.AuthorId) || !await _bookRepository.PublishingHouseExists(bookUpdateDTO.PublishingHouseId))
            {
                Errors.Add("Author or publishing house does not exist");
                return null;
            }

            book.Title = bookUpdateDTO.Title;
            book.Pages = bookUpdateDTO.Pages;
            book.Price = bookUpdateDTO.Price;
            book.Discontinued = bookUpdateDTO.Discontinued;
            book.AuthorId = bookUpdateDTO.AuthorId;
            book.PublishingHouseId = bookUpdateDTO.PublishingHouseId;

            if (bookUpdateDTO.Photo != null)
            {
                await UpdatePhotoCover(book, bookUpdateDTO.Photo);

            }
            else if (!string.IsNullOrEmpty(book.PhotoCover))
            {
                await _fileManager.DeleteFile(book.PhotoCover, "img");
                book.PhotoCover = null;
            }

            try
            {
                _bookRepository.Update(book);
                await _bookRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _bookRepository.BookExists(id))
                {
                    Errors.Add($"Book with ISBN {id} not found");
                    return null;
                }
                throw;
            }
            return _mapper.Map<BookDTO>(book);
        }

        public bool Validate(BookUpdateDTO bookUpdateDTO)
        {
            if (_bookRepository.Search(l => l.Title == bookUpdateDTO.Title && l.IdBook != bookUpdateDTO.IdBook).Any())
            {
                Errors.Add("There is already a book with that title");
                return false;
            }
            return true;
        }

        private async Task UpdatePhotoCover(Book book, IFormFile photo)
        {
            if (!string.IsNullOrEmpty(book.PhotoCover))
            {
                await _fileManager.DeleteFile(book.PhotoCover, "img");
            }

            using (var memoryStream = new MemoryStream())
            {
                await photo.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                var extension = Path.GetExtension(photo.FileName);
                book.PhotoCover = await _fileManager.SaveFile(content, extension, "img", photo.ContentType);
            }
        }

        public async Task<Book> GetBookById(int id)
        {
            return await _bookRepository.GetBookById(id);
        }

        public async Task DeleteBook(Book book)
        {
            await _bookRepository.DeleteBook(book);
        }

        public bool Validate(BookInsertDTO bookInsertDTO)
        {
            if (_bookRepository.Search(b => b.Title == bookInsertDTO.Title).Count() > 0)
            {
                Errors.Add("There is already a book with that title");
                return false;
            }
            return true;
        }

        public async Task<BookDTO?> Delete(int id)
        {
            var book = await _bookRepository.GetById(id);
            if (book == null)
            {
                return null;
            }

            await _fileManager.DeleteFile(book.PhotoCover, "img");

            await _bookRepository.DeleteBook(book);

            return _mapper.Map<BookDTO>(book);
        }

    }
}
