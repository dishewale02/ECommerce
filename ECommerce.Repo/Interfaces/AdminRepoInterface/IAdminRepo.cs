using ECommerce.Models.DataModels.AuthDataModels;
using ECommerce.Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Repo.Interfaces.AdminRepoInterface
{
    public interface IAdminRepo
    {
        Task<Response<User>> RActivateUserAsync(string userId);
        Task<Response<List<User>>> RGetDeletedAndNonActiveUsers();
    }
}
