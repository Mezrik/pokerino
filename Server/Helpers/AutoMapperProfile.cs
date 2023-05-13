using System;
using AutoMapper;
using Pokerino.Shared.Entities;
using Pokerino.Shared.Models.Users;
using Pokerino.Shared.Models.Rooms;

namespace Pokerino.Server.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // UserCreateRequest -> User
            CreateMap<UserCreateRequest, User>();

            // UserUpdateRequest -> User
            CreateMap<UserUpdateRequest, User>()
                .ForAllMembers(x => x.Condition(
                    (src, dest, prop) =>
                    {
                        // ignore both null & empty string properties
                        if (prop == null) return false;
                        if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                        return true;
                    }
                ));

            // RoomCreateRequest -> Room
            CreateMap<RoomCreateRequest, Room>();
        }
    }
}

