using ECommerce.Data;
using ECommerce.Models.DataModels.InfoModel;
using ECommerce.Models.ResponseModel;
using ECommerce.Repo.Interfaces.GenericRepoInterface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ECommerce.Repo.Classes.GenericRepoClass
{
    public class GenericRepo<T> : IGenericRepo<T> 
        where T : GenericInfo
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public GenericRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public virtual async Task<Response<T>> RCreateAsync(T entity)
        {
            //check if input is null.
            if (entity == null)
            {
                return Response<T>.Failure("input entity is blank.");
            }

            //save entity in database.
            EntityEntry<T> createEntityResponse = await _dbSet.AddAsync(entity);

            //Get response entry.
            T response = createEntityResponse.Entity;

            //check if response is null.
            if (response == null)
            {
                return Response<T>.Failure("entity can't save. Internal server error.");
            }

            //save changes
            await _dbContext.SaveChangesAsync();

            return Response<T>.Success(response);
        }

        public async Task<Response<T>> RSoftDeleteAsync(string? id)
        {
            if(id == null)
            {
                return Response<T>.Failure("input id is empty.");
            }

            //check if entity available in database.
            T? findEntityInDatabaseResponse = await _dbSet.FindAsync(id);

            if (findEntityInDatabaseResponse == null)
            {
                return Response<T>.Failure("not found in database to delete.");
            }

            //EntityEntry<T> deleteEntityResponse = _dbSet.Remove(findEntityInDatabaseResponse);

            findEntityInDatabaseResponse.IsDeleted = true;
            findEntityInDatabaseResponse.IsActive = false;

            Response<T> updateDeletedEntityInDatabaseREsponse = await RUpdateAsync(findEntityInDatabaseResponse);

            //get entity from response.
            T response = updateDeletedEntityInDatabaseREsponse.Value;

            //check if response is null.
            if(response == null)
            {
                return Response<T>.Failure("internal server error while deleting.");
            }

            //save changes
            await _dbContext.SaveChangesAsync();

            return Response<T>.Success(response);
        }

        public async Task<Response<IEnumerable<T>>> RGetAllAsync()
        {
            //find all the entity from database.
            IEnumerable<T> values = await _dbSet.ToListAsync();

            if (values == null)
            {
                return Response<IEnumerable<T>>.Failure("no entry found.");
            }

            return Response<IEnumerable<T>>.Success(values);
        }

        public async Task<Response<T>> RGetAsync(string id)
        {
            if (id == null)
            {
                return Response<T>.Failure("id is null.");
            }

            //find entity in database using id.
            T? foundInDatabase = await _dbSet.FindAsync(id);
            if (foundInDatabase == null)
            {
                return Response<T>.Failure("entity not found.");
            }

            return Response<T>.Success(foundInDatabase);
        }

        public async Task<Response<T>> RUpdateAsync(T entity)
        {
            if (entity == null)
            {
                return Response<T>.Failure("Input entity is null.");
            }

            // Retrieve the existing entity from the database
            T? existingEntity = await _dbSet.FindAsync(entity.Id);

            if (existingEntity is null)
            {
                return Response<T>.Failure("Entity not found.");
            }

            // Update properties of the existing entity with values from the input entity
            _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);

            // Save the changes
            await _dbContext.SaveChangesAsync();

            return Response<T>.Success(existingEntity);
        }
    }
}
