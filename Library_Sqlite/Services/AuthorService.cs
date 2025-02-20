using AutoMapper;
using Library.DTOs;
using Library.Models;
using Library.Repository;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class AuthorService : IAuthorService
    {
        private IAuthorRepository _authorRepository;
        private IMapper _mapper;
        private LibraryContext _context;
        public List<string> Errors { get; }

        public AuthorService(IAuthorRepository authorRepository,
            IMapper mapper,
            LibraryContext context)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            Errors = new List<string>();
            _context = context;
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
                var authorDTO = _mapper.Map<AuthorDTO>(author);
                return authorDTO;
            }

            return null;
        }

        public async Task<IEnumerable<AuthorBookDTO>> GetAuthorsWithDetails()
        {
            return await _context.Authors
                .Include(a => a.Books)
                .Select(a => new AuthorBookDTO
                {
                    IdAuthor = a.IdAuthor,
                    Name = a.Name,
                    TotalBooks = a.Books.Count,
                    AveragePrices = a.Books.Any() ? a.Books.Average(l => l.Price) : 0,
                    Books = a.Books.Select(l => new BookItemDTO
                    {
                        Title = l.Title
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<AuthorBookDTO?> GetAuthorBooksSelect(int id)
        {
            return await _authorRepository.GetAuthorBooksSelect(id);
        }

        public async Task<IEnumerable<Author>> GetAuthorsSortedByName(bool up)
        {
            return await _authorRepository.GetAuthorsSortedByName(up);
        }

        public async Task<IEnumerable<Author>> GetAuthorsByNameContent(string text)
        {
            return await _authorRepository.GetAuthorsByNameContent(text);
        }

        public async Task<IEnumerable<Author>> GetAuthorsPaginated(int from, int until)
        {
            return await _authorRepository.GetAuthorsPaginated(from, until);
        }
        public async Task<AuthorDTO> Add(AuthorInsertDTO authorInsertDTO)
        {
            var author = _mapper.Map<Author>(authorInsertDTO);

            await _authorRepository.Add(author);
            await _authorRepository.Save();

            var authorDTO = _mapper.Map<AuthorDTO>(author);

            return authorDTO;
        }
        public async Task<AuthorDTO> Update(int id, AuthorUpdateDTO authorUpdateDTO)
        {
            var author = await _authorRepository.GetById(id);

            if (author != null)
            {
                author = _mapper.Map<AuthorUpdateDTO, Author>(authorUpdateDTO, author);

                _authorRepository.Update(author);
                await _authorRepository.Save();

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
            if (_authorRepository.Search(b => b.Name == authorInsertDTO.Name).Count() > 0)
            {
                Errors.Add("There is already an author with that name");
                return false;
            }
            return true;
        }

        public bool Validate(AuthorUpdateDTO authorUpdateDTO)
        {
            if (_authorRepository.Search(b => b.Name == authorUpdateDTO.Name && authorUpdateDTO.IdAuthor !=
            b.IdAuthor).Count() > 0)
            {
                Errors.Add("There is already an author with that name");
                return false;
            }
            return true;

        }

    }
}
