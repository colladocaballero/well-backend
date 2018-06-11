using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WellBackend.Models;
using WellBackend.Services;

namespace WellBackend.Controllers
{
    [Produces("application/json")]
    [Route("api/Messages")]
    public class MessagesController : Controller
    {
        private readonly IMessagesService _messagesService;

        public MessagesController(IMessagesService messagesService)
        {
            _messagesService = messagesService;
        }

        // GET: api/Messages/5
        [HttpGet("{id}/Message", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            return new OkObjectResult(new
            {
                statusCode = 200,
                data = _messagesService.GetById(id)
            });
        }

        // GET: api/Messages/5
        [HttpGet("{id}", Name = "GetUserMessages")]
        public IActionResult GetUserMessages(string id)
        {
            return new OkObjectResult(new
            {
                statusCode = 200,
                data = new
                {
                    unreadCount = _messagesService.GetUnreadCount(id),
                    messages = _messagesService.GetUserMessages(id)
                }
            });
        }

        // POST: api/Messages
        [HttpPost]
        public async Task<IActionResult> NewMessage([FromBody]Message newMessage)
        {
            await _messagesService.AddMessage(newMessage);

            return Created("NewMessage", new
            {
                statusCode = 201,
                data = new
                {
                    message = "Message created"
                }
            });
        }
        
        // PUT: api/Messages/5/MarkAsRead
        [HttpPut("{id}/MarkAsRead")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _messagesService.MarkAsRead(id);

            return NoContent();
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
