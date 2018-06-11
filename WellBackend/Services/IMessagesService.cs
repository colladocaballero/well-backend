using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WellBackend.Models;

namespace WellBackend.Services
{
    public interface IMessagesService
    {
        dynamic GetById(int id);

        List<dynamic> GetUserMessages(string userId);

        Task AddMessage(Message newMessage);

        int GetUnreadCount(string userId);

        Task MarkAsRead(int id);
    }
}
