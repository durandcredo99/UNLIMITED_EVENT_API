using Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private string absoluteFilePath;
        private string relativeFilePath;

        public string AbsoluteFilePath
        {
            get { return absoluteFilePath; }
            //set { folderName = value; }
        }

        private string fileName;
        public string FilePath { set => fileName = value; }



        public FileRepository(IWebHostEnvironment webHostEnvironment, string filePath)
        {
            _webHostEnvironment = webHostEnvironment;
            this.absoluteFilePath = $"{_webHostEnvironment.WebRootPath}/{filePath}";

            relativeFilePath = $"{filePath}";
        }



        public async Task DeleteFile(string filePath)
        {
            try
            {
                await Task.Run(()=> File.Delete(filePath));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            try
            {
                if (!System.IO.Directory.Exists(absoluteFilePath)) Directory.CreateDirectory(absoluteFilePath);


                if (file.Length > 0)
                {
                    string extention = file.FileName.Split('.').Last();

                    var fullPath = $"{this.absoluteFilePath}/{fileName}.{extention}";
                    var relativePath = $"{this.relativeFilePath}/{fileName}.{extention}";
                    using (var stream = File.Create(fullPath))
                    {
                        await file.CopyToAsync(stream);
                        await stream.FlushAsync();
                        return relativePath;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return null;
        }
    }
}
