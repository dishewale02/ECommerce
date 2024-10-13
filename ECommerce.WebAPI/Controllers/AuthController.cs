using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.InputModelsDTO.AuthInputModelsDTO;
using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.AuthServiceInterface;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.GenericRepoServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepoService _authRepoService;
        private readonly IGenericRepoService<UserInputDTO, User> _genericRepoService;

        public AuthController(IAuthRepoService authRepoService, IGenericRepoService<UserInputDTO, User> genericRepoService)
        {
            _authRepoService = authRepoService;
            _genericRepoService = genericRepoService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserInputDTO registerModel)
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
                    Response<UserInputDTO> registerServiceResponse = await _authRepoService.RegisterUserAsync(registerModel);

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
                    return StatusCode(200, new Response<UserInputDTO>() { ErrorMessage = ex.Message });
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
                    return StatusCode(200, new Response<LoginInpulDTO>() { ErrorMessage = ex.Message });
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

            //call forgot password method.

            Response<string> forgotPasswordResponse = await _authRepoService.ForgotPasswordAsync(model);
            
            //check response.
            if(!forgotPasswordResponse.IsSuccessfull)
            {
                return Ok(forgotPasswordResponse.ErrorMessage);

            }

            return Ok(forgotPasswordResponse.Value.ToString());
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordInputDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //get response from reset password function.
            Response<bool> ResetPasswordResponse = await _authRepoService.ResetPasswordAsync(model);

            if(!ResetPasswordResponse.IsSuccessfull)
            {
                return StatusCode(200, ResetPasswordResponse);
            }

            return Ok(ResetPasswordResponse);
        }

        [HttpPost]
        [Route("check-forgot-password-token")]
        public async Task<IActionResult> CheckForgotPasswordToken([FromBody] CheckForgotPasswordTokenModel model)
        {
            try
            {
                //call check forgot password service.
                Response<bool> checkForgotPasswordTokenResponse = await _authRepoService.CheckForgotPasswordTokenAsync(model);

                if(!checkForgotPasswordTokenResponse.IsSuccessfull)
                {
                    return Ok(Response<bool>.Failure(checkForgotPasswordTokenResponse.ErrorMessage));
                }

                return Ok(Response<bool>.Success(true));
            }
            catch (Exception ex) 
            {
                return Ok(Response<bool>.Failure(ex.Message));
            }

        }

        [HttpGet]
        [Route("get-user-details")]
        [Authorize]
        public async Task<IActionResult> GetUserDetails()
        {
            try
            {
                string? id = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
                string? email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                string? userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                string? role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                //check if id is null.
                if(string.IsNullOrEmpty(id))
                {
                    return BadRequest("user id is not available.");
                }

                //get user from User Id.
                Response<UserInputDTO> getUserResponse = await _genericRepoService.GetAsync(id);

                if(!getUserResponse.IsSuccessfull)
                {
                    return Ok(getUserResponse);
                }

                return Ok(getUserResponse.Value);
            }
            catch(Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
