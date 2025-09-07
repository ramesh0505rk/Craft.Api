using AutoMapper;
using Craft.Application.Operations.Commands.Requests;
using Craft.Application.Operations.Queries.Requests;
using Craft.Application.ResponseDTOs;
using Craft.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Application.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            #region Commands
            CreateMap<SignUpCommand, SignUpRequest>();
            #endregion

            #region Queries
            CreateMap<GetUserListQuery, GetUserListRequest>();
            CreateMap<GetUserPreferencesResponse, GetUserPreferencesDTO>();
            CreateMap<GetUserPreferencesQuery, GetUserPreferencesRequest>();
            #endregion
        }
    }
}
