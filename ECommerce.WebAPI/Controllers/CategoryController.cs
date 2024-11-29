using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.ModelDTOs.CategoryModelDTO;
using ECommerce.Models.ModelDTOs.ProductInputModelDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.CategoryRepoServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController: ControllerBase
    {
        private readonly ICategoryRepoService _categoryRepoService;

        public CategoryController(ICategoryRepoService categoryRepoService)
        {
            _categoryRepoService = categoryRepoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                //send the request to service layer.
                Response<List<CategoryDTO>> getAllCategoryRequestResponse = await _categoryRepoService.GetAllAsync();

                //check response
                if (!getAllCategoryRequestResponse.IsSuccessfull)
                {
                    return Ok(Response<string>.Failure(getAllCategoryRequestResponse.ErrorMessage));
                }

                return Ok(getAllCategoryRequestResponse);
            }
            catch (Exception ex)
            {
                return Ok(Response<string>.Failure(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            try
            {
                //check if the id is null.
                if (string.IsNullOrEmpty(id))
                {
                    return Ok(Response<string>.Failure("input id can not be null"));
                }

                //send the request to service layer.
                Response<CategoryDTO> getCategorytRequestResponse = await _categoryRepoService.GetAsync(id);

                //check response
                if (!getCategorytRequestResponse.IsSuccessfull)
                {
                    return Ok(Response<string>.Failure(getCategorytRequestResponse.ErrorMessage));
                }

                return Ok(getCategorytRequestResponse);
            }
            catch (Exception ex)
            {
                return Ok(Response<string>.Failure(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryById(string id)
        {
            try
            {
                //check if the id is null.
                if (string.IsNullOrEmpty(id))
                {
                    return Ok(Response<string>.Failure("input id can not be null"));
                }

                //send the request to service layer.
                Response<CategoryDTO> getCategoryRequestResponse = await _categoryRepoService.DeleteAsync(id);

                //check response
                if (!getCategoryRequestResponse.IsSuccessfull)
                {
                    return Ok(Response<string>.Failure(getCategoryRequestResponse.ErrorMessage));
                }

                return Ok(getCategoryRequestResponse);
            }
            catch (Exception ex)
            {
                return Ok(Response<string>.Failure(ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateNewCategory([FromForm] CategoryDTO createCategoryDTO)
        {
            try
            {
                //check if the id is null.
                if (!ModelState.IsValid)
                {
                    return Ok(Response<string>.Failure("input model state is not valid"));
                }

                //get current user claims.
                UserClaimModel userClaimResponse = await GetUserClaims();

                //send the request to service layer.
                Response<CategoryDTO> createNewCategoryResponse = await _categoryRepoService.CreateAsync(createCategoryDTO, userClaimResponse);

                //check response
                if (!createNewCategoryResponse.IsSuccessfull)
                {
                    return Ok(Response<string>.Failure(createNewCategoryResponse.ErrorMessage));
                }

                return Ok(createNewCategoryResponse);
            }
            catch (Exception ex)
            {
                return Ok(Response<string>.Failure(ex.Message));
            }
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateCategory([FromForm] CategoryDTO updateCategoryDTO)
        {
            try
            {
                //check if the id is null.
                if (!ModelState.IsValid)
                {
                    return Ok(Response<string>.Failure("input model state is not valid"));
                }

                //get current user claims.
                UserClaimModel userClaimResponse = await GetUserClaims();

                //send the request to service layer.
                Response<CategoryDTO> updateProductResponse = await _categoryRepoService.UpdateAsync(updateCategoryDTO, userClaimResponse);

                //check response
                if (!updateProductResponse.IsSuccessfull)
                {
                    return Ok(Response<string>.Failure(updateProductResponse.ErrorMessage));
                }

                return Ok(updateProductResponse);
            }
            catch (Exception ex)
            {
                return Ok(Response<string>.Failure(ex.Message));
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
