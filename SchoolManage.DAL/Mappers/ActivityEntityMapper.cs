using SchoolManage.DAL.Entities;

namespace SchoolManage.DAL.Mappers;

public class ActivityEntityMapper : IEntityMapper<ActivityEntity>
{
    public void MapToExistingEntity(ActivityEntity existingEntity, ActivityEntity newEntity)
    {
        existingEntity.Start = newEntity.Start;
        existingEntity.End = newEntity.End;
        existingEntity.Room = newEntity.Room;
        existingEntity.Description = newEntity.Description;
        existingEntity.Type = newEntity.Type;
    }
}