using AutoMapper;
using TaskBoard.Application.Common.Dtos;
using TaskBoard.Domain.Entities;
using TaskEntity = TaskBoard.Domain.Entities.Task;

namespace TaskBoard.Application.Common.Mapping;

public class EntityMappingProfiles : Profile
{
    public EntityMappingProfiles()
    {
        CreateMap<User, UserDto>();
        CreateMap<TaskActivityLog, TaskActivityLogDto>();

        CreateMap<TaskEntity, TaskDto>()
            .ForMember(dest => dest.AssignedUsers, opt => opt.MapFrom(src => src.UserTasks.Select(ut => ut.User)))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString().ToLower()));

        CreateMap<Column, ColumnDto>();
        CreateMap<Board, BoardDto>()
            .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.UserBoards.Select(ub => ub.User)))
            .ForMember(dest => dest.IsOwner, opt => opt.Ignore());

    }
}