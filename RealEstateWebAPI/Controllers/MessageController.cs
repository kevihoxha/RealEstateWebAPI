using Microsoft.AspNetCore.Mvc;
using RealEstateWebAPI.BLL.DTO;
using RealEstateWebAPI.BLL.Services;
using System.Net;

namespace RealEstateWebAPI.Controllers
{

   
    
        [ApiController]

        public class MessagesController : BaseController
        {
            private readonly IMessageService _messageService;
        private readonly IPropertiesService _propertiesService;
            private readonly ILogger<MessagesController> _logger;

            public MessagesController(ILogger<MessagesController> logger, IMessageService messageService,IPropertiesService propertiesService) : base(logger)
            {
                _logger = logger;
                _messageService = messageService;
            _propertiesService = propertiesService;
            }

            [HttpGet("properties/messages/{propertyId}")]
            public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessagesForProperty(int propertyId)
            {
                return await HandleAsync<IEnumerable<MessageDTO>>(async () =>
                {
                    var messages = await _messageService.GetMessagesForPropertyAsync(propertyId);
                    return Ok(messages);
                });
            }

        [HttpPost("properties/{id}/message")]
        public async Task<ActionResult> SendMessage(int id,MessageDTO messageDTO)
            {
                return await HandleAsync(async () =>
                {
                    var property = await _propertiesService.GetPropertyByIdAsync(id);
                    messageDTO.PropertyId = id;
                    await _messageService.SendMessageAsync(messageDTO);
                    _logger.LogInformation("Message sent successfully");
                   
                });
            }
        }
        }
    

