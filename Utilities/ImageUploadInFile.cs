using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Utilities.Interface;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Utilities
{
    public class ImageUploadInFile : IImageUploadInFile
    {
        private readonly List<string> allowedTypes = new List<string> { ".jpg", ".jpeg", ".png", ".pdf", ".xls", ".xlx", ".doc", ".docx" };
        private readonly List<string> imageTypes = new List<string> { ".jpg", ".jpeg", ".png" };

        public bool Delete(string path)
        {
            try
            {
               
                
                if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
                {
                    File.Delete(path);
                    var thumbnailpath = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path)+ ".png");
                    if (File.Exists(thumbnailpath))
                    {
                        File.Delete(thumbnailpath);
                        
                    }
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

        public async Task<string> UploadAsync(IFormFile file, string subDirectory = null)
        {
            var id = Guid.NewGuid().ToString().Replace("-", "");

            if (file != null && file.Length > 0)
            {
                try
                {
                    String path = "ImageFileStore/";
                    var type = Path.GetExtension(file.FileName);
                    if (!allowedTypes.Contains(type))
                        return null;
                    
                    if (!Directory.Exists("ImageFileStore"))
                        Directory.CreateDirectory("ImageFileStore");

                    path = path + id+ file.FileName;
                    if (imageTypes.Contains(type))
                    {
                        
                        var tempath = Path.GetDirectoryName(path)+"/"+Path.GetFileNameWithoutExtension(path);
                        //saving main image
                        Image b = Bitmap.FromStream(file.OpenReadStream());
                        var resized = new Bitmap(200, 150);
                        using (var graphics = Graphics.FromImage(resized))
                        {
                            graphics.CompositingQuality = CompositingQuality.HighSpeed;
                            graphics.InterpolationMode=InterpolationMode.Low;
                            graphics.CompositingMode = CompositingMode.SourceCopy;
                            graphics.DrawImage(b, 0, 0, 200, 150);
                            resized.Save(tempath + ".png", ImageFormat.Png);
                        }
                        Image c = Image.FromStream(file.OpenReadStream());
                        c.Save(tempath + ".jpg", ImageFormat.Jpeg);
                        return tempath + ".jpg";



                    }
                    else
                    {
                        
                        using (FileStream filestream = File.Create(path))
                        {
                            await file.CopyToAsync(filestream);
                            filestream.Flush();
                        }
                        return path;
                    }
                    
                    
                }
                catch (Exception ex)
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