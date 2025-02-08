using SchoolManage.DAL.Entities;

namespace SchoolManage.DAL.Mappers;

public class EvaluationEntityMapper : IEntityMapper<EvaluationEntity>
{
    public void MapToExistingEntity(EvaluationEntity existingEntity, EvaluationEntity newEntity)
    {
        existingEntity.Points = newEntity.Points;
        existingEntity.Description = newEntity.Description;
    }
}