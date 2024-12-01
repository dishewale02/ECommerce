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
    [Route("[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepoService _adminService;
        private readonly IGenericRepoService<UserInputDTO, User> _genericRepoService;

        public AdminController(IAdminRepoService adminService, IGenericRepoService<UserInputDTO, User> genericRepoService)
        {
            _adminService = adminService;
            _genericRepoService = genericRepoService;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        [Route("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] UserInputDTO userInputDTO)
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
                    if(!User.Identity.IsAuthenticated)
                    {
                        return Ok("user is not authenticated.");
                    }

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
                    
                    //send Create User Request to service layer.
                    Response<UserInputDTO> createUserServiceResponse = await _adminService.CreateAsync(userInputDTO, userClaimModel);

                    //check if response has error.
                    if (!createUserServiceResponse.IsSuccessfull)
                    {
                        return Ok(createUserServiceResponse);
                    }
                    else
                    {
                        return Ok(createUserServiceResponse.Value);
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(200, new Response<UserInputDTO>() { ErrorMessage = ex.Message });
                }
            }
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        [Route("update-user")]
        public async Task<IActionResult> UpdateUser([FromBody] UserInputDTO updateUserInputModelDTO)
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
                    Response<UserInputDTO> updateUserServiceResponse = await _adminService.UpdateAsync(updateUserInputModelDTO, loggedInUserClaims);

                    //check if response has error.
                    if (!updateUserServiceResponse.IsSuccessfull)
                    {
                        return Ok(updateUserServiceResponse);
                    }
                    else
                    {
                        return Ok(updateUserServiceResponse.Value);
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(200, new Response<UserInputDTO>() { ErrorMessage = ex.Message });
                }
            }
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [Route("get-user")]
        public async Task<IActionResult> GetUser(string? userId)
        {
            try
            {
                //check if input id is null.
                if(string.IsNullOrEmpty(userId))
                {
                    return Ok("input id is null.");
                }

                //send request to the service layer.
                Response<UserInputDTO> foundUserDetailResponse = await _genericRepoService.GetAsync(userId);

                //check response.
                if (!foundUserDetailResponse.IsSuccessfull)
                {
                    return Ok(foundUserDetailResponse);
                }

                return Ok(foundUserDetailResponse.Value);
            }
            catch (Exception ex)
            {
                return StatusCode(200, new Response<UserInputDTO>() { ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [Route("get-all-nondeleted-active-users")]
        public async Task<IActionResult> GetAllNonDeletedAndActiveUsers()
        {
            try
            {
                //send request to the service layer.
                Response<List<UserInputDTO>> getAllUsersResponse = await _genericRepoService.GetAllAsync();

                //check response.
                if (!getAllUsersResponse.IsSuccessfull)
                {
                    return Ok(getAllUsersResponse);
                }

                return Ok(getAllUsersResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(200, new Response<UserInputDTO>() { ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [Route("get-all-deleted-nonactive-users")]
        public async Task<IActionResult> GetAllDeletedAndNonActiveUsers()
        {
            try
            {
                //send request to the service layer.
                Response<List<UserInputDTO>> getAllUsersResponse = await _adminService.GetAllDeletedAndNonActiveUsers();

                //check response.
                if (!getAllUsersResponse.IsSuccessfull)
                {
                    return Ok(getAllUsersResponse);
                }

                return Ok(getAllUsersResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(200, new Response<UserInputDTO>() { ErrorMessage = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        [Route("delete-user")]
        public async Task<IActionResult> DeleteUser([FromQuery] string? userId)
        {
            try
            {
                //check if input id is null.
                if (string.IsNullOrEmpty(userId))
                {
                    return Ok("input id is null.");
                }

                //send request to the service layer.
                Response<UserInputDTO> foundUserDeleteResponse = await _genericRepoService.DeleteAsync(userId);

                //check response.
                if (!foundUserDeleteResponse.IsSuccessfull)
                {
                    return Ok(foundUserDeleteResponse);
                }

                return Ok(foundUserDeleteResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(200, new Response<UserInputDTO>() { ErrorMessage = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        [Route("activate-deleted-user")]
        public async Task<IActionResult> ActivateDeletedUser([FromQuery] string? userId)
        {
            try
            {
                //check if input id is null.
                if (string.IsNullOrEmpty(userId))
                {
                    return Ok("input id is null.");
                }

                //send request to the service layer.
                Response<UserInputDTO> foundUserDeleteResponse = await _adminService.ActivateDeletedUserAsync(userId);

                //check response.
                if (!foundUserDeleteResponse.IsSuccessfull)
                {
                    return Ok(foundUserDeleteResponse);
                }

                return Ok(foundUserDeleteResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(200, new Response<UserInputDTO>() { ErrorMessage = ex.Message });
            }
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
