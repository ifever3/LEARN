using AutoMapper;
using LEARN.model;
using Mediator.Net.Context;
namespace LEARN.mappings
{
    public class staffmapping : Profile
    {
        public staffmapping() 
        {
            CreateMap<createStaffcommand, Staff>();
            CreateMap<updateStaffcommand, Staff>();
        }
    }
}
