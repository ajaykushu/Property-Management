using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Utilities.Interface
{
    public interface IImageUploadInFile
    {
        Task<string> UploadAsync(IFormFile file);

        bool Delete(string path);
    }
}