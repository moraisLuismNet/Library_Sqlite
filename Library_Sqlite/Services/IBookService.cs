using Library.DTOs;
using Library.Models;

namespace Library.Services
{
    public interface IBookService : ICommonServiceBase<BookDTO, BookInsertDTO, BookUpdateDTO>
    {
        Task<IEnumerable<BookSaleDTO>> GetBooksAndPrices();
        Task<IEnumerable<BookGroupDTO>> GetBooksGroupedByDiscontinued();
        Task<IEnumerable<Book>> GetBooksPaginated(int from, int until);
        Task<IEnumerable<Book>> GetBooksByPrice(decimal priceMin, decimal priceMax);
        Task<IEnumerable<Book>> GetBooksSortedByTitle(bool up);
        Task<IEnumerable<Book>> GetBooksByTitleContent(string text);
        Task<Book> GetBookById(int id);
        Task DeleteBook(Book book);
    }
}
