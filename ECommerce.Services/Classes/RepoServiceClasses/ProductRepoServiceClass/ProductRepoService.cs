
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

        public async Task<Response<List<ProductDTO>>> GetAllSearchedProductsAsync(string searchString)
        {
            try
            {
                //check input string.
                if (string.IsNullOrEmpty(searchString))
                {
                    return Response<List<ProductDTO>>.Failure("input search field is empty.");
                }

                //send the string to search in database.
                Response<List<Product>> searchedProductsByStringResponse = await _productRepo.RSearchProductAsync(searchString);

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
    }
}
