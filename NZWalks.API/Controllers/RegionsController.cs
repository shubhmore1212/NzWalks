using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    //ApiController==>controller is for api use,returns 400 for error
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NzWalksDbContext dbContext;

        public RegionsController(NzWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            //Get data from db - Domain Models
            var regionsDomian = dbContext.Regions.ToList();

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
        public IActionResult GetByID([FromRoute] Guid id)
        {
            var regionDomain = dbContext.Regions.Find(id);
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
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map Convert DTO to domain model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                ImageUrl = addRegionRequestDto.ImageUrl,
            };

            //Use Domain Model to create region
            dbContext.Regions.Add(regionDomainModel);
            dbContext.SaveChanges();

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
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionDomainModel = dbContext.Regions.FirstOrDefault(_ => _.Id == id);

            if(regionDomainModel==null)
            {
                return NotFound();
            }

            //Map Dto to domain model
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.ImageUrl = updateRegionRequestDto.ImageUrl;

            dbContext.SaveChanges();

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
        public IActionResult Delete([FromRoute] Guid id)
        {
            var regionDomainModel=dbContext.Regions.FirstOrDefault(_ => _.Id == id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            dbContext.Regions.Remove(regionDomainModel);
            dbContext.SaveChanges();

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
