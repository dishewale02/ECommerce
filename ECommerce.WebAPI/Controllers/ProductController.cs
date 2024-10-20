using ECommerce.Models.DataModels.ProductModel;
using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.ModelDTOs.ProductInputModelDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Services.Classes.RepoServiceClasses.ProductRepoServiceClass;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.GenericRepoServiceInterface;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.ProductRepoServiceInterface;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IGenericRepoService<ProductInputDTO, Product> _genericRepoService;
        private readonly IProductRepoService _productRepoService;
        private readonly IWebHostEnvironment _env;

        public ProductController(IGenericRepoService<ProductInputDTO, Product> createProdcutService, IProductRepoService productRepoService, IWebHostEnvironment env)
        {
            _genericRepoService = createProdcutService;
            _productRepoService = productRepoService;
            _env = env;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewProduct([FromForm] ProductInputDTO productInputDTO)
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
                    //if (!User.Identity.IsAuthenticated)
                    //{
                    //    return Ok("user is not authenticated.");
                    //}

                    if(productInputDTO.Images is null || !productInputDTO.Images.Any())
                    {
                        return Ok("image should be present.");
                    }

                    // Folder where images will be saved
                    string uploadPath = Path.Combine(_env.WebRootPath, "ProdImages");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    //save uploadPaths into DTO image paths.
                    productInputDTO.ImagePaths = uploadPath;
                    
                    //get user claims.
                    UserClaimModel loggedInUserClaims = await GetUserClaims();

                    //send Create User Request to service layer.
                    Response<ProductInputDTO> createUserServiceResponse = await _productRepoService.CreateAsync(productInputDTO, loggedInUserClaims);

                    //check if response has error.
                    if (!createUserServiceResponse.IsSuccessfull)
                    {
                        return Ok(createUserServiceResponse);
                    }
                    else
                    {
                        return Ok(createUserServiceResponse);
                    }
                }
                catch (Exception ex)
                {
                    return Ok(Response<ProductInputDTO>.Failure(ex.Message));
                }
            }
            
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateProduct(string productId, [FromBody] ProductInputDTO updateProductDetails)
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

        [HttpGet("{productId}")]
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

                return Ok(foundUserDetailResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(200, new Response<ProductInputDTO>() { ErrorMessage = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts(string? category)
        {
            try
            {
                Response<IEnumerable<ProductInputDTO>>? getAllUsersResponse = null;

                //send request to the service layer.
                if (string.IsNullOrEmpty(category))
                {
                    getAllUsersResponse = await _genericRepoService.GetAllAsync();
                }
                else
                {
                    getAllUsersResponse = await _productRepoService.GetProductsByCategoryServiceAsync(category);
                }

                //check response.
                if (!getAllUsersResponse.IsSuccessfull)
                {
                    return Ok(getAllUsersResponse);
                }
                    
                return Ok(getAllUsersResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(200, new Response<ProductInputDTO>() { ErrorMessage = ex.Message });
            }
        }

        [HttpDelete("{productId}")]
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

        //[HttpGet]
        //[Route("get-products-by-category")]
        //public async Task<IActionResult> GetProductsByCategory(string categoryName)
        //{
        //    try
        //    {
        //        //check if input id is null.
        //        if (string.IsNullOrEmpty(categoryName))
        //        {
        //            return Ok("input category is null.");
        //        }

        //        //fetch all products if categoryName is All.
        //        if(categoryName == "All")
        //        {
        //            Response<IEnumerable<ProductInputDTO>> getAllProductList = await _genericRepoService.GetAllAsync();

        //            //check response.
        //            if(!getAllProductList.IsSuccessfull)
        //            {
        //                return Ok(getAllProductList);
        //            }

        //            return Ok(getAllProductList);
        //        }

        //        //send request to the service layer.
        //        Response<IEnumerable<ProductInputDTO>> foundProductsByCategory = await _productRepoService.GetProductsByCategoryServiceAsync(categoryName);

        //        //check response.
        //        if (!foundProductsByCategory.IsSuccessfull)
        //        {
        //            return Ok(foundProductsByCategory);
        //        }

        //        return Ok(foundProductsByCategory);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(200, new Response<ProductInputDTO>() { ErrorMessage = ex.Message });
        //    }
        //}

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
