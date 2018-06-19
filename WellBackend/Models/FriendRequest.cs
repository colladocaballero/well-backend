using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellBackend.Models
{
    public class FriendRequest
    {
        public long Id { get; set; }
        public string User1Id { get; set; }
        public string User2Id { get; set; }
        public DateTime Date { get; set; }

        public User User1 { get; set; }
        public User User2 { get; set; }
    }
}
