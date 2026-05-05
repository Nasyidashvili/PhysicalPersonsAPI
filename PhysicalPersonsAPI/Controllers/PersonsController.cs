using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhysicalPersonsAPI.Data;
using PhysicalPersonsAPI.Models;
using PhysicalPersonsAPI.DTOS;
using PhysicalPersonsAPI.Interfaces;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc.Infrastructure;


namespace PhysicalPersonsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPhysicalPersonService _physicalPersonService;

        public PersonsController(IPhysicalPersonService _physicalPersonService)
        {
            this._physicalPersonService = _physicalPersonService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhysicalPersonResponseDto>>> GetPhysicalPersons()
        {
            var physicalPersons = await _physicalPersonService.GetAllAsync();
            return Ok(physicalPersons);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PhysicalPersonResponseDto>> GetPhysicalPerson(int id)
        {
            var phyisalPerson = await _physicalPersonService.GetByIdAsync(id);
            if (phyisalPerson == null)
            {
                return NotFound();
            }

            return Ok(phyisalPerson);
        }

        [HttpPost]
        public async Task<ActionResult<PhysicalPersonResponseDto>> CreatePhysicalResponse([FromBody] CreatePhysicalPersonDto physicalPerson)
        {
            var createdPerson = await _physicalPersonService.CreateAsync(physicalPerson);
            if (createdPerson == null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetPhysicalPerson), new { id = createdPerson.Id }, createdPerson);
        }
        [HttpPost("{id}/upload-image")]
        public async Task<ActionResult> UploadImage(int id, IFormFile image)
        {
            var result = await _physicalPersonService.UploadImageAsync(id, image);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PhysicalPersonResponseDto>> UpdatePhysicalResponse(int id, [FromBody] UpdatePhysicalPersonDto physicalPerson)
        {
            var updatedPerson = await _physicalPersonService.UpdateAsync(id, physicalPerson);
            if (updatedPerson == null)
            {
                return NotFound();
            }
            return Ok(updatedPerson);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePhysicalResponse(int id)
        {
            var deleted = await _physicalPersonService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpGet("search")]
        public async Task<ActionResult<ResultDto<PhysicalPersonResponseDto>>> SearchPhysicalResponse([FromQuery]SearchDto searchDto)
        {
            var result = await _physicalPersonService.SearchAsync(searchDto);
            return Ok(result);
        }
    }
}

