using SchoolManage.DAL.Entities;

namespace SchoolManage.DAL.Mappers;

public class SubjectEntityMapper : IEntityMapper<SubjectEntity>
{
    public void MapToExistingEntity(SubjectEntity existingEntity, SubjectEntity newEntity)
    {
        existingEntity.Name = newEntity.Name;
        existingEntity.Acronym = newEntity.Acronym;
    }
}