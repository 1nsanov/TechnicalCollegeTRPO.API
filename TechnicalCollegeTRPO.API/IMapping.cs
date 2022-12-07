using AspTestStage.BaseClasses;
using TechnicalCollegeTRPO.API.Models.Dto;

namespace TechnicalCollegeTRPO.API;

public interface IMapping<TEntity, TDto>
where TEntity : EntityBase
where TDto : EntityDto, new()
{
    public abstract TDto MapToDto(TEntity entity);
}