using Library.DTOs;
using Library.Models;
using Library.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private IValidator<AuthorInsertDTO> _authorInsertValidator;
        private IValidator<AuthorUpdateDTO> _authorUpdateValidator;
        private IAuthorService _authorService;
        private readonly ActionsService _actionsService;
        public AuthorsController(
            IValidator<AuthorInsertDTO> authorInsertValidator,
            IValidator<AuthorUpdateDTO> authorUpdateValidator,
            IAuthorService authorService,
            ActionsService actionsService)
        {
            _authorInsertValidator = authorInsertValidator;
            _authorUpdateValidator = authorUpdateValidator;
            _authorService = authorService;
            _actionsService = actionsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDTO>>> Get()
        {
            await _actionsService.AddAction("Get authors", "Authors");
            var authors = await _authorService.Get();
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDTO>> GetById(int id)
        {
            await _actionsService.AddAction("Get author by Id", "Authors");
            var authorDTO = await _authorService.GetById(id);
            return authorDTO == null ? NotFound() : Ok(authorDTO);
        }

        [HttpGet("details")]
        public async Task<ActionResult<IEnumerable<AuthorBookDTO>>> GetAuthorsWithDetails()
        {
            await _actionsService.AddAction("Get authors with details", "Authors");
            var authorsDto = await _authorService.GetAuthorsWithDetails();

            if (authorsDto == null || !authorsDto.Any())
            {
                return NotFound("No authors with details were found");
            }

            return Ok(authorsDto);
        }

        [HttpGet("authorBooksSelect")]
        public async Task<ActionResult<AuthorBookDTO>> GetAuthorBooksSelect(int id)
        {
            await _actionsService.AddAction("Get author with details of his books", "Autores");
            var author = await _authorService.GetAuthorBooksSelect(id);

            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        [HttpGet("sortedName/{up}")]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthorsSortedByName(bool up)
        {
            await _actionsService.AddAction("Get authors sorted by name", "Authors");
            var authors = await _authorService.GetAuthorsSortedByName(up);

            if (!authors.Any())
            {
                return NotFound("No authors found");
            }

            return Ok(authors);
        }

        [HttpGet("name/contain/{text}")]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthorsByNameContain(string text)
        {
            await _actionsService.AddAction("Get authors with the name it contains", "Authors");
            if (string.IsNullOrEmpty(text))
            {
                return BadRequest("The search text cannot be empty");
            }

            var authors = await _authorService.GetAuthorsByNameContent(text);

            if (!authors.Any())
            {
                return NotFound("No authors were found containing the specified text");
            }

            return Ok(authors);
        }

        [HttpGet("pagination/{from}/{until}")]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthorsPaginated(int from, int until)
        {
            await _actionsService.AddAction("Get authors paginated", "Authors");
            if (from < until)
            {
                return BadRequest("The maximum cannot be less than the minimum");
            }

            var authors = await _authorService.GetAuthorsPaginated(from, until);
            return Ok(authors);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<AuthorDTO>> Add(AuthorInsertDTO authorInsertDTO)
        {
            var validationResult = await _authorInsertValidator.ValidateAsync(authorInsertDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_authorService.Validate(authorInsertDTO))
            {
                return BadRequest(_authorService.Errors);
            }

            var authorDTO = await _authorService.Add(authorInsertDTO);

            return CreatedAtAction(nameof(GetById), new { id = authorDTO.IdAuthor }, authorDTO);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<AuthorDTO>> Update(int id, AuthorUpdateDTO authorUpdateDTO)
        {
            var validationResult = await _authorUpdateValidator.ValidateAsync(authorUpdateDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_authorService.Validate(authorUpdateDTO))
            {
                return BadRequest(_authorService.Errors);
            }

            var authorDTO = await _authorService.Update(id, authorUpdateDTO);

            return authorDTO == null ? NotFound() : Ok(authorDTO);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<AuthorDTO>> Delete(int id)
        {
            var authorDTO = await _authorService.Delete(id);
            return authorDTO == null ? NotFound($"Author with ID {id} not found") : Ok(authorDTO);
        }
    }
}

