using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RealEstateWebAPI.BLL.DTO;
using RealEstateWebAPI.BLL.Services;
using System.Collections.Generic;
using RealEstateWebAPI.Common.ErrorHandeling;
using RealEstateWebAPI.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using MimeKit;

namespace RealEstateWebAPI.Controllers
{

    [ApiController]
    [Route("users")]
    public class UserController : BaseController
    {
        private readonly IUsersService _userService;
        private readonly IEmailService _emailService;

        public UserController(IUsersService userService, IEmailService emailService)
        {

            _userService = userService;
            _emailService = emailService;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthorisationFilter))]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            return await HandleAsync<IEnumerable<UserDTO>>(async () =>
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            });
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(AuthorisationFilter))]

        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            /*throw new Common.ErrorHandeling.CustomException("missing user");*/

            return await HandleAsync<UserDTO>(async () =>
            {
                var user = await _userService.GetUserByIdAsync(id);
                return user;
            });
        }

        [HttpPost("create")]
        [TypeFilter(typeof(AuthorisationFilter))]
        public async Task<ActionResult<int>> AddUser(UserDTO userDTO)
        {
            return await HandleAsync<int>(async () =>
            {
                var userId = await _userService.AddUserAsync(userDTO);

                // Send email to the user
                await SendNewUserEmail(userDTO.Email, userDTO.UserName, userDTO.Password);

                return userId;
            });
        }


        [HttpPut("update/{id}")]
        [TypeFilter(typeof(AuthorisationFilter))]
        public async Task<ActionResult> UpdateUser(int id, UserDTO userDTO)
        {
            return await HandleAsync(async () =>
            {
                await _userService.UpdateUserAsync(id, userDTO);
            });
        }

        [HttpDelete("delete/{id}")]
        [TypeFilter(typeof(AuthorisationFilter))]
        public async Task<ActionResult> DeleteUser(int id)
        {
            return await HandleAsync(async () =>
            {
                await _userService.DeleteUserAsync(id);
            });
        }
        [HttpGet("{userId}/properties")]
        [TypeFilter(typeof(AuthorisationFilter))]
        public async Task<ActionResult<IEnumerable<PropertyDTO>>> GetPropertiesByUserId(int userId)
        {
            return await HandleAsync<IEnumerable<PropertyDTO>>(async () =>
            {
                var properties = await _userService.GetPropertiesByUserIdAsync(userId);
                return Ok(properties);
            });
        }
        private async Task SendNewUserEmail(string email, string username, string password)
        {
            // Create the email message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Admin", "hoxhakevi09@gmail.com")); // Set the admin's email address
            message.To.Add(new MailboxAddress("", email)); // Set the user's email address
            message.Subject = "Welcome to the Real Estate Web App";
            message.Body = new TextPart("plain")
            {
                Text = $"Hello {username},\n\n" +
                       $"Your account has been created for the Real Estate Web App. Here are your credentials:\n\n" +
                       $"Username: {username}\n" +
                       $"Password: {password}\n\n" +
                       $"Thank you for joining our platform!"
            };

            // Send the email using the email service
            await _emailService.SendAsync(message);
        }
    }
}
