using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellBackend.Models
{
    public class Message
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string UserTransmitterId { get; set; }
        public string UserReceiverId { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }

        public User UserTransmitter { get; set; }
        public User UserReceiver { get; set; }
    }
}
