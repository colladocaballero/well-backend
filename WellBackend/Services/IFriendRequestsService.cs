using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WellBackend.Models;

namespace WellBackend.Services
{
    public interface IFriendRequestsService
    {
        List<dynamic> GetUserFriendRequests(string id);

        int GetRequestsCount(string id);

        Task SendFriendRequest(FriendRequest friendRequest);

        Task AcceptFriendRequest(FriendRequest friendRequest);

        Task RejectFriendRequest(FriendRequest friendRequest);
    }
}
