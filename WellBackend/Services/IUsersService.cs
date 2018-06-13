using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WellBackend.Services
{
    public interface IUsersService
    {
        List<dynamic> GetFriends(string id);
    }
}
