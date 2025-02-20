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
    public class PublishingHouseController : ControllerBase
    {
        private IValidator<PublishingHouseInsertDTO> _publishingHouseInsertValidator;
        private IValidator<PublishingHouseUpdateDTO> _publishingHouseUpdateValidator;
        private IPublishingHouseService _publishingHouseService;
        private readonly ActionsService _actionsService;

        public PublishingHouseController(IValidator<PublishingHouseInsertDTO> publishingHouseInsertValidator,
            IValidator<PublishingHouseUpdateDTO> publishingHouseUpdateValidator,
            IPublishingHouseService publishingHouseService,
            ActionsService actionsService)
        {
            _publishingHouseInsertValidator = publishingHouseInsertValidator;
            _publishingHouseUpdateValidator = publishingHouseUpdateValidator;
            _publishingHouseService = publishingHouseService;
            _actionsService = actionsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PublishingHouseDTO>>> Get()
        {
            await _actionsService.AddAction("Get publishing houses", "PublishingHouses");
            var publishingHouse = await _publishingHouseService.Get();
            return Ok(publishingHouse);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PublishingHouseDTO>> GetById(int id)
        {
            await _actionsService.AddAction("Get publishing house by Id", "PublishingHouses");
            var publishingHouseDTO = await _publishingHouseService.GetById(id);
            return publishingHouseDTO == null ? NotFound() : Ok(publishingHouseDTO);
        }

        [HttpGet("publishingHouseBooks/{id:int}")]
        public async Task<ActionResult<PublishingHouseBookDTO>> GetPublishingHousesBooksEager(int id)
        {
            await _actionsService.AddAction("Get PublishingHouse with books", "PublishingHouses");
            var publishingHouse = await _publishingHouseService.GetPublishingHousesBooksEager(id);

            if (publishingHouse == null)
            {
                return NotFound();
            }

            return Ok(publishingHouse);
        }

        [HttpGet("sortedByName/{up}")]
        public async Task<ActionResult<IEnumerable<PublishingHouse>>> GetPublishingHousesSortedByName(bool up)
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
        public async Task<ActionResult<IEnumerable<PublishingHouse>>> GetPublishingHousesByNameContain(string text)
        {
            await _actionsService.AddAction("Obtener publishing houses with the name it contains", "PublishingHouses");
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
        public async Task<ActionResult<IEnumerable<PublishingHouse>>> GetPublishingHousesPaginated(int from, int until)
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
        public async Task<ActionResult<PublishingHouseDTO>> Add(PublishingHouseInsertDTO publishingHouseInsertDTO)
        {
            var validationResult = await _publishingHouseInsertValidator.ValidateAsync(publishingHouseInsertDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_publishingHouseService.Validate(publishingHouseInsertDTO))
            {
                return BadRequest(_publishingHouseService.Errors);
            }

            var publishingHouseDTO = await _publishingHouseService.Add(publishingHouseInsertDTO);

            return CreatedAtAction(nameof(GetById), new { id = publishingHouseDTO.IdPublishingHouse }, publishingHouseDTO);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<PublishingHouseDTO>> Update(int id, PublishingHouseUpdateDTO publishingHouseUpdateDTO)
        {
            var validationResult = await _publishingHouseUpdateValidator.ValidateAsync(publishingHouseUpdateDTO);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            if (!_publishingHouseService.Validate(publishingHouseUpdateDTO))
            {
                return BadRequest(_publishingHouseService.Errors);
            }

            var publishingHouseDTO = await _publishingHouseService.Update(id, publishingHouseUpdateDTO);

            return publishingHouseDTO == null ? NotFound() : Ok(publishingHouseDTO);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<PublishingHouseDTO>> Delete(int id)
        {
            var publishingHouseDTO = await _publishingHouseService.Delete(id);
            return publishingHouseDTO == null ? NotFound() : Ok(publishingHouseDTO);
        }

    }
}

