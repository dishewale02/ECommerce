
using ECommerce.Models.DataModels.InfoModel;
using ECommerce.Models.InputModelsDTO.AuthInputModelsDTO;
using ECommerce.Models.ResponseModel;

namespace ECommerce.Services.Interfaces.RepoServiceInterfaces.GenericRepoServiceInterface
{
    public interface IGenericRepoService<TSource, TTarget> where TSource : IIdentityModel where TTarget : GenericInfo
    {
        Task<Response<TSource>> CreateAsync(TSource entity, UserClaimModel userClaimModel);
        Task<Response<TSource>> UpdateAsync(TSource entity);
        Task<Response<TSource>> DeleteAsync(string id);
        Task<Response<TSource>> GetAsync(string id);
        Task<Response<IEnumerable<TSource>>> GetAllAsync();
    }
}
