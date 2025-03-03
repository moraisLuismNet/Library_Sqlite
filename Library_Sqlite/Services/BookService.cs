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
        public List<string> Errors { get; }

        public BookService(IBookRepository bookRepository,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            Errors = new List<string>();
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

        public async Task<IEnumerable<BookDTO>> GetBooksPaginated(int start, int end)
        {
            return await _bookRepository.GetBooksPaginated(start, end);
        }

        public async Task<IEnumerable<BookDTO>> GetBooksByPrice(decimal priceMin, decimal priceMax)
        {
            return await _bookRepository.GetBooksByPrice(priceMin, priceMax);
        }

        public async Task<IEnumerable<BookDTO>> GetBooksSortedByTitle(bool up)
        {
            return await _bookRepository.GetBooksSortedByTitle(up);
        }

        public async Task<IEnumerable<BookDTO>> GetBooksByTitleContent(string text)
        {
            return await _bookRepository.GetBooksByTitleContent(text);
        }

        public async Task<BookDTO> Add(BookInsertDTO bookInsertDTO)
        {
            var book = _mapper.Map<Book>(bookInsertDTO);
            await _bookRepository.Add(bookInsertDTO);
            await _bookRepository.Save();
            var bookDTO = _mapper.Map<BookDTO>(book);
            return bookDTO;
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

            try
            {
                await _bookRepository.Update(bookUpdateDTO);
                await _bookRepository.Save();
                book = await _bookRepository.GetById(id);
                return _mapper.Map<BookDTO>(book);
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

        public async Task<Book> GetBookById(int id)
        {
            return await _bookRepository.GetBookById(id);
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

            await _bookRepository.DeleteBook(book);

            return _mapper.Map<BookDTO>(book);
        }

    }
}
