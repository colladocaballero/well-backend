using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WellBackend.Contexts;
using WellBackend.Models;

namespace WellBackend.Services
{
    public class UsersService : IUsersService
    {
        private readonly WellDbContext _wellDbContext;

        public UsersService(WellDbContext wellDbContext)
        {
            _wellDbContext = wellDbContext;
        }

        public List<dynamic> GetFriends(string id, string userId)
        {
            var friendships = id == userId ? _wellDbContext.Friendships.Where(f => f.User1Id == id || f.User2Id == id).ToList() : _wellDbContext.Friendships.Where(f => (f.User1Id == id || f.User2Id == id) && (f.User1Id != userId && f.User2Id != userId)).ToList();

            List<string> ids = new List<string>();

            friendships.ForEach(f =>
            {
                if (f.User1Id == id)
                {
                    ids.Add(f.User2Id);
                }
                else if (f.User2Id == id)
                {
                    ids.Add(f.User1Id);
                }
            });

            List<dynamic> friends = new List<dynamic>();
            for (int i = 0; i < ids.Count; i++)
            {
                _wellDbContext.UsersWell.Where(u => u.Id == ids[i]).ToList().ForEach(u =>
                {
                    var frienship = _wellDbContext.Friendships.FirstOrDefault(f => (f.User1Id == u.Id && f.User2Id == userId) || (f.User2Id == u.Id && f.User1Id == userId));

                    bool areFriends = frienship != null ? true : false;

                    var friendRequest = _wellDbContext.FriendRequests.FirstOrDefault(fr => (fr.User1Id == userId && fr.User2Id == u.Id) || (fr.User1Id == userId && fr.User2Id == u.Id));

                    bool hasFriendRequest = friendRequest != null ? true : false;

                    friends.Add(new
                    {
                        u.Id,
                        u.Name,
                        u.Surname,
                        u.ProfilePicture,
                        areFriends,
                        hasFriendRequest
                    });
                });
            }

            return friends;
        }

        public List<dynamic> SearchUsers(string query, string id)
        {
            query = query.ToUpper();
            var users = _wellDbContext.UsersWell.Where(u => (u.Name.ToUpper().Contains(query) || u.Surname.ToUpper().Contains(query) || (u.Name + " " + u.Surname).ToUpper().Contains(query)) && u.Id != id).ToList(); // Find query except actual user

            List<dynamic> usersList = new List<dynamic>();
            users.ForEach(u =>
            {
                var frienship = _wellDbContext.Friendships.FirstOrDefault(f => (f.User1Id == u.Id && f.User2Id == id) || (f.User2Id == u.Id && f.User1Id == id)); // Check if user and search results are already friends

                bool areFriends = frienship != null ? true : false;

                var friendRequest = _wellDbContext.FriendRequests.FirstOrDefault(fr => (fr.User1Id == id && fr.User2Id == u.Id) || (fr.User1Id == u.Id && fr.User2Id == id)); // Check if user has active friend requests with someone on the search results

                bool hasFriendRequest = friendRequest != null ? true : false;

                usersList.Add(new
                {
                    u.Id,
                    u.Name,
                    u.Surname,
                    u.ProfilePicture,
                    u.Gender,
                    u.Country,
                    u.City,
                    areFriends,
                    hasFriendRequest
                });
            });

            return usersList;
        }

        public async Task ChangeProfilePicture(string id, Photo photo)
        {
            var user = _wellDbContext.UsersWell.FirstOrDefault(u => u.Id == id);
            user.ProfilePicture = id + "/" + photo.FileName;

            _wellDbContext.UsersWell.Update(user);
            await _wellDbContext.SaveChangesAsync();
        }

        public async Task RemoveFriend(string user1Id, string user2Id)
        {
            _wellDbContext.Friendships.Remove(_wellDbContext.Friendships.FirstOrDefault(fs => (fs.User1Id == user1Id && fs.User2Id == user2Id) || (fs.User1Id == user2Id && fs.User2Id == user1Id)));

            await _wellDbContext.SaveChangesAsync();
        }
    }
}
