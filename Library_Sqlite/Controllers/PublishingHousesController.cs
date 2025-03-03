using Library.DTOs;
using Library.Models;
using Library.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Library.Validators;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishingHousesController : ControllerBase
    {
        private IValidator<PublishingHouseInsertDTO> _publishingHouseInsertValidator;
        private IValidator<PublishingHouseUpdateDTO> _publishingHouseUpdateValidator;
        private IPublishingHouseService _publishingHouseService;
        private readonly ActionsService _actionsService;

        public PublishingHousesController(IValidator<PublishingHouseInsertDTO> publishingHouseInsertValidator,
            IValidator<PublishingHouseUpdateDTO> publishingHouseUpdateValidator,
            IPublishingHouseService publishingHouseService,
            ActionsService actionsService)
        {
            _publishingHouseInsertValidator = publishingHouseInsertValidator;
            _publishingHouseUpdateValidator = publishingHouseUpdateValidator;
            _publishingHouseService = publishingHouseService;
            _actionsService = actionsService;
        }

        [HttpGet("withTotalBooks")]
        public async Task<ActionResult> Get()
        {
            await _actionsService.AddAction("Get publishing houses with total books", "PublishingHouses");
            var publishingHouses = await _publishingHouseService.Get();
            return Ok(publishingHouses);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PublishingHouseDTO>> GetById(int id)
        {
            await _actionsService.AddAction("Get publishing house by Id", "PublishingHouses");
            var publishingHouseDTO = await _publishingHouseService.GetById(id);
            return publishingHouseDTO == null ? NotFound() : Ok(publishingHouseDTO);
        }

        [HttpGet("publishingHouseBooks/{id:int}")]
        public async Task<ActionResult<PublishingHouseBookDTO>> GetPublishingHouseBooksSelect(int id)
        {
            await _actionsService.AddAction("Get PublishingHouse with books", "PublishingHouses");
            var publishingHouse = await _publishingHouseService.GetPublishingHouseBooksSelect(id);

            if (publishingHouse == null)
            {
                return NotFound();
            }

            return Ok(publishingHouse);
        }

        [HttpGet("sortedByName/{up}")]
        public async Task<ActionResult<IEnumerable<PublishingHouseInsertDTO>>> GetPublishingHousesSortedByName(bool up)
        {
            await _actionsService.AddAction("Get publishing house sorted by name", "PublishingHouses");
            var publishingHouses = await _publishingHouseService.GetPublishingHousesSortedByName(up);

            if (!publishingHouses.Any())
            {
                return NotFound("No publishing houses found");
            }

            return Ok(publishingHouses);
        }

        [HttpGet("name/contain/{text}")]
        public async Task<ActionResult<IEnumerable<PublishingHouseInsertDTO>>> GetPublishingHousesByNameContain(string text)
        {
            await _actionsService.AddAction("Get publishing houses with the name it contains", "PublishingHouses");
            if (string.IsNullOrEmpty(text))
            {
                return BadRequest("The search text cannot be empty");
            }

            var publishingHouses = await _publishingHouseService.GetPublishingHousesByNameContent(text);

            if (!publishingHouses.Any())
            {
                return NotFound("No publishers were found containing the specified text");
            }

            return Ok(publishingHouses);
        }

        [HttpGet("pagination/{from}/{until}")]
        public async Task<ActionResult<IEnumerable<PublishingHouseInsertDTO>>> GetPublishingHousesPaginated(int from, int until)
        {
            await _actionsService.AddAction("Get publishing house paginated", "PublishingHouses");
            if (from < until)
            {
                return BadRequest("The maximum cannot be less than the minimum");
            }

            var publishingHouses = await _publishingHouseService.GetPublishingHousesPaginated(from, until);
            return Ok(publishingHouses);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<PublishingHouseInsertDTO>> Add([FromBody] PublishingHouseInsertDTO publishingHouse)
        {
            await _actionsService.AddAction("Add publishingHouse", "PublishingHouses");
            var validationResult = await _publishingHouseInsertValidator.ValidateAsync(publishingHouse);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            var newPublishingHouse = await _publishingHouseService.Add(publishingHouse);
            return CreatedAtAction(nameof(Get), new { id = newPublishingHouse.NamePublishingHouse }, newPublishingHouse);
        }

        [Authorize]
        [HttpPut("{idPublishingHouse:int}")]
        public async Task<IActionResult> Update(int idPublishingHouse, [FromBody] PublishingHouseUpdateDTO publishingHouse)
        {
            await _actionsService.AddAction("Update publishingHouse", "PublishingHouses");
            var validationResult = await _publishingHouseUpdateValidator.ValidateAsync(publishingHouse);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (idPublishingHouse != publishingHouse.IdPublishingHouse)
            {
                return BadRequest(new { message = "The route ID does not match the body ID" });
            }

            var publishingHouseUpdated = await _publishingHouseService.Update(idPublishingHouse, publishingHouse);

            if (publishingHouseUpdated != null)
            {
                return Ok(new { message = "The publishing house has been successfully updated" });
            }

            return NotFound(new { message = "The publishing house was not found" });
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<PublishingHouseDTO>> Delete(int id)
        {
            await _actionsService.AddAction("Delete publishingHouse", "PublishingHouses");
            var publishingHouseDTO = await _publishingHouseService.Delete(id);
            return publishingHouseDTO == null ? NotFound() : Ok(publishingHouseDTO);
        }

    }
}

