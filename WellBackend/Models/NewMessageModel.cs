using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellBackend.Models
{
    public class NewMessageModel : Message
    {
        public string[] ReceiversIds { get; set; }
    }
}
