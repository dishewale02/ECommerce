using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.InputModelsDTO.AuthInputModelsDTO;
using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.GenericRepoServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IGenericRepoService<UserInputDTO, User> _genericRepoService;

        public UserController(IGenericRepoService<UserInputDTO, User> genericRepoService)
        {
            _genericRepoService = genericRepoService;
        }

        [HttpPut("{productId}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(string productId, [FromBody] UserInputDTO userInputDTO)
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
                    Response<UserInputDTO> createUserServiceResponse = await _genericRepoService.UpdateAsync(userInputDTO, userClaimModel);

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
    }
}
