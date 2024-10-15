
namespace ECommerce.Models.ResponseModel
{
    public class Response<T>
    {
        public bool IsSuccessfull { get; set; }
        public string? ErrorMessage { get; set; }
        public IEnumerable<string>? ErrorsMessages { get; set; }
        public T Value { get; set; }

        public Response()
        {

        }

        public static Response<T> Success(T entity)
        {
            return new Response<T>()
            {
                Value = entity,
                IsSuccessfull = true,
                ErrorMessage = null
            };
        }

        public static Response<T> Failure(string? errorMessage)
        {
            return new Response<T>()
            {
                ErrorMessage = errorMessage,
                IsSuccessfull = false,
                Value = default(T)
            };
        }

        public static Response<bool> Success(bool successfull)
        {
            return new Response<bool>()
            {
                Value = successfull,
                IsSuccessfull = successfull,
                ErrorMessage= null
            };
        }
    }
}
