using AutoMapper;
using CatDataBank.Model.Dto;
using CatDataBank.Model;

namespace CatDataBank.Helper
{
    public interface IAutoMapperProfile { IMapper GetMapper(); }
    public class AutoMapperProfile : IAutoMapperProfile
    {
        private IMapper _mapper;
        public AutoMapperProfile()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<UserDto, User>();
            });
            _mapper = config.CreateMapper();
        }

        public IMapper GetMapper()
        {
            return _mapper;
        }
    }
}