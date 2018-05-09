using Core.Api.DB;
using Core.Api.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Core.Api.Services
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        private readonly CoreDbContext _coreDbContext;

        public CountryRepository(CoreDbContext coreDbContext) : base(coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        public async Task<Country> GetAsync(int id)
        {
            return await _coreDbContext.Country.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
