using Library.DTOs;
using Library.Models;

namespace Library.Repository
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        Task<bool> BookExists(int id);
        Task<IEnumerable<BookSaleDTO>> GetBooksAndPrices();
        Task<IEnumerable<BookGroupDTO>> GetBooksGroupedByDiscontinued();
        Task<IEnumerable<BookDTO>> GetBooksPaginated(int from, int until);
        Task<IEnumerable<BookDTO>> GetBooksByPrice(decimal priceMin, decimal priceMax);
        Task<IEnumerable<BookDTO>> GetBooksSortedByTitle(bool up);
        Task<IEnumerable<BookDTO>> GetBooksByTitleContent(string text);
        Task<Book> GetBookById(int id);
        Task<bool> AuthorExists(int authorId);
        Task<bool> PublishingHouseExists(int publishingHouseId);
        Task<IEnumerable<BookDTO>> Get();
        Task<BookDTO> GetById(int id);
        Task Add(BookInsertDTO bookInsertDTO);
        Task Update(BookUpdateDTO bookUpdateDTO);
        Task DeleteBook(BookDTO book);
    }
}

