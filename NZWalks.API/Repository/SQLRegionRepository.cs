using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repository
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NzWalksDbContext dbContext;
        public SQLRegionRepository(NzWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(_ => _.Id == id);
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var existingRegion=await dbContext.Regions.FirstOrDefaultAsync(_ => _.Id == id);
            if(existingRegion == null)
            {
                return null;
            }
            
            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.ImageUrl = region.ImageUrl;

            await dbContext.SaveChangesAsync();
            
            return existingRegion;
        }

        public async Task<Region?> DeleteByIdAsync(Guid id)
        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(_ => _.Id == id);

            if(existingRegion == null)
            {
                return null;
            }

            dbContext.Regions.Remove(existingRegion);
            await dbContext.SaveChangesAsync();

            return existingRegion;
        }
    }
}
