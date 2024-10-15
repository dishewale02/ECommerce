using AutoMapper;
using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.InputModelsDTO.AuthInputModelsDTO;
using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Services.Interfaces.OtherServicesInterfaces.JwtTokenGeneratorInterface;
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
        private readonly IGenericRepoService<UserInputDTO, User> _genSerForUserInputDTO;
        private readonly IGenericRepoService<GetUserDetailsOutputDTO, User> _getSerForGetUserDetailsOutputDTO;
        private readonly IMapper _mapper;
        private readonly IAuthenticator _authenticator;

        public AuthController(IAuthRepoService authRepoService,
            IMapper mapper,
            IGenericRepoService<UserInputDTO, User> genSerForUserInputDTO, 
            IGenericRepoService<GetUserDetailsOutputDTO, User> getSerForGetUserDetailsOutputDTO,
            IAuthenticator authenticator)
        {
            _authRepoService = authRepoService;
            _mapper = mapper;
            _genSerForUserInputDTO = genSerForUserInputDTO;
            _getSerForGetUserDetailsOutputDTO = getSerForGetUserDetailsOutputDTO;
            _authenticator = authenticator;
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
                    Response<TokensOutputDTO> registerServiceResponse = await _authRepoService.LoginUserAsync(loginInpulDTO);

                    //check if response has error.
                    if (!registerServiceResponse.IsSuccessfull)
                    {
                        return Ok(registerServiceResponse);
                    }
                    else
                    {
                        return Ok(registerServiceResponse);
                    }
                }
                catch (Exception ex)
                {
                    return Ok(Response<TokensOutputDTO>.Failure(ex.Message));
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

                var refreshToken = Request.Cookies["refreshToken"]; // Get the refresh token from the HTTP-only cookie

                //check if id is null.
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("user id is not available.");
                }

                //get user from User Id.
                Response<GetUserDetailsOutputDTO> getUserResponse = await _getSerForGetUserDetailsOutputDTO.GetAsync(id);

                if(!getUserResponse.IsSuccessfull)
                {
                    return Ok(getUserResponse);
                }

                return Ok(getUserResponse);
            }
            catch(Exception ex)
            {
                return Ok(Response<GetUserDetailsOutputDTO>.Failure(ex.Message));
            }
        }

        [HttpPost]
        [Authorize]
        [Route("update-user")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserInputDTO updateUserInputModelDTO)
        {
            //check if input model is valid or not.
            if (!ModelState.IsValid)
            {
                return Ok("input is not valid");
            }
            else
            {
                try
                {
                    if (!User.Identity.IsAuthenticated)
                    {
                        return Ok("user is not authenticated.");
                    }

                    //get user claims.
                    UserClaimModel loggedInUserClaims = await GetUserClaims();

                    //send Create User Request to service layer.
                    Response<UpdateUserInputDTO> updateUserServiceResponse = await _authRepoService.UpdateUserAsync(updateUserInputModelDTO, loggedInUserClaims);

                    //check if response has error.
                    if (!updateUserServiceResponse.IsSuccessfull)
                    {
                        return Ok(updateUserServiceResponse);
                    }
                    else
                    {
                        return Ok(updateUserServiceResponse);
                    }
                }
                catch (Exception ex)
                {
                    return Ok(Response<UpdateUserInputDTO>.Failure(ex.Message));
                }
            }
        }

        [HttpPost]
        [Route("request-token")]
        public async Task<IActionResult> RequestToken([FromBody] RequestTokenInputDTO tokenRequest)
        {
            //check if the refreshToken Is available in database or not.
            string refreshToken = tokenRequest.RefreshToken;

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest(new { message = "No refresh token provided" });
            }

            //check if refresh token exist in database.
            Response<JwtTokenOutputDTO> foundTokenResponse = await _authRepoService.GetTokenDetailsAsync(refreshToken);

            //check response.
            if(!foundTokenResponse.IsSuccessfull || foundTokenResponse.Value.UserId == null)
            {
                return Ok(foundTokenResponse);
            }

            //Check if the token has expired
            if (foundTokenResponse.Value.RefreshTokenValidityTill < DateTime.UtcNow)
            {
                return Ok(Response<JwtTokenOutputDTO>.Failure("refresh token expired."));
            }

            //find the user from User id.
            Response<GetUserDetailsOutputDTO> foundUserDetailsResponse = await _getSerForGetUserDetailsOutputDTO.GetAsync(foundTokenResponse.Value.UserId);

            if(!foundUserDetailsResponse.IsSuccessfull)
            {
                return Ok(foundUserDetailsResponse);
            }

            //Token is valid, so generate new access and refresh tokens and save in database.
            Response<TokensOutputDTO> generateTokensResponse = await _authRepoService.GetDirectTokensAsync(foundUserDetailsResponse.Value.UserName);

            //check response.
            if(!generateTokensResponse.IsSuccessfull)
            {
                return Ok(generateTokensResponse);
            }

            return Ok(generateTokensResponse);
        }

        private async Task<UserClaimModel> GetUserClaims()
        {
            string? id = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            string? email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            string? userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            string? role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            //create new UserClaimModel
            UserClaimModel userClaimModel = new UserClaimModel()
            {
                Id = id,
                Email = email,
                UserName = userName,
                Role = role
            };

            return userClaimModel;
        }
    }
}
