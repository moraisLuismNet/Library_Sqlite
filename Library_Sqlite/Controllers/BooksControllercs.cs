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
    public class BooksController : ControllerBase
    {
        private IValidator<BookInsertDTO> _bookInsertValidator;
        private IValidator<BookUpdateDTO> _bookUpdateValidator;
        private IBookService _bookService;
        private readonly ActionsService _actionsService;

        public BooksController(IValidator<BookInsertDTO> bookInsertValidator,
            IValidator<BookUpdateDTO> bookUpdateValidator,
            IBookService bookService,
            ActionsService actionsService)
        {
            _bookInsertValidator = bookInsertValidator;
            _bookUpdateValidator = bookUpdateValidator;
            _bookService = bookService;
            _actionsService = actionsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> Get()
        {
            await _actionsService.AddAction("Get books", "Books");
            var books = await _bookService.Get();
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookDTO>> GetById(int id)
        {
            await _actionsService.AddAction("Get book by Id", "Books");
            var bookDTO = await _bookService.GetById(id);
            return bookDTO == null ? NotFound() : Ok(bookDTO);
        }

        [HttpGet("sale")]
        public async Task<ActionResult<IEnumerable<BookSaleDTO>>> GetBooksAndPrices()
        {
            await _actionsService.AddAction("Get books and prices", "Books");
            var books = await _bookService.GetBooksAndPrices();

            return Ok(books);
        }

        /* Get all books grouped by their Out of Print property. For each group, get how many are
	    out of print and how many are not out of print */
        [HttpGet("groupByDiscontinued")]
        public async Task<ActionResult<IEnumerable<BookGroupDTO>>> GetBooksGroupedByDiscontinued()
        {
            await _actionsService.AddAction("Get books grouped by their property Discontinued", "Books");
            var groupedData = await _bookService.GetBooksGroupedByDiscontinued();
            return Ok(groupedData);
        }

        [HttpGet("pagination/{from}/{until}")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooksPaginated(int from, int until)
        {
            await _actionsService.AddAction("Get books paginated", "Books");
            if (from < until)
            {
                return BadRequest("The maximum cannot be less than the minimum");
            }

            var books = await _bookService.GetBooksPaginated(from, until);
            return Ok(books);
        }

        [HttpGet("price/{min}/{max}")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooksByPrice(decimal min, decimal max)
        {
            await _actionsService.AddAction("Get books with a certain price", "Books");
            if (min > max)
            {
                return BadRequest("Minimum price cannot be higher than the maximum price");
            }

            var books = await _bookService.GetBooksByPrice(min, max);

            if (!books.Any())
            {
                return NotFound("No books were found in the specified price range");
            }

            return Ok(books);
        }

        [HttpGet("SortedByTitle/{up}")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooksSortedByTitle(bool up)
        {
            await _actionsService.AddAction("Get books sorted by title", "Books");
            var books = await _bookService.GetBooksSortedByTitle(up);

            if (!books.Any())
            {
                return NotFound("No books found");
            }

            return Ok(books);
        }

        [HttpGet("title/contain/{text}")]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooksByTitleContain(string text)
        {
            await _actionsService.AddAction("Get books with the title that contains", "Books");
            if (string.IsNullOrEmpty(text))
            {
                return BadRequest("Search text cannot be empty");
            }

            var books = await _bookService.GetBooksByTitleContent(text);

            if (!books.Any())
            {
                return NotFound("No books were found containing the specified text");
            }

            return Ok(books);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BookDTO>> Add([FromForm] BookInsertDTO bookInsertDTO)
        {
            await _actionsService.AddAction("Add book", "Books");
            if (!_bookService.Validate(bookInsertDTO))
            {
                return BadRequest(_bookService.Errors);
            }

            var newBook = await _bookService.Add(bookInsertDTO);

            return Ok(newBook);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromForm] BookUpdateDTO dtoBook)
        {
            await _actionsService.AddAction("Update book", "Books");
            if (!_bookService.Validate(dtoBook))
            {
                return BadRequest(_bookService.Errors);
            }

            var bookUpdated = await _bookService.Update(id, dtoBook);

            if (bookUpdated == null)
            {
                return NotFound("The book was not found or there was an error");
            }

            return Ok(bookUpdated);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            await _actionsService.AddAction("Delete book", "Books");
            var result = await _bookService.Delete(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok();
        }

    }
}
