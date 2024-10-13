using ECommerce.Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Repo.Interfaces.GenericRepoInterface
{
    public interface IGenericRepo<T> 
        where T : class
    {
        Task<Response<T>> RCreateAsync(T entity);
        Task<Response<T>> RUpdateAsync(T entity);
        Task<Response<T>> RDeleteAsync(string? id);
        Task<Response<T>> RGetAsync(string? id);
        Task<Response<IEnumerable<T>>> RGetAllAsync();
    }
}
