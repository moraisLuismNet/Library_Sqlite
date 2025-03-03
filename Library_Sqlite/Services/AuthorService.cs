using AutoMapper;
using Library.DTOs;
using Library.Models;
using Library.Repository;

namespace Library.Services
{
    public class AuthorService : IAuthorService
    {
        private IAuthorRepository _authorRepository;
        private IMapper _mapper;
        public List<string> Errors { get; }

        public AuthorService(IAuthorRepository authorRepository,
            IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            Errors = new List<string>();
        }

        public async Task<IEnumerable<AuthorDTO>> Get()
        {
            var authors = await _authorRepository.Get();
            return authors.Select(author => _mapper.Map<AuthorDTO>(author));
        }

        public async Task<AuthorDTO> GetById(int id)
        {
            var author = await _authorRepository.GetById(id);

            if (author != null)
            {
                return new AuthorDTO
                {
                    IdAuthor = author.IdAuthor,
                    NameAuthor = author.Name,
                    TotalBooks = author.Books.Count
                };
            }

            return null;
        }

        public async Task<IEnumerable<AuthorBookDTO>> GetAuthorsWithDetails()
        {
            return await _authorRepository.GetAuthorsWithDetails();
        }

        public async Task<AuthorBookDTO?> GetAuthorBooksSelect(int id)
        {
            return await _authorRepository.GetAuthorBooksSelect(id);
        }

        public async Task<IEnumerable<AuthorInsertDTO>> GetAuthorsSortedByName(bool up)
        {
            return await _authorRepository.GetAuthorsSortedByName(up);
        }

        public async Task<IEnumerable<AuthorInsertDTO>> GetAuthorsByNameContent(string text)
        {
            return await _authorRepository.GetAuthorsByNameContent(text);
        }

        public async Task<IEnumerable<AuthorInsertDTO>> GetAuthorsPaginated(int start, int end)
        {
            return await _authorRepository.GetAuthorsPaginated(start, end);
        }
        public async Task<AuthorInsertDTO> Add(AuthorInsertDTO authorInsertDTO)
        {
            var author = _mapper.Map<Author>(authorInsertDTO);

            await _authorRepository.Add(authorInsertDTO);
            await _authorRepository.Save();

            var authorDTO = _mapper.Map<AuthorDTO>(author);

            return authorInsertDTO;
        }
        public async Task<AuthorDTO> Update(int id, AuthorUpdateDTO authorUpdateDTO)
        {
            var author = await _authorRepository.GetById(id);

            if (author != null)
            {
                author = _mapper.Map<AuthorUpdateDTO, Author>(authorUpdateDTO, author);

                await _authorRepository.Update(authorUpdateDTO);

                var authorDTO = _mapper.Map<AuthorDTO>(author);

                return authorDTO;
            }
            return null;
        }

        public async Task<AuthorDTO> Delete(int id)
        {
            var author = await _authorRepository.GetById(id);

            if (author != null)
            {
                var authorDTO = _mapper.Map<AuthorDTO>(author);

                _authorRepository.Delete(author);
                await _authorRepository.Save();

                return authorDTO;
            }
            return null;
        }
        public bool Validate(AuthorInsertDTO authorInsertDTO)
        {
            if (_authorRepository.Search(b => b.Name == authorInsertDTO.NameAuthor).Count() > 0)
            {
                Errors.Add("There is already an author with that name");
                return false;
            }
            return true;
        }

        public bool Validate(AuthorUpdateDTO authorUpdateDTO)
        {
            if (_authorRepository.Search(b => b.Name == authorUpdateDTO.NameAuthor && authorUpdateDTO.IdAuthor !=
            b.IdAuthor).Count() > 0)
            {
                Errors.Add("There is already an author with that name");
                return false;
            }
            return true;
        }

    }
}
