using AutoMapper;
using ECommerce.Models.DataModels.InfoModel;
using ECommerce.Models.InputModelsDTO.AuthInputModelsDTO;
using ECommerce.Models.ResponseModel;
using ECommerce.Repo.Interfaces.GenericRepoInterface;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.GenericRepoServiceInterface;

namespace ECommerce.Services.Classes.RepoServiceClasses.GenericRepoServiceClass
{
    public class GenericRepoService<TSource, TTarget> : IGenericRepoService<TSource, TTarget>
        where TSource : IIdentityModel where TTarget : GenericInfo
    {
        protected readonly IGenericRepo<TTarget> _genericRepo;
        protected readonly IMapper _mapper;

        public GenericRepoService(IGenericRepo<TTarget> genericRepo, IMapper mapper)
        {
            _genericRepo = genericRepo;
            _mapper = mapper;
        }

        public virtual async Task<Response<TSource>> CreateAsync(TSource entity, UserClaimModel userClaimModel)
        {
            try
            {
                //check if input entity is null.
                if (entity is null || userClaimModel is null)
                {
                    return Response<TSource>.Failure("entity can not be null.");
                }

                //send entity to mapp.
                TTarget tSourceToTTargetMapped = _mapper.Map<TSource, TTarget>(entity);
                
                //fill all the required information.
                tSourceToTTargetMapped.Id = Guid.NewGuid().ToString();
                tSourceToTTargetMapped.CreatedOn = DateTime.Now;

                tSourceToTTargetMapped.CreatedBy = userClaimModel.UserName;
                tSourceToTTargetMapped.IsDeleted = false;
                tSourceToTTargetMapped.IsActive = true;

                //send entity to save in database.
                Response<TTarget> saveEntityInDatabaseResponse = await _genericRepo.RCreateAsync(tSourceToTTargetMapped);

                //mapp response entity to TTarget model.

                TSource targetEntityMapped = _mapper.Map<TTarget, TSource>(saveEntityInDatabaseResponse.Value);

                //check if response is successfull.
                if (!saveEntityInDatabaseResponse.IsSuccessfull)
                {
                    return Response<TSource>.Failure(saveEntityInDatabaseResponse.ErrorMessage);
                }

                return Response<TSource>.Success(targetEntityMapped);
            }
            catch (Exception ex)
            {
                return Response<TSource>.Failure(ex.Message);
            }
        }

        public virtual async Task<Response<TSource>> DeleteAsync(string id)
        {
            try
            {
                //check if input id is null.
                if (string.IsNullOrEmpty(id))
                {
                    return Response<TSource>.Failure("input id is null.");
                }

                //send request to delete entity.
                Response<TTarget> deleteEntitytResponse = await _genericRepo.RDeleteAsync(id);

                //check Response to return
                if (!deleteEntitytResponse.IsSuccessfull)
                {
                    return Response<TSource>.Failure(deleteEntitytResponse.ErrorMessage);
                }

                //mapp deleteEntityModel to TTarget model.
                TSource TSourceToTTargetMapped = _mapper.Map<TTarget, TSource>(deleteEntitytResponse.Value);

                if (TSourceToTTargetMapped == null)
                {
                    return Response<TSource>.Failure("internal mapping error.");
                }

                return Response<TSource>.Success(TSourceToTTargetMapped);
            }
            catch (Exception ex)
            {
                return Response<TSource>.Failure(ex.Message);
            }
        }

        public virtual async Task<Response<IEnumerable<TSource>>> GetAllAsync()
        {
            try
            {
                //send request to Get All entries from database.
                Response<IEnumerable<TTarget>> getAllModelResponse = await _genericRepo.RGetAllAsync();

                List<TSource> result = new List<TSource>();

                //convert all entries into TTarget model.
                foreach (TTarget item in getAllModelResponse.Value)
                {
                    //mapp TSource to TTarget.
                    TSource mappedResult = _mapper.Map<TTarget, TSource>(item);

                    result.Add(mappedResult);
                }

                if (result is null)
                {
                    return Response<IEnumerable<TSource>>.Failure(getAllModelResponse.ErrorMessage);
                }

                return Response<IEnumerable<TSource>>.Success(result);
            }
            catch(Exception ex)
            {
                return Response<IEnumerable<TSource>>.Failure(ex.Message);
            }
        }
        
        public virtual async Task<Response<TSource>> GetAsync(string id)
        {
            try
            {
                //check if input is null.
                if(string.IsNullOrEmpty(id))
                {
                    return Response<TSource>.Failure("input id is not provided.");
                }

                //send a request to Repo service with input id.
                Response<TTarget> findEntityResponse = await _genericRepo.RGetAsync(id);

                //check response.
                if(!findEntityResponse.IsSuccessfull)
                {
                    return Response<TSource>.Failure(findEntityResponse.ErrorMessage);
                }

                //map TSource to TTarget.
                TSource mappedTSourceToTTarget = _mapper.Map<TTarget, TSource>(findEntityResponse.Value);

                if (mappedTSourceToTTarget == null)
                {
                    return Response<TSource>.Failure("internal mapping error.");
                }

                return Response<TSource>.Success(mappedTSourceToTTarget);
            }
            catch (Exception ex)
            {
                return Response<TSource>.Failure(ex.Message);
            }
        }

        public virtual async Task<Response<TSource>> UpdateAsync(TSource entity)
        {
            try
            {
                //check input entity.
                if (entity == null)
                {
                    return Response<TSource>.Failure("input entity is blank.");
                }

                //mapp input entity.
                TTarget tSourceToTTargetMapped = _mapper.Map<TSource, TTarget>(entity);

                //send request to RepoClass for update.
                Response<TTarget> updateEntityResponse = await _genericRepo.RUpdateAsync(tSourceToTTargetMapped);

                //check response.
                if(!updateEntityResponse.IsSuccessfull)
                {
                    return Response<TSource>.Failure(updateEntityResponse.ErrorMessage);
                }

                //mapp TSource to TTarget.
                TSource mappedTTargetFromTSource = _mapper.Map<TTarget, TSource>(updateEntityResponse.Value);

                if(mappedTTargetFromTSource == null)
                {
                    return Response<TSource>.Failure("internal error.");
                }

                return Response<TSource>.Success(mappedTTargetFromTSource);
            }
            catch (Exception ex)
            {
                return Response<TSource>.Failure(ex.Message);
            }
        }
    }
}
