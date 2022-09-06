using AutoMapper;

namespace Services
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ItemDto, ItemModel>();
            CreateMap<ItemModel, ItemDto>();
        }
    }
}
