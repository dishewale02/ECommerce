
using AutoMapper;
using ECommerce.Models.DataModels.ProductModel;
using ECommerce.Models.ModelDTOs.ProductInputModelDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Repo.Interfaces.GenericRepoInterface;
using ECommerce.Repo.Interfaces.ProductRepoInterface;
using ECommerce.Services.Classes.RepoServiceClasses.GenericRepoServiceClass;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.ProductRepoServiceInterface;

namespace ECommerce.Services.Classes.RepoServiceClasses.ProductRepoServiceClass
{
    public class ProductRepoService : GenericRepoService<ProductDTO, Product>, IProductRepoService
    {
        private readonly IProductRepo _productRepo;


        public ProductRepoService(IGenericRepo<Product> genericRepo, IMapper mapper, IProductRepo productRepo) : base(genericRepo, mapper)
        {
            _productRepo = productRepo;
        }

        public async Task<Response<List<ProductDTO>>> GetAllSearchedProductsAsync(string category, string searchString)
        {
            try
            {
                //send the string to search in database.
                Response<List<Product>> searchedProductsByStringResponse = await _productRepo.RSearchProductAsync(category, searchString);

                //check output response.
                if (!searchedProductsByStringResponse.IsSuccessfull)
                {
                    return Response<List<ProductDTO>>.Failure(searchedProductsByStringResponse.ErrorMessage);
                }

                //create new List to store ProductsDTO.
                List<ProductDTO> searchedProductsDTOs = new List<ProductDTO>();

                //convert all products into ProductsDTO model.
                foreach (Product product in searchedProductsByStringResponse.Value)
                {
                    try
                    {
                        //mapp the Product to ProductDTO
                        ProductDTO mappedProductToProductDTO = _mapper.Map<ProductDTO>(product);

                        //save productDTO into List.
                        searchedProductsDTOs.Add(mappedProductToProductDTO);
                    }
                    catch (Exception ex)
                    {
                        return Response<List<ProductDTO>>.Failure(ex.Message);
                    }
                }

                //check response list.
                if (!searchedProductsDTOs.Any())
                {
                    return Response<List<ProductDTO>>.Failure("no Product Found.");
                }

                return Response<List<ProductDTO>>.Success(searchedProductsDTOs);

            }
            catch(Exception ex)
            {
                return Response<List<ProductDTO>>.Failure(ex.Message);
            }
        }

        public async Task<Response<List<ProductDTO>>> GetProductsByCategoryId(string categoryId)
        {
            try
            {
                //check input string.
                if (string.IsNullOrEmpty(categoryId))
                {
                    return Response<List<ProductDTO>>.Failure("input search field is empty.");
                }

                //send the string to search in database.
                Response<List<Product>> searchedProductsByCategoryIdResponse = await _productRepo.RSearchProductsByCategoryIdAsync(categoryId);

                //check output response.
                if (!searchedProductsByCategoryIdResponse.IsSuccessfull)
                {
                    return Response<List<ProductDTO>>.Failure(searchedProductsByCategoryIdResponse.ErrorMessage);
                }

                //create new List to store ProductsDTO.
                List<ProductDTO> searchedProductsDTOs = new List<ProductDTO>();

                //convert all products into ProductsDTO model.
                foreach (Product product in searchedProductsByCategoryIdResponse.Value)
                {
                    try
                    {
                        //mapp the Product to ProductDTO
                        ProductDTO mappedProductToProductDTO = _mapper.Map<ProductDTO>(product);

                        //save productDTO into List.
                        searchedProductsDTOs.Add(mappedProductToProductDTO);
                    }
                    catch (Exception ex)
                    {
                        return Response<List<ProductDTO>>.Failure(ex.Message);
                    }
                }

                //check response list.
                if (!searchedProductsDTOs.Any())
                {
                    return Response<List<ProductDTO>>.Failure("no Product Found.");
                }

                return Response<List<ProductDTO>>.Success(searchedProductsDTOs);

            }
            catch (Exception ex)
            {
                return Response<List<ProductDTO>>.Failure(ex.Message);
            }
        }

        public async Task<Response<List<ProductDTO>>> GetDeletedAndNonActiveProducts()
        {
            try
            {
                //send request to data layer.
                Response<List<Product>> deletedAndNonActiveProductsResponse = await _productRepo.RGetDeletedAndNonActiveProducts();

                //check Response to return
                if (!deletedAndNonActiveProductsResponse.IsSuccessfull)
                {
                    return Response<List<ProductDTO>>.Failure(deletedAndNonActiveProductsResponse.ErrorMessage);
                }

                //create new response list.
                List<ProductDTO> productsResponse = new List<ProductDTO>();

                //map and save entry in new list.
                foreach (Product product in deletedAndNonActiveProductsResponse.Value)
                {
                    ProductDTO mappedProduct = _mapper.Map<ProductDTO>(product);

                    productsResponse.Add(mappedProduct);
                }

                return Response<List<ProductDTO>>.Success(productsResponse);
            }
            catch (Exception ex)
            {
                return Response<List<ProductDTO>>.Failure(ex.Message);
            }
        }

        public async Task<Response<ProductDTO>> ActivateDeletedProduct(string productid)
        {
            try
            {
                //check if the product available in database.
                Response<Product> isProductAvailableInDatabaseResponse = await _productRepo.RGetAsync(productid);

                //check response.
                if (!isProductAvailableInDatabaseResponse.IsSuccessfull)
                {
                    return Response<ProductDTO>.Failure(isProductAvailableInDatabaseResponse.ErrorMessage);
                }

                //update the product state.
                isProductAvailableInDatabaseResponse.Value.IsActive = true;
                isProductAvailableInDatabaseResponse.Value.IsDeleted = false;

                //sent product to update in database.
                Response<Product> updatedProductResponse = await _productRepo.RUpdateAsync(isProductAvailableInDatabaseResponse.Value);

                //check response.
                if (!updatedProductResponse.IsSuccessfull)
                {
                    return Response<ProductDTO>.Failure(updatedProductResponse.ErrorMessage);
                }

                ProductDTO activattedProduct = _mapper.Map<ProductDTO>(updatedProductResponse.Value);

                return Response<ProductDTO>.Success(activattedProduct);
            }
            catch (Exception ex)
            {
                return Response<ProductDTO>.Failure(ex.Message);
            }
        }
    }
}
