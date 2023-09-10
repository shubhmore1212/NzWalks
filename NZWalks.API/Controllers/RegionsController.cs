using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repository;

namespace NZWalks.API.Controllers
{
    //ApiController==>controller is for api use,returns 400 for error
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NzWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;

        public RegionsController(NzWalksDbContext dbContext, IRegionRepository regionRepository)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get data from db - Domain Models
            var regionsDomian = await regionRepository.GetAllAsync();

            //Map Domain Models to DTOs
            var regionsDto = new List<RegionDto>();
            foreach (var regionDomain in regionsDomian)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    ImageUrl = regionDomain.ImageUrl,
                });
            }

            //Return DTOs
            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetByID([FromRoute] Guid id)
        {
            var regionDomain = await regionRepository.GetByIdAsync(id);
            //var regionsDomain = dbContext.Regions.FirstOrDefault(x=>x.Id==id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                ImageUrl = regionDomain.ImageUrl,
            };

            return Ok(regionDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map Convert DTO to domain model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                ImageUrl = addRegionRequestDto.ImageUrl,
            };

            //Use Domain Model to create region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            //Map domain model back to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                ImageUrl = regionDomainModel.ImageUrl,
            };

            return CreatedAtAction(nameof(GetByID), new { id = regionDomainModel.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Map DTO to Domain
            var regionDomainModel = new Region
            {
                Code = updateRegionRequestDto.Code,
                Name = updateRegionRequestDto.Name,
                ImageUrl = updateRegionRequestDto.ImageUrl,
            };

            regionDomainModel=await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                ImageUrl = regionDomainModel.ImageUrl,
            };

            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteByIdAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //Returning deleted region back
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                ImageUrl = regionDomainModel.ImageUrl,
            };

            return Ok(regionDto);
        }

    }
}
