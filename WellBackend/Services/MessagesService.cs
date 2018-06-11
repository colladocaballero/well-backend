using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WellBackend.Contexts;
using WellBackend.Models;

namespace WellBackend.Services
{
    public class MessagesService : IMessagesService
    {
        private readonly WellDbContext _wellDbContext;

        public MessagesService(WellDbContext wellDbContext)
        {
            _wellDbContext = wellDbContext;
        }

        public dynamic GetById(int id)
        {
            var message = _wellDbContext.Messages.FirstOrDefault(m => m.Id == id);

            return generateResponse(message);
        }

        public List<dynamic> GetUserMessages(string userId)
        {
            List<Message> messages = _wellDbContext.Messages.Where(message => message.UserReceiverId == userId).OrderByDescending(m => m.Date).ToList();

            List<dynamic> messagesResponse = new List<dynamic>();

            messages.ForEach(m => messagesResponse.Add(generateResponse(m)));

            return messagesResponse;
        }

        public async Task AddMessage(Message newMessage)
        {
            newMessage.Status = "Unread";
            _wellDbContext.Messages.Add(newMessage);
            await _wellDbContext.SaveChangesAsync();
        }

        public int GetUnreadCount(string userId)
        {
            int unreadCount = _wellDbContext.Messages.Where(m => m.Status == "Unread").ToList().Count;

            return unreadCount;
        }

        private dynamic generateResponse(Message message)
        {
            var userReceiver = _wellDbContext.UsersWell.FirstOrDefault(u => u.Id == message.UserReceiverId);
            var userTransmitter = _wellDbContext.UsersWell.FirstOrDefault(u => u.Id == message.UserTransmitterId);

            return new
            {
                message.Id,
                message.Title,
                message.Text,
                message.Date,
                message.Status,
                message.UserTransmitterId,
                message.UserReceiverId,
                UserTransmitter = new
                {
                    userTransmitter.Id,
                    userTransmitter.Name,
                    userTransmitter.Surname,
                    userTransmitter.ProfilePicture
                },
                UserReceiver = new
                {
                    userReceiver.Id,
                    userReceiver.Name,
                    userReceiver.Surname,
                    userReceiver.ProfilePicture
                }
            };
        }

        public async Task MarkAsRead(int id)
        {
            var message = _wellDbContext.Messages.FirstOrDefault(m => m.Id == id);
            message.Status = "Read";
            _wellDbContext.Messages.Update(message);
            await _wellDbContext.SaveChangesAsync();
        }
    }
}
