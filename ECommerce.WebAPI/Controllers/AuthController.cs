using ECommerce.Models.InputModelsDTO.AuthInputModelsDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Services.RepoServiceInterfaces.AuthRepoServiceInterface;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepoService _authRepoService;

        public AuthController(IAuthRepoService authRepoService)
        {
            _authRepoService = authRepoService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterInputDTO registerModel)
        {
            //check if input model is valid or not.
            if(!ModelState.IsValid)
            {
                return BadRequest("input is not valid");
            }
            else
            {
                try
                {
                    //send Register Input Model to service layer.
                    Response<RegisterInputDTO> registerServiceResponse = await _authRepoService.RegisterUserAsync(registerModel);

                    //check if response has error.
                    if(registerServiceResponse.Value is null)
                    {
                        return StatusCode(500, registerServiceResponse.ErrorMessage);
                    }
                    else
                    {
                        return Ok(registerServiceResponse.Value);
                    }
                }
                catch(Exception ex) 
                {
                    return StatusCode(500, ex.Message);
                }
            }
        }
    }
}
