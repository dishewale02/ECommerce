using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.DataModels.ProductModel;
using ECommerce.Models.InputModelsDTO.AuthInputModelsDTO;
using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.ModelDTOs.CategoryInputModelDTO;
using ECommerce.Models.ModelDTOs.ProductInputModelDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.GenericRepoServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IGenericRepoService<CategoryInputDTO, Category> _genericRepoService;

        public CategoryController(IGenericRepoService<CategoryInputDTO, Category> createProdcutService)
        {
            _genericRepoService = createProdcutService;
        }

        [HttpPost]
        [Route("create-new-category")]
        public async Task<IActionResult> CreateNewCategory(CategoryInputDTO categoryInputDTO)
        {
            //check model state.
            if (!ModelState.IsValid)
            {
                return Ok("input is not valid");
            }
            else
            {
                try
                {
                    //get user claims.
                    UserClaimModel loggedInUserClaims = await GetUserClaims();

                    //send Create User Request to service layer.
                    Response<CategoryInputDTO> createUserServiceResponse = await _genericRepoService.CreateAsync(categoryInputDTO, loggedInUserClaims);

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
                    return Ok(Response<CategoryInputDTO>.Failure(ex.Message));
                }
            }
            
        }

        [HttpPost]
        [Route("update-category")]
        public async Task<IActionResult> UpdateCategory(CategoryInputDTO updateCategoryDetails)
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
                    Response<CategoryInputDTO> updateUserServiceResponse = await _genericRepoService.UpdateAsync(updateCategoryDetails, loggedInUserClaims);

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
                    return StatusCode(200, new Response<CategoryInputDTO>() { ErrorMessage = ex.Message });
                }
            }
        }

        [HttpGet]
        [Route("get-category")]
        public async Task<IActionResult> GetCategory(string categoryId)
        {
            try
            {
                //check if input id is null.
                if (string.IsNullOrEmpty(categoryId))
                {
                    return Ok("input id is null.");
                }

                //send request to the service layer.
                Response<CategoryInputDTO> foundUserDetailResponse = await _genericRepoService.GetAsync(categoryId);

                //check response.
                if (!foundUserDetailResponse.IsSuccessfull)
                {
                    return Ok(foundUserDetailResponse);
                }

                return Ok(foundUserDetailResponse.Value);
            }
            catch (Exception ex)
            {
                return StatusCode(200, new Response<CategoryInputDTO>() { ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("get-all-categories")]
        public async Task<IActionResult> GetAllCategory()
        {
            try
            {
                //send request to the service layer.
                Response<IEnumerable<CategoryInputDTO>> getAllUsersResponse = await _genericRepoService.GetAllAsync();

                //check response.
                if (!getAllUsersResponse.IsSuccessfull)
                {
                    return Ok(getAllUsersResponse);
                }

                return Ok(getAllUsersResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(200, new Response<CategoryInputDTO>() { ErrorMessage = ex.Message });
            }
        }

        [HttpPost]
        [Route("soft-delete-category")]
        public async Task<IActionResult> SoftDeleteCategory(string? categoryId)
        {
            try
            {
                //check if input id is null.
                if (string.IsNullOrEmpty(categoryId))
                {
                    return Ok("input id is null.");
                }

                //get user claims.
                UserClaimModel loggedInUserClaims = await GetUserClaims();

                //send request to the service layer.
                Response<CategoryInputDTO> foundUserDeleteResponse = await _genericRepoService.SoftDeleteAsync(categoryId, loggedInUserClaims);

                //check response.
                if (!foundUserDeleteResponse.IsSuccessfull)
                {
                    return Ok(foundUserDeleteResponse);
                }

                return Ok(foundUserDeleteResponse.Value);
            }
            catch (Exception ex)
            {
                return StatusCode(200, new Response<CategoryInputDTO>() { ErrorMessage = ex.Message });
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
