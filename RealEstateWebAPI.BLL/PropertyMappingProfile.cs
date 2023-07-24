using AutoMapper;
using RealEstateWebAPI.BLL.DTO;
using RealEstateWebAPI.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.BLL
{/// <summary>
/// Mappimi i DTO me Entities
/// </summary>
    public class PropertyMappingProfile : Profile
    {
        public PropertyMappingProfile()
        {
            CreateMap<PropertyDTO, Property>().ReverseMap();
            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<CreateUpdateUserDTO, User>().ReverseMap();
            CreateMap<TransactionDTO, Transaction>().ReverseMap();
            CreateMap<RoleDTO, Role>().ReverseMap();
            CreateMap<MessageDTO, Message>().ReverseMap();
        }
    }
}