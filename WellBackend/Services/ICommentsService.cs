using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WellBackend.Models;

namespace WellBackend.Services
{
    public interface ICommentsService
    {
        Task AddComment(Comment newComment);

        List<dynamic> GetWallComments(string id);

        List<Comment> GetUserComments(string id);
    }
}
