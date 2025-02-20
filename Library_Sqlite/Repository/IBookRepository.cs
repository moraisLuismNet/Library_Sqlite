using Library.DTOs;
using Library.Models;

namespace Library.Repository
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        Task<bool> BookExists(int id);
        Task<IEnumerable<BookSaleDTO>> GetBooksAndPrices();
        Task<IEnumerable<BookGroupDTO>> GetBooksGroupedByDiscontinued();
        Task<IEnumerable<Book>> GetBooksPaginated(int from, int until);
        Task<IEnumerable<Book>> GetBooksByPrice(decimal priceMin, decimal priceMax);
        Task<IEnumerable<Book>> GetBooksSortedByTitle(bool up);
        Task<IEnumerable<Book>> GetBooksByTitleContent(string text);
        Task<Book> GetBookById(int id);
        Task DeleteBook(Book book);
        Task<bool> AuthorExists(int authorId);
        Task<bool> PublishingHouseExists(int publishingHouseId);
    }
}

