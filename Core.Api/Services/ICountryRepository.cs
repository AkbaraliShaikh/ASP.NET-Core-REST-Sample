using Core.Api.Models;
using System.Threading.Tasks;

namespace Core.Api.Services
{
    public interface ICountryRepository : IRepository<Country>
    {
        Task<Country> GetAsync(int id);
    }
}
