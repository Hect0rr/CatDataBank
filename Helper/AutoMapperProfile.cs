using AutoMapper;
using CatDataBank.Model.Dto;
using CatDataBank.Model;

namespace CatDataBank.Helper
{
    public class AutoMapperProfile
    {
        public AutoMapperProfile()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<UserDto, User>();
            });
            IMapper mapper = config.CreateMapper();
        }
    }
}