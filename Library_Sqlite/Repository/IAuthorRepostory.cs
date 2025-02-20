using Library.DTOs;
using Library.Models;

namespace Library.Repository
{
    public interface IAuthorRepository : IBaseRepository<Author>
    {
        Task<IEnumerable<Book>> GetAuthorsWithDetails();
        Task<AuthorBookDTO?> GetAuthorBooksSelect(int id);
        Task<bool> AuthorExists(int authorId);
        Task<IEnumerable<Author>> GetAuthorsSortedByName(bool up);
        Task<IEnumerable<Author>> GetAuthorsByNameContent(string text);
        Task<IEnumerable<Author>> GetAuthorsPaginated(int from, int until);
    }
}

