
using Microsoft.AspNetCore.Mvc;
using RealEstateWebAPI.ActionFilters;
using RealEstateWebAPI.BLL.DTO;
using RealEstateWebAPI.BLL.Services;
using RealEstateWebAPI.Middleware;
using Serilog;
using System.Net;
using System.Security.Claims;

namespace RealEstateWebAPI.Controllers
{



    [ApiController]

    public class MessagesController : BaseController
    {
        private readonly IMessageService _messageService;
        private readonly IPropertiesService _propertiesService;


        public MessagesController(
                                  IMessageService messageService,
                                  IPropertiesService propertiesService
                                  )
        {
            _messageService = messageService;
            _propertiesService = propertiesService;
        }
        /// <summary>
        ///pasi kalon authorizimin nga middleware ,  merr mesazhet qe i perkasin atij agjenti te loguar
        /// </summary>
        [HttpGet("properties/messages")]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetAllMessages()
        {
            return await HandleAsync<IEnumerable<MessageDTO>>(async () =>
            {
                int authenticatedUserId = GetAuthenticatedUserId();
                var messages = await _messageService.GetAllMessagesByUser(authenticatedUserId);
                return Ok(messages);
            });
        }

        /// <summary>
        ///pasi kalon authentikim dhe authorizimin nga middleware , merr mesazhet per nje property me id specifike
        ///</summary>
        [HttpGet("properties/messages/{propertyId}")]
        [TypeFilter(typeof(AuthorisationFilter))]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessagesForProperty(int propertyId)
        {

            return await HandleAsync<IEnumerable<MessageDTO>>(async () =>
            {
                var messages = await _messageService.GetMessagesForPropertyAsync(propertyId);
                return Ok(messages);
            });
        }
        /// <summary>
        /// metode qe do te kthehe Id e userit te loguar ne ate moment 
        ///</summary>
        private int GetAuthenticatedUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim);
        }
        /// <summary>
        ///aksesi ne kete endpoint eshte anonymous ,  dergon mesazh mbi pronen me id Specifike
        /// </summary>
        [HttpPost("properties/{id}/message")]
        [AllowAnonymous]
        public async Task<ActionResult> SendMessage(int id, MessageDTO messageDTO)
        {
            return await HandleAsync(async () =>
            {
                var property = await _propertiesService.GetPropertyByIdAsync(id);
                messageDTO.PropertyId = id;
                await _messageService.SendMessageAsync(messageDTO);
                Log.Information("Message sent successfully");

            });
        }
    }
}


