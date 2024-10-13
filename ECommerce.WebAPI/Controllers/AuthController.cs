using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.InputModelsDTO.AuthInputModelsDTO;
using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.InputModelsDTO.EmailSenderDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.AuthServiceInterface;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.EmailServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepoService _authRepoService;
        private readonly IEmailService _emailService;
        private readonly EmailSettings _emailSettings;

        public AuthController(IAuthRepoService authRepoService, IEmailService emailService, EmailSettings emailSettings)
        {
            _authRepoService = authRepoService;
            _emailService = emailService;
            _emailSettings = emailSettings;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterInputDTO registerModel)
        {
            //check if input model is valid or not.
            if(!ModelState.IsValid)
            {
                return StatusCode(200, "input is not valid");
            }
            else
            {
                try
                {
                    //send Register Input Model to service layer.
                    Response<RegisterInputDTO> registerServiceResponse = await _authRepoService.RegisterUserAsync(registerModel);

                    //check if response has error.
                    if(!registerServiceResponse.IsSuccessfull)
                    {
                        return StatusCode(200, registerServiceResponse);
                    }
                    else
                    {
                        return Ok(registerServiceResponse);
                    }
                }
                catch(Exception ex) 
                {
                    return StatusCode(200, new Response<RegisterInputDTO>() { ErrorMessage = ex.Message });
                }
            }
        }

        [HttpPost]
        [Route("log-in")]
        public async Task<IActionResult> LogIn([FromBody] LoginInpulDTO loginInpulDTO)
        {
            //check if input model is valid or not.
            if (!ModelState.IsValid)
            {
                return StatusCode(200, "input is not valid");
            }
            else
            {
                try
                {
                    //send LogIn Input Model to service layer.
                    Response<JwtTokenDTO> registerServiceResponse = await _authRepoService.LoginUserAsync(loginInpulDTO);

                    //check if response has error.
                    if (!registerServiceResponse.IsSuccessfull)
                    {
                        return StatusCode(200, registerServiceResponse);
                    }
                    else
                    {
                        return Ok(registerServiceResponse);
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(200, new Response<RegisterInputDTO>() { ErrorMessage = ex.Message });
                }
            }
        }

        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //send email id to verify availablity in database.
            Response<User> foundUserResponse = await _authRepoService.FindByEmailAsync(model.Email);
            if (!foundUserResponse.IsSuccessfull)
            {
                return BadRequest("User does not exist or email not confirmed.");
            }

            // Generate reset token and expiry
            foundUserResponse.Value.ResetToken = Guid.NewGuid().ToString();
            foundUserResponse.Value.ResetTokenExpiry = DateTime.Now.AddHours(1); // Token valid for 1 hour

            //update User in database wit reset token.


            //generate reset password link.
            string resetLink = $"{Request.Scheme}://{_emailSettings.ReturnRequestServer}/reset-password/{foundUserResponse.Value.ResetToken}/{_emailSettings.ToEmail}";

            //send email with reset link.
            await _emailService.SendEmailAsync(foundUserResponse.Value.Email, "Reset Password", $"Click <a href='{resetLink}'>here</a> to reset your password.");

            return Ok("Password reset link has been sent to your email.");
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            return Ok("password reset successfully");
        }

        [HttpGet]
        [Route("test")]
        [Authorize]
        public async Task<IActionResult> Test()
        {
            return Ok("You are authorised");
        }
    }
}
