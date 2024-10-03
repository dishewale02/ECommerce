﻿
namespace ECommerce.Models.ResponseModel
{
    public class Response<T> where T : class
    {
        public bool IsSuccessfull { get; set; }
        public string? ErrorMessage { get; set; }
        public IEnumerable<string>? ErrorsMessages { get; set; }
        public T? Value { get; set; }

        public Response()
        {

        }

        public Response(string? errorMessage)
        {
            ErrorMessage = errorMessage;
            Value = null;
            IsSuccessfull = false;
        }

        public Response(T entity)
        {
            ErrorsMessages = null;
            Value = entity;
            IsSuccessfull = true;
        }

        public Response(IEnumerable<string> errorsMessages)
        {
            ErrorsMessages = errorsMessages;
            Value = null;
            IsSuccessfull = false;
        }

        public Response<string> AddStringValue(string value)
        {
            return new Response<string>()
            {
                Value = value,
                IsSuccessfull = true
            };
        }
    }
}