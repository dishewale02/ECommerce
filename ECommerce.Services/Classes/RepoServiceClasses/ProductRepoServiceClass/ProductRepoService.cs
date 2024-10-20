
using AutoMapper;
using ECommerce.Models.DataModels.ProductModel;
using ECommerce.Models.InputModelsDTO.AuthOutputModelDTO;
using ECommerce.Models.ModelDTOs.ProductInputModelDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Repo.Interfaces.GenericRepoInterface;
using ECommerce.Repo.Interfaces.ProductRepoInterface;
using ECommerce.Services.Classes.RepoServiceClasses.GenericRepoServiceClass;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.ProductRepoServiceInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Reflection.Metadata.Ecma335;

namespace ECommerce.Services.Classes.RepoServiceClasses.ProductRepoServiceClass
{
    public class ProductRepoService : GenericRepoService<ProductInputDTO, Product>, IProductRepoService
    {
        private readonly IProductRepo _productRepo;
        private readonly IGenericRepo<Category> _genericRepoCategory;

        public ProductRepoService(IGenericRepo<Product> genericRepoProduct, IMapper mapper, IProductRepo productRepo, IGenericRepo<Category> genericRepoCategory) : base(genericRepoProduct, mapper)
        {
            _productRepo = productRepo;
            _genericRepoCategory = genericRepoCategory;
        }

        public async Task<Response<IEnumerable<ProductInputDTO>>> GetProductsByCategoryServiceAsync(string categoryName)
        {
            try
            {
                //check if category name is null.
                if (categoryName == null)
                {
                    return Response<IEnumerable<ProductInputDTO>>.Failure("category can not null.");
                }

                //send category to get all related product list.
                Response<IEnumerable<Product>> foundCategoryResponse = await _productRepo.GetProductsByCategoryRepoAsync(categoryName);

                //check response
                if(!foundCategoryResponse.IsSuccessfull)
                {
                    return Response<IEnumerable<ProductInputDTO>>.Failure(foundCategoryResponse.ErrorMessage);
                }

                //convert all Product to ProductInputDTO.
                List<ProductInputDTO> mappedProductList = new List<ProductInputDTO>();
                foreach (Product product in foundCategoryResponse.Value)
                {
                    ProductInputDTO mappedProductDTO = _mapper.Map<ProductInputDTO>(product);
                    mappedProductList.Add(mappedProductDTO);
                }

                return Response<IEnumerable<ProductInputDTO>>.Success(mappedProductList);
            }
            catch (Exception ex)
            {
                return Response<IEnumerable<ProductInputDTO>>.Failure(ex.Message);
            }

        }

        public override async Task<Response<ProductInputDTO>> CreateAsync(ProductInputDTO productInputDTO, UserClaimModel userClaimModel)
        {
            try
            {
                if(productInputDTO is null || productInputDTO.Images is null)
                {
                    return Response<ProductInputDTO>.Failure("input product can not blank.");
                }

                // Save each image and get the paths
                var imagePaths = productInputDTO.Images.Select(img => {
                    if (img is not null)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + img.FileName;
                        var filePath = Path.Combine(productInputDTO.ImagePaths, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            img.CopyTo(fileStream);
                        }

                        return $"/ProdImages/{uniqueFileName}"; // Return relative path
                    }
                    return "";
                }).ToList();

                //find the category information from productDTO category Id.
                Response<Category> foundCategory = await _genericRepoCategory.RGetAsync(productInputDTO.CategoryId);

                if (!foundCategory.IsSuccessfull)
                {
                    return Response<ProductInputDTO>.Failure(foundCategory.ErrorMessage);
                }

                // Create new Product entity
                Product product = new Product
                {
                    Name = productInputDTO.Name,
                    Price = productInputDTO.Price,
                    Description = productInputDTO.Description,
                    CategoryId = foundCategory.Value.Id,
                    ImagePaths = string.Join(";", imagePaths) // Store as a semicolon-separated string in DB
                };

                //var imagePath = Path.Combine("wwwroot/ProdImages", productInputDTO.Image.FileName);

                //using (var stream = new FileStream(imagePath, FileMode.Create))
                //{
                //    await productInputDTO.Image.CopyToAsync(stream);
                //}

                //send entity to mapp.
                Product tSourceToTTargetMapped = _mapper.Map<ProductInputDTO, Product>(productInputDTO);

                //fill all the required information.
                product.Id = Guid.NewGuid().ToString();
                product.CreatedOn = DateTime.Now;

                product.CreatedBy = userClaimModel.UserName;
                product.IsDeleted = false;
                product.IsActive = true;

                //send entity to save in database.
                Response<Product> saveEntityInDatabaseResponse = await _genericRepo.RCreateAsync(product);

                //mapp response entity to TTarget model.

                ProductInputDTO targetEntityMapped = _mapper.Map<Product, ProductInputDTO>(saveEntityInDatabaseResponse.Value);

                //check if response is successfull.
                if (!saveEntityInDatabaseResponse.IsSuccessfull)
                {
                    return Response<ProductInputDTO>.Failure(saveEntityInDatabaseResponse.ErrorMessage);
                }

                return Response<ProductInputDTO>.Success(targetEntityMapped);
            }
            catch (Exception ex)
            {
                return Response<ProductInputDTO>.Failure(ex.Message);
            }
        }
    }
}
