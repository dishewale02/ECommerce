using ECommerce.Models.DataModels.ProductModel;
using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.ModelDTOs.ProductInputModelDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.GenericRepoServiceInterface;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IGenericRepoService<ProductInputDTO, Product> _genericRepoService;

        public ProductController(IGenericRepoService<ProductInputDTO, Product> createProdcutService)
        {
            _genericRepoService = createProdcutService;
        }

        [HttpPost]
        [Route("create-new-product")]
        public async Task<IActionResult> CreateNewProduct(ProductInputDTO productInputDTO)
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
                    if (!User.Identity.IsAuthenticated)
                    {
                        return Ok("user is not authenticated.");
                    }

                    //get user claims.
                    UserClaimModel loggedInUserClaims = await GetUserClaims();

                    //send Create User Request to service layer.
                    Response<ProductInputDTO> createUserServiceResponse = await _genericRepoService.CreateAsync(productInputDTO, loggedInUserClaims);

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
                    return Ok(Response<ProductInputDTO>.Failure(ex.Message));
                }
            }
            
        }

        [HttpPost]
        [Route("update-product")]
        public async Task<IActionResult> UpdateProduct(ProductInputDTO updateProductDetails)
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
                    Response<ProductInputDTO> updateUserServiceResponse = await _genericRepoService.UpdateAsync(updateProductDetails, loggedInUserClaims);

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
                    return StatusCode(200, new Response<ProductInputDTO>() { ErrorMessage = ex.Message });
                }
            }
        }

        [HttpGet]
        [Route("get-Product")]
        public async Task<IActionResult> GetProduct(string productId)
        {
            try
            {
                //check if input id is null.
                if (string.IsNullOrEmpty(productId))
                {
                    return Ok("input id is null.");
                }

                //send request to the service layer.
                Response<ProductInputDTO> foundUserDetailResponse = await _genericRepoService.GetAsync(productId);

                //check response.
                if (!foundUserDetailResponse.IsSuccessfull)
                {
                    return Ok(foundUserDetailResponse);
                }

                return Ok(foundUserDetailResponse.Value);
            }
            catch (Exception ex)
            {
                return StatusCode(200, new Response<ProductInputDTO>() { ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        [Route("get-all-products")]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                //send request to the service layer.
                Response<IEnumerable<ProductInputDTO>> getAllUsersResponse = await _genericRepoService.GetAllAsync();

                //check response.
                if (!getAllUsersResponse.IsSuccessfull)
                {
                    return Ok(getAllUsersResponse);
                }

                return Ok(getAllUsersResponse.Value);
            }
            catch (Exception ex)
            {
                return StatusCode(200, new Response<ProductInputDTO>() { ErrorMessage = ex.Message });
            }
        }

        [HttpPost]
        [Route("soft-delete-product")]
        public async Task<IActionResult> SoftDeleteProduct(string? productId)
        {
            try
            {
                //check if input id is null.
                if (string.IsNullOrEmpty(productId))
                {
                    return Ok("input id is null.");
                }

                //get user claims.
                UserClaimModel loggedInUserClaims = await GetUserClaims();

                //send request to the service layer.
                Response<ProductInputDTO> foundUserDeleteResponse = await _genericRepoService.SoftDeleteAsync(productId, loggedInUserClaims);

                //check response.
                if (!foundUserDeleteResponse.IsSuccessfull)
                {
                    return Ok(foundUserDeleteResponse);
                }

                return Ok(foundUserDeleteResponse.Value);
            }
            catch (Exception ex)
            {
                return StatusCode(200, new Response<ProductInputDTO>() { ErrorMessage = ex.Message });
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
