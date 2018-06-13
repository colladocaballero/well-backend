using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WellBackend.Contexts;

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
    }
}
