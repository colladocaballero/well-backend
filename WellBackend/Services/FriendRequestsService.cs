using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WellBackend.Contexts;
using WellBackend.Models;

namespace WellBackend.Services
{
    public class FriendRequestsService : IFriendRequestsService
    {

        private readonly WellDbContext _wellDbContext;

        public FriendRequestsService(WellDbContext wellDbContext)
        {
            _wellDbContext = wellDbContext;
        }

        public List<dynamic> GetUserFriendRequests(string id)
        {
            var friendRequests = _wellDbContext.FriendRequests.Where(fr => fr.User2Id == id).ToList();

            List<dynamic> requests = new List<dynamic>();

            friendRequests.ForEach(fr =>
            {
                var user1 = _wellDbContext.UsersWell.FirstOrDefault(u => u.Id == fr.User1Id);
                var user2 = _wellDbContext.UsersWell.Find(id);

                requests.Add(new
                {
                    fr.Id,
                    fr.User1Id,
                    fr.User2Id,
                    fr.Date,
                    User1 = new
                    {
                        user1.Id,
                        user1.Name,
                        user1.Surname,
                        user1.ProfilePicture
                    },
                    User2 = new
                    {
                        user2.Id,
                        user2.Name,
                        user2.Surname,
                        user2.ProfilePicture
                    }
                });
            });

            return requests;
        }

        public int GetRequestsCount(string id)
        {
            return _wellDbContext.FriendRequests.Where(fr => fr.User2Id == id).ToList().Count;
        }

        public async Task SendFriendRequest(FriendRequest friendRequest)
        {
            if (_wellDbContext.FriendRequests.FirstOrDefault(fr => fr.User1Id == friendRequest.User1Id && fr.User2Id == friendRequest.User2Id) == null)
            {
                friendRequest.Date = DateTime.Now;

                _wellDbContext.FriendRequests.Add(friendRequest);
                await _wellDbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception();
            }
        }

        public async Task AcceptFriendRequest(FriendRequest friendRequest)
        {
            _wellDbContext.Friendships.Add(new Friendship
            {
                User1Id = friendRequest.User1Id,
                User2Id = friendRequest.User2Id,
                FriendsSince = DateTime.Now
            });

            await _wellDbContext.SaveChangesAsync();

            _wellDbContext.FriendRequests.Remove(friendRequest);

            await _wellDbContext.SaveChangesAsync();
        }

        public async Task RejectFriendRequest(long id)
        {
            _wellDbContext.FriendRequests.Remove(_wellDbContext.FriendRequests.FirstOrDefault(fr => fr.Id == id));

            await _wellDbContext.SaveChangesAsync();
        }
    }
}
