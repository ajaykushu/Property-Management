using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Utilities.Interface;

namespace Utilities
{
    public class ImageUploadInFile : IImageUploadInFile
    {
        private List<string> allowedTypes=new List<string> {".jpg",".jpeg",".png",".pdf",".xls",".xlx",".doc",".docx" };
        
        public bool Delete(string path)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
                {
                    File.Delete(path);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<string> UploadAsync(IFormFile file)
        {
            var id = Guid.NewGuid().ToString().Replace("-", "");
            if (file!=null && file.Length > 0)
            {

                try
                {
                    var type=System.IO.Path.GetExtension(file.FileName);
                    if (!allowedTypes.Contains(type))
                        return null;
                    if (!Directory.Exists("ImageFileStore"))
                    {
                        Directory.CreateDirectory("ImageFileStore");
                    }
                    string path = "ImageFileStore/" + id + file.FileName;
                    using (FileStream filestream = System.IO.File.Create(path))
                    {
                        await file.CopyToAsync(filestream);
                        filestream.Flush();
                    }
                    return path;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}