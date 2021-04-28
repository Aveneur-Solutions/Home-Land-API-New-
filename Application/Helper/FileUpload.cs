using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
namespace Application.Helper
{
    public static class FileUpload
    {

        public static List<string> UploadImage(List<IFormFile> files, IWebHostEnvironment environment, string folderName = null)
        {
            var fileNames = new List<string> { };
            if (files.Count > 0)
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var fName = folderName != null ? "\\" + folderName + "\\" : "\\CommonImages\\";
                        try
                        {
                            if (!Directory.Exists(environment.WebRootPath + fName))
                            {
                                Directory.CreateDirectory(environment.WebRootPath + fName);
                            }
                            var fileLocationPath = fName + Guid.NewGuid().ToString()+ file.FileName;
                            using (FileStream filestream = System.IO.File.Create(environment.WebRootPath +fileLocationPath))
                            {
                                file.CopyTo(filestream);
                                filestream.Flush();
                                
                                fileNames.Add(fileLocationPath);
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }

            return fileNames;


        }

    }
}