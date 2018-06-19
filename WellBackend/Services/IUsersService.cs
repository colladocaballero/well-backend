using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WellBackend.Models;

namespace WellBackend.Services
{
    public interface IUsersService
    {
        List<dynamic> GetFriends(string id, string userId);

        List<dynamic> SearchUsers(string query, string id);

        Task ChangeProfilePicture(string id, Photo photo);

        Task RemoveFriend(string user1Id, string user2Id);
    }
}
