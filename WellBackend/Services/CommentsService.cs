using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WellBackend.Contexts;
using WellBackend.Models;

namespace WellBackend.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly WellDbContext _wellDbContext;

        public CommentsService(WellDbContext wellDbContext)
        {
            _wellDbContext = wellDbContext;
        }

        public async Task AddComment(Comment newComment)
        {
            var user = _wellDbContext.UsersWell.FirstOrDefault(u => u.Id == newComment.UserId);

            _wellDbContext.Comments.Add(newComment);

            await _wellDbContext.SaveChangesAsync();
        }

        public List<dynamic> GetWallComments(string id)
        {
            var friendships = _wellDbContext.Friendships.Where(f => f.User1Id == id || f.User2Id == id).ToList();

            List<string> ids = new List<string>
            {
                id
            };

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
            
            List<dynamic> comments = new List<dynamic>();
            for (int i = 0; i < ids.Count; i++)
            {
                _wellDbContext.Comments.Where(c => c.UserId == ids[i]).OrderByDescending(c => c.Date).ToList().ForEach(c =>
                {
                    var user = _wellDbContext.UsersWell.FirstOrDefault(u => u.Id == c.UserId);

                    comments.Add(new 
                    {
                        c.Id,
                        c.Text,
                        c.Date,
                        c.Likes,
                        User = new
                        {
                            user.Id,
                            user.Name,
                            user.Surname,
                            user.ProfilePicture
                        }
                    });
                });
            }

            return comments.OrderByDescending(c => c.Date).ToList();
        }
    }
}
