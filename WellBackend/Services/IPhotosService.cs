using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WellBackend.Models;

namespace WellBackend.Services
{
    public interface IPhotosService
    {
        IEnumerable<Photo> GetByUserId(string userId);

        Task<string> UploadImage(IFormFile image, string userId);
    }
}
