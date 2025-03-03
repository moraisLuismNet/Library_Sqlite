using Library.DTOs;
using Library.Models;

namespace Library.Repository
{
    public interface IAuthorRepository : IBaseRepository<Author>
    {
        Task<IEnumerable<AuthorBookDTO>> GetAuthorsWithDetails();
        Task<AuthorBookDTO?> GetAuthorBooksSelect(int id);
        Task<IEnumerable<AuthorInsertDTO>> GetAuthorsSortedByName(bool up);
        Task<IEnumerable<AuthorInsertDTO>> GetAuthorsByNameContent(string text);
        Task<IEnumerable<AuthorInsertDTO>> GetAuthorsPaginated(int from, int until);
        Task<IEnumerable<AuthorDTO>> Get();
        Task Add(AuthorInsertDTO authorInsertDTO);
        Task Update(AuthorUpdateDTO authorUpdateDTO);
        Task<Author> GetById(int id);
        void Delete(Author author);
    }
}

