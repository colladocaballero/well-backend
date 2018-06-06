using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellBackend.Models
{
    public class Comment
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public long Likes { get; set; }
        public string UserId { get; set; }
        public long? PhotoId { get; set; }
        public long? ParentCommentId { get; set; }

        public User User { get; set; }
        public Photo Photo { get; set; }
        public Comment ParentComment { get; set; }
    }
}
