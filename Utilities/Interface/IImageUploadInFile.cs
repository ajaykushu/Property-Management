using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Utilities.Interface
{
    public interface IImageUploadInFile
    {
        Task<string> UploadAsync(IFormFile file, string directory = null);

        bool Delete(string path);
    }
}