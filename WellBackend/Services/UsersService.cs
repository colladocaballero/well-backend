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

        public List<dynamic> GetFriends(string id)
        {
            var friendships = _wellDbContext.Friendships.Where(f => f.User1Id == id || f.User2Id == id).ToList();

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
                    friends.Add(new
                    {
                        u.Id,
                        u.Name,
                        u.Surname,
                        u.ProfilePicture
                    });
                });
            }

            return friends;
        }

        public List<dynamic> SearchUsers(string query, string id)
        {
            query = query.ToUpper();
            var users = _wellDbContext.UsersWell.Where(u => (u.Name.ToUpper().Contains(query) || u.Surname.ToUpper().Contains(query)) && u.Id != id).ToList();

            var friendships = _wellDbContext.Friendships.Where(f => f.User1Id == id || f.User2Id == id).ToList();

            List<dynamic> usersList = new List<dynamic>();
            users.ForEach(u =>
            {
                var frienship = friendships.Find(f => (f.User1Id == u.Id && f.User2Id == id) || (f.User2Id == u.Id && f.User1Id == id));

                bool areFriends;
                areFriends = frienship != null ? true : false;

                usersList.Add(new
                {
                    u.Id,
                    u.Name,
                    u.Surname,
                    u.ProfilePicture,
                    u.Gender,
                    u.Country,
                    u.City,
                    areFriends
                });
            });

            return usersList;
        }
    }
}
