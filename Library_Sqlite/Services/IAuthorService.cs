using Library.DTOs;
using Library.Models;

namespace Library.Services
{
    public interface IAuthorService : ICommonServiceBase<AuthorDTO, AuthorInsertDTO, AuthorUpdateDTO>
    {
        Task<IEnumerable<AuthorBookDTO>> GetAuthorsWithDetails();
        Task<AuthorBookDTO> GetAuthorBooksSelect(int id);
        Task<IEnumerable<AuthorInsertDTO>> GetAuthorsSortedByName(bool up);
        Task<IEnumerable<AuthorInsertDTO>> GetAuthorsByNameContent(string text);
        Task<IEnumerable<AuthorInsertDTO>> GetAuthorsPaginated(int start, int end);
        Task<AuthorInsertDTO> Add(AuthorInsertDTO authorInsertDTO);
    }
}

