using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.ModelDTOs.CategoryModelDTO;
using ECommerce.Models.ModelDTOs.ProductInputModelDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.CategoryRepoServiceInterface;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.ProductRepoServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepoService _productRepoService;
        private readonly ICategoryRepoService _categoryRepoService;

        public ProductController(IProductRepoService productRepoService, ICategoryRepoService categoryRepoService)
        {
            _productRepoService = productRepoService;
            _categoryRepoService = categoryRepoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetNonDeletedAndActiveProducts()
        {
            try
            {
                //send the request to service layer.
                Response<List<ProductDTO>> getAllProductsRequestResponse = await _productRepoService.GetAllAsync();

                //check response
                if (!getAllProductsRequestResponse.IsSuccessfull)
                {
                    return Ok(Response<string>.Failure(getAllProductsRequestResponse.ErrorMessage));
                }

                return Ok(getAllProductsRequestResponse);
            }
            catch (Exception ex)
            {
                return Ok(Response<string>.Failure(ex.Message));
            }
        }

        [HttpGet]
        [Route("get-deleted-and-non-active-products")]
        public async Task<IActionResult> GetDeletedAndNonActiveProducts()
        {
            try
            {
                //send the request to service layer.
                Response<List<ProductDTO>> getAllProductsRequestResponse = await _productRepoService.GetDeletedAndNonActiveProducts();

                //check response
                if (!getAllProductsRequestResponse.IsSuccessfull)
                {
                    return Ok(Response<string>.Failure(getAllProductsRequestResponse.ErrorMessage));
                }

                return Ok(getAllProductsRequestResponse);
            }
            catch (Exception ex)
            {
                return Ok(Response<string>.Failure(ex.Message));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            try
            {
                //check if the id is null.
                if (string.IsNullOrEmpty(id))
                {
                    return Ok(Response<string>.Failure("input id can not be null"));
                }

                //send the request to service layer.
                Response<ProductDTO> getProductRequestResponse = await _productRepoService.GetAsync(id);

                //check response
                if (!getProductRequestResponse.IsSuccessfull)
                {
                    return Ok(Response<IEnumerable<ProductDTO>>.Failure(getProductRequestResponse.ErrorMessage));
                }

                return Ok(getProductRequestResponse);
            }
            catch (Exception ex)
            {
                return Ok(Response<string>.Failure(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> DeleteProductById([FromQuery]string id)
        {
            try
            {
                //check if the id is null.
                if (string.IsNullOrEmpty(id))
                {
                    return Ok(Response<string>.Failure("input id can not be null"));
                }

                //send the request to service layer.
                Response<ProductDTO> getProductRequestResponse = await _productRepoService.DeleteAsync(id);

                //check response
                if (!getProductRequestResponse.IsSuccessfull)
                {
                    return Ok(Response<ProductDTO>.Failure(getProductRequestResponse.ErrorMessage));
                }

                return Ok(getProductRequestResponse);
            }
            catch (Exception ex)
            {
                return Ok(Response<string>.Failure(ex.Message));
            }
        }

        [HttpDelete]
        [Route("activate-deleted-product")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ActivateDeletedProductById([FromQuery] string id)
        {
            try
            {
                //check if the id is null.
                if (string.IsNullOrEmpty(id))
                {
                    return Ok(Response<string>.Failure("input id can not be null"));
                }

                //send the request to service layer.
                Response<ProductDTO> getProductRequestResponse = await _productRepoService.ActivateDeletedProduct(id);

                //check response
                if (!getProductRequestResponse.IsSuccessfull)
                {
                    return Ok(Response<ProductDTO>.Failure(getProductRequestResponse.ErrorMessage));
                }

                return Ok(getProductRequestResponse);
            }
            catch (Exception ex)
            {
                return Ok(Response<string>.Failure(ex.Message));
            }
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateNewProduct([FromForm] ProductDTO createProductDTO)
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
                Response<ProductDTO> createNewProductResponse = await _productRepoService.CreateAsync(createProductDTO, userClaimResponse);

                //check response
                if (!createNewProductResponse.IsSuccessfull)
                {
                    return Ok(Response<IEnumerable<ProductDTO>>.Failure(createNewProductResponse.ErrorMessage));
                }

                return Ok(createNewProductResponse);
            }
            catch (Exception ex)
            {
                return Ok(Response<string>.Failure(ex.Message));
            }
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductDTO updateProductDTO)
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
                Response<ProductDTO> updateProductResponse = await _productRepoService.UpdateAsync(updateProductDTO, userClaimResponse);

                //check response
                if (!updateProductResponse.IsSuccessfull)
                {
                    return Ok(Response<IEnumerable<ProductDTO>>.Failure(updateProductResponse.ErrorMessage));
                }

                return Ok(updateProductResponse);
            }
            catch (Exception ex)
            {
                return Ok(Response<string>.Failure(ex.Message));
            }
        }

        //[HttpPost("find")]
        //public async Task<IActionResult> SearchProductByField([FromForm] string searchString)
        //{
        //    try
        //    {
        //        //check if the id is null.
        //        if (string.IsNullOrEmpty(searchString))
        //        {
        //            return Ok(Response<string>.Failure("input id can not be null"));
        //        }

        //        //send the request to service layer.
        //        Response<List<ProductDTO>> getProductRequestResponse = await _productRepoService.GetAllSearchedProductsAsync(searchString);

        //        //check response
        //        if (!getProductRequestResponse.IsSuccessfull)
        //        {
        //            return Ok(Response<IEnumerable<ProductDTO>>.Failure(getProductRequestResponse.ErrorMessage));
        //        }

        //        return Ok(getProductRequestResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(Response<string>.Failure(ex.Message));
        //    }
        //}

        [HttpGet("search-product")]
        public async Task<IActionResult> SearchProducts([FromQuery] string category, [FromQuery] string searchField)
        {
            try
            {
                //check if the category is null.
                if (string.IsNullOrEmpty(category))
                {
                    return Ok(Response<List<ProductDTO>>.Failure("input category can not be null"));
                }

                //Case: 1
                if (category == "all" && searchField == "null")
                {
                    Response<List<ProductDTO>> allProductsResponse = await _productRepoService.GetAllAsync();

                    //check response.
                    if (!allProductsResponse.IsSuccessfull)
                    {
                        return Ok(Response<List<ProductDTO>>.Failure(allProductsResponse.ErrorMessage));
                    }

                    return Ok(allProductsResponse);
                }
                //Case: 2
                else if (category != "all" && category != "null" && (searchField == " null" || searchField == "null"))
                {
                    Response<CategoryDTO> getCategoryByCategoryNameResponse = await _categoryRepoService.GetByCategoryNameAsync(category);

                    //check the find category info response
                    if (!getCategoryByCategoryNameResponse.IsSuccessfull || getCategoryByCategoryNameResponse.Value.Id == null)
                    {
                        return Ok(Response<List<ProductDTO>>.Failure(getCategoryByCategoryNameResponse.ErrorMessage));
                    }

                    //send the request to service layer.
                    Response<List<ProductDTO>> getProductRequestResponse = await _productRepoService.GetProductsByCategoryId(getCategoryByCategoryNameResponse.Value.Id);

                    //check response
                    if (!getProductRequestResponse.IsSuccessfull)
                    {
                        return Ok(Response<IEnumerable<ProductDTO>>.Failure(getProductRequestResponse.ErrorMessage));
                    }

                    return Ok(getProductRequestResponse);
                }
                //Case: 3
                else if (category == "all" && searchField != "null")
                {
                    Response<List<ProductDTO>> productListResponse = await _productRepoService.GetAllSearchedProductsAsync(category, searchField);
                    if (!productListResponse.IsSuccessfull)
                    {
                        return Ok(Response<List<ProductDTO>>.Failure(productListResponse.ErrorMessage));
                    }
                    return Ok(productListResponse);
                }
                //Case: 4
                else if ((category != "all" || category != "null") && (searchField != " null" || searchField != "null"))
                {
                    Response<List<ProductDTO>> productListResponse = await _productRepoService.GetAllSearchedProductsAsync(category, searchField);
                    if (!productListResponse.IsSuccessfull)
                    {
                        return Ok(Response<List<ProductDTO>>.Failure(productListResponse.ErrorMessage));
                    }
                    return Ok(productListResponse);
                }
                else 
                {
                    return Ok(new Response<List<ProductDTO>> { ErrorMessage = "no product found." });
                }
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
