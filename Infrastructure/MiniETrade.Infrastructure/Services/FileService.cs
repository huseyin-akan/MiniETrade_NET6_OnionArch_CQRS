using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MiniETrade.Application.Services;
using MiniETrade.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                using FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);

                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {
                //TODO: loglama yapılacak.
                throw ex;
            }            
        }

        private async Task<string> FileRenameAsync2(string path, string fileName, bool first = true)
        {
            string newFileName = await Task.Run<string>(async () =>
            {
                string extension = Path.GetExtension(fileName);
                string newFileName = String.Empty;
                if (first)
                {
                    string oldName = Path.GetFileNameWithoutExtension(fileName);
                    newFileName = RenameHelper.CharRegulator(oldName) + extension;
                }
                else
                {
                    newFileName = fileName;
                    int indexNo1 = newFileName.LastIndexOf("-");
                    if (indexNo1 == -1)
                    {
                        newFileName = Path.GetFileNameWithoutExtension(newFileName) + "-" + 2 + extension;
                    }
                    else
                    {
                        int indexNo2 = newFileName.IndexOf(".");
                        string fileNo = newFileName.Substring(indexNo1 +1, indexNo2- indexNo1-1);

                        if (int.TryParse(fileNo, out int _fileNo))
                        {
                            _fileNo++;
                            newFileName = newFileName.Remove(indexNo1 +1, indexNo2 - indexNo1 - 1)
                                                 .Insert(indexNo1 +1, _fileNo.ToString());
                        }
                        else
                        {
                            newFileName = Path.GetFileNameWithoutExtension(newFileName) + "-2" + extension;
                        }
                    }
                }                

                if (File.Exists(path + "\\" + newFileName))
                {
                    return await FileRenameAsync2(path, newFileName, false);
                }
                else
                {
                    return newFileName;
                }
            });
            return newFileName;
        }

        private async Task<string> FileRenameAsync(string path, string fileName)
        {            
            string extension = Path.GetExtension(fileName);
            string oldName = Path.GetFileNameWithoutExtension(fileName);
            string regulatedFileName = RenameHelper.CharRegulator(oldName);
           
            var files = Directory.GetFiles(path, regulatedFileName + "*"); //bu isimle başlayan tüm dosyaları bulur

            if (files.Length == 0) return regulatedFileName + "-1" + extension; //Demek ki bu isimde ilk kez dosya yükleniyor.

            int[] fileNumbers = new int[files.Length];
            int lastHyphenIndex;
            for (int i = 0; i < files.Length; i++)
            {
                lastHyphenIndex = files[i].LastIndexOf("-");
                fileNumbers[i] = int.Parse(files[i].Substring(lastHyphenIndex + 1, files[i].Length - extension.Length - lastHyphenIndex -1) );
            }
            var biggestNumber = fileNumbers.Max();
            biggestNumber++;
            return regulatedFileName + "-" + biggestNumber + extension;
        }

        public async Task<List<(string fileName, string path)>> UploadAsync(string path, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(webHostEnvironment.WebRootPath, path);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            List<(string, string)> datas = new();
            List<bool> results = new();
            foreach (IFormFile file in files)
            {
                string fileNewName = await FileRenameAsync(uploadPath, file.FileName);

                bool result = await CopyFileAsync($"{uploadPath}\\{fileNewName}", file);
                datas.Add((fileNewName, $"{uploadPath}\\{fileNewName}"));
                results.Add(result);
            }

            if (results.TrueForAll(r => r.Equals(true) ) )  //If all results are true
            {
                return datas;
            }
            return null;
            //TODO: eğer false ise yukarısı burda hata fırlatmalıyız.
        }        
    }
}
