using AutoMapper;
using ECommerce.Models.DataModels.ProductModel;
using ECommerce.Models.ModelDTOs.CategoryModelDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Repo.Interfaces.CategoryRepoInterface;
using ECommerce.Repo.Interfaces.GenericRepoInterface;
using ECommerce.Services.Classes.RepoServiceClasses.GenericRepoServiceClass;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.CategoryRepoServiceInterface;

namespace ECommerce.Services.Classes.RepoServiceClasses.CategoryRepoServiceClass
{
    public class CategoryRepoService : GenericRepoService<CategoryDTO, Category>, ICategoryRepoService
    {
        private readonly ICategoryRepo _categoryRepo;

        public CategoryRepoService(IGenericRepo<Category> genericRepo, IMapper mapper, ICategoryRepo categoryRepo) : base(genericRepo, mapper)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<Response<CategoryDTO>> GetByCategoryNameAsync(string categoryName)
        {
            try
            {
                //check if input string is null.
                if (string.IsNullOrEmpty(categoryName))
                {
                    return Response<CategoryDTO>.Failure("category input is null");
                }

                //sent request to the database class.
                Response<Category> foundCategoryByCategoryNameResponse = await _categoryRepo.RGetCategoryByCategoryIdAsync(categoryName);

                //check if category found or not.
                if (!foundCategoryByCategoryNameResponse.IsSuccessfull)
                {
                    return Response<CategoryDTO>.Failure(foundCategoryByCategoryNameResponse?.ErrorMessage);
                }

                //mapp Category to CategoryDTO.
                CategoryDTO mappedCategoryToCategoryDTO = _mapper.Map<CategoryDTO>(foundCategoryByCategoryNameResponse.Value);

                return Response<CategoryDTO>.Success(mappedCategoryToCategoryDTO);
            }
            catch (Exception ex)
            {
                return Response<CategoryDTO>.Failure(ex.Message);
            }
        }

        public async Task<Response<List<CategoryDTO>>> GetAllCategoriesAsync()
        {
            try
            {
                //send request to data layer.
                Response<List<Category>> getAllCategoriesResponse = await _categoryRepo.RGetAllCategoriesAsync();

                //check response.
                if(!getAllCategoriesResponse.IsSuccessfull)
                {
                    return Response<List<CategoryDTO>>.Failure(getAllCategoriesResponse?.ErrorMessage);
                }

                List<CategoryDTO> categoriesResponse = new List<CategoryDTO>();
                //mapp all the category list to CategorDTOs
                foreach (Category category in getAllCategoriesResponse.Value)
                {
                    //mapp from Category to CategoryDTO.
                    CategoryDTO mappedCategory = _mapper.Map<CategoryDTO>(category);

                    mappedCategory.ProductCount = category.Products.Count();

                    categoriesResponse.Add(mappedCategory);
                }
                return Response<List<CategoryDTO>>.Success(categoriesResponse);
            }
            catch (Exception ex)
            {
                return Response<List<CategoryDTO>>.Failure(ex.Message);
            }
        }
    }
}
