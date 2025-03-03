using Library.DTOs;
using Library.Models;

namespace Library.Services
{
    public interface IBookService : ICommonServiceBase<BookDTO, BookInsertDTO, BookUpdateDTO>
    {
        Task<IEnumerable<BookSaleDTO>> GetBooksAndPrices();
        Task<IEnumerable<BookGroupDTO>> GetBooksGroupedByDiscontinued();
        Task<IEnumerable<BookDTO>> GetBooksPaginated(int start, int end);
        Task<IEnumerable<BookDTO>> GetBooksByPrice(decimal priceMin, decimal priceMax);
        Task<IEnumerable<BookDTO>> GetBooksSortedByTitle(bool up);
        Task<IEnumerable<BookDTO>> GetBooksByTitleContent(string text);
        Task<Book> GetBookById(int id);
        Task<BookDTO> Add(BookInsertDTO bookInsertDTO);
    }
}
