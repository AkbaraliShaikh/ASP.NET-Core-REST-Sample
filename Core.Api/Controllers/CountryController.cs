using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.Api.DB;
using Core.Api.Models;
using Core.Api.Services;

namespace Core.Api.Controllers
{
    [Route("api/v1/[controller]")]
    public class CountryController : Controller
    {
        private readonly ICsvService _csvService;
        private readonly IFileService _fileService;
        private readonly ICountryRepository _countryRepository;

        public CountryController(ICsvService csvService, IFileService fileService,
            ICountryRepository countryRepository)
        {
            _csvService = csvService;
            _fileService = fileService;
            _countryRepository = countryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var records = await _countryRepository.GetAsync();
            if (records == null)
                return NoContent();

            return Ok(records);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var record = await _countryRepository.GetAsync(id);
            if (record == null)
                return NotFound();

            return Ok(record);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Country country)
        {
            if (country == null)
                return BadRequest();

            await _countryRepository.SaveAsync(country);
            return Ok();
        }

        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Post(IFormFile file)
        {
            var path = await _fileService.SaveFileAsync(file);

            if (path == null)
                return BadRequest();

            var records = _csvService.Get<Country>(System.IO.File.OpenText(path));
            await _countryRepository.SaveAsync(records);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody]Country country)
        {
            if (country == null)
                return BadRequest();

            var record = await _countryRepository.GetAsync(id);
            if (record == null)
                return NotFound();

            country.Id = record.Id;
            await _countryRepository.UpdateAsync(country);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var record = await _countryRepository.GetAsync(id);

            if (record != null)
                await _countryRepository.DeleteAsync(record);

            return NoContent();
        }
    }
}
