using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using WellBackend.Contexts;
using WellBackend.Models;

namespace WellBackend.Services
{
    public class PhotosService : IPhotosService
    {
        private readonly WellDbContext _wellDbContext;
        private readonly IHostingEnvironment _hostingEnvironment;

        public PhotosService(WellDbContext wellDbContext, IHostingEnvironment hostingEnvironment)
        {
            _wellDbContext = wellDbContext;
            _hostingEnvironment = hostingEnvironment;
        }

        public IEnumerable<Photo> GetByUserId(string userId)
        {
            return _wellDbContext.Photos.Where(photo => photo.UserId == userId);
        }

        public async Task<string> UploadImage(IFormFile image, string userId)
        {
            string imageName = Path.GetFileNameWithoutExtension(image.FileName); // Image's name withour extension
            string imageExtension = Path.GetExtension(image.FileName); // Image's extension

            Regex datePattern = new Regex("[/: ]"); // RegEx for date formatting
            string uploadTime = datePattern.Replace(DateTime.Now.ToString(), ""); // Date and time of the upload

            string fileName = imageName + uploadTime + imageExtension;

            DirectoryInfo di = Directory.CreateDirectory(_hostingEnvironment.WebRootPath + "\\img\\" + userId);

            using (FileStream fs = File.Create(_hostingEnvironment.WebRootPath + "\\img\\" + userId + "\\" + fileName))
            {
                image.CopyTo(fs);
                fs.Flush();
            }

            Photo photo = new Photo
            {
                FileName = fileName,
                UploadDate = DateTime.Now,
                Likes = 0,
                UserId = userId,
                User = _wellDbContext.UsersWell.FirstOrDefault(user => user.Id == userId)
            };

            await _wellDbContext.Photos.AddAsync(photo);

            await _wellDbContext.SaveChangesAsync();

            return fileName;
        }
    }
}
