using Mediator.Net.Contracts;

namespace LEARN.authentication
{
    public class BaseResponse<T> : IResponse
    {
        public BaseResponse()
        {
        }

        public BaseResponse(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}