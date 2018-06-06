using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellBackend.Models
{
    public class Photo
    {
        public long Id { get; set; }
        public string FileName { get; set; }
        public DateTime UploadDate { get; set; }
        public long Likes { get; set; }
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
