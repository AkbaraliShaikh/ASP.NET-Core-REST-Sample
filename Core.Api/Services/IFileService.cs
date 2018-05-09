using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Core.Api.Services
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file);
    }
}
