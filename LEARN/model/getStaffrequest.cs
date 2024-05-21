using Mediator.Net.Contracts;

namespace LEARN.model
{
    public class getStaffrequest :IRequest
    {
        public string Major { get; set; }
    }

    public class getStaffresponse<T> : IResponse
    {
        public getStaffresponse(List<T> data)
        {
            Data = data;
        }
        public List<T> Data { get; set; }
       
    }

}
