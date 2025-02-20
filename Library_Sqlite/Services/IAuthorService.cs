using Library.DTOs;
using Library.Models;

namespace Library.Services
{
    public interface IAuthorService : ICommonServiceBase<AuthorDTO, AuthorInsertDTO, AuthorUpdateDTO>
    {
        Task<IEnumerable<AuthorBookDTO>> GetAuthorsWithDetails();
        Task<AuthorBookDTO> GetAuthorBooksSelect(int id);
        Task<IEnumerable<Author>> GetAuthorsSortedByName(bool up);
        Task<IEnumerable<Author>> GetAuthorsByNameContent(string text);
        Task<IEnumerable<Author>> GetAuthorsPaginated(int from, int until);
    }
}

